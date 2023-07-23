using IdentityModel.Client;
using Microservice.Shared.Dtos;
using Microservice.Web.Models;
using Microservice.Web.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

namespace Microservice.Web.Services
{
    public class IdentityService : IIdentityService
    {

        private readonly HttpClient httpClient;
        //cookie erişim için
        private readonly IHttpContextAccessor httpContextAccessor;
        private ClientSettings clientSettings;
        private ServiceApiSettings serviceApiSettings;

        public IdentityService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor, IOptions<ClientSettings> clientSettings, IOptions<ServiceApiSettings> serviceApiSettings)
        {
            this.httpClient = httpClient;
            this.httpContextAccessor = httpContextAccessor;
            this.clientSettings = clientSettings.Value;
            this.serviceApiSettings = serviceApiSettings.Value;
        }

        public async Task<TokenResponse> GetAccessTokenByRefreshToken()
        {
            //http://localhost:5001/connect/token
            //identitymodel -> oauth 2.0 olan identityserver'a istek atmamızı sağlar
            //GetDiscoveryDocumentAsync bize identityserver'daki metotları getiren api'dir
            var discovery = await httpClient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
            {
                Address = serviceApiSettings.IdentityBaseUri,
                Policy = new DiscoveryPolicy { RequireHttps = false }
            }); //belirtmezsek default https ile istek yapar

            if (discovery.IsError)
            {
                throw discovery.Exception;
            }

            var resfreshToken = await httpContextAccessor.HttpContext.GetTokenAsync
                (OpenIdConnectParameterNames.RefreshToken);

            //refresh-token request 
            RefreshTokenRequest refreshTokenRequest = new()
            {
                ClientId = clientSettings.WebClientForUser.ClientId,
                ClientSecret = clientSettings.WebClientForUser.ClientSecret,
                RefreshToken = resfreshToken,
                Address = discovery.TokenEndpoint
            };

            var token = await httpClient.RequestRefreshTokenAsync(refreshTokenRequest);

            if (token.IsError)
            {
                return null;
            }

            //cookie'de olduğu için tekrar oluşturmamıza gerek yok
            var authTokens = new List<AuthenticationToken>()
            {
                new AuthenticationToken{Name = OpenIdConnectParameterNames.AccessToken,Value = token.AccessToken},
                new AuthenticationToken{Name = OpenIdConnectParameterNames.RefreshToken,Value = token.RefreshToken},
                new AuthenticationToken{Name = OpenIdConnectParameterNames.ExpiresIn,Value = DateTime.Now.AddSeconds(token.ExpiresIn).ToString("o",CultureInfo.InvariantCulture)} //culture'ı yok saydık
            };

            var authResult = await httpContextAccessor.HttpContext.AuthenticateAsync();
            var properties = authResult.Properties;

            properties.StoreTokens(authTokens);

            await httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,authResult.Principal,properties);

            return token;
        }

        public async Task RevokeRefreshToken()
        {
            //kullanıcı çıkış yapınca refresh token'ı db'den sileriz
            var discovery = await httpClient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
            {
                Address = serviceApiSettings.IdentityBaseUri,
                Policy = new DiscoveryPolicy { RequireHttps = false }
            }); //belirtmezsek default https ile istek yapar

            if (discovery.IsError)
            {
                throw discovery.Exception;
            }

            var refreshToken = await httpContextAccessor.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken);

            TokenRevocationRequest tokenRevocationRequest = new()
            {
                ClientId = clientSettings.WebClientForUser.ClientId,
                ClientSecret = clientSettings.WebClientForUser.ClientSecret,
                Address = discovery.RevocationEndpoint,
                Token = refreshToken,
                TokenTypeHint = "refresh_token"
            };

            await httpClient.RevokeTokenAsync(tokenRevocationRequest);

        }

        public async Task<ResponseDto<bool>> SignIn(SignInInput signIn)
        {
            //http://localhost:5001/connect/token
            //identitymodel -> oauth 2.0 olan identityserver'a istek atmamızı sağlar
            //GetDiscoveryDocumentAsync bize identityserver'daki metotları getiren api'dir
            var discovery = await httpClient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
            {
                Address = serviceApiSettings.IdentityBaseUri,
                Policy = new DiscoveryPolicy { RequireHttps = false }
            }); //belirtmezsek default https ile istek yapar

            if (discovery.IsError)
            {
                throw discovery.Exception;
            }

            //kullanıcı login'i gerekli olan token isteği
            var passwordTokenRequest = new PasswordTokenRequest
            {
                ClientId = clientSettings.WebClientForUser.ClientId,
                ClientSecret = clientSettings.WebClientForUser.ClientSecret,
                UserName = signIn.Email,
                Password = signIn.Password,
                Address = discovery.TokenEndpoint
            };

            var tokenResponse = await httpClient.RequestPasswordTokenAsync(passwordTokenRequest);

            if (tokenResponse.IsError)
            {
                var responseContent = await tokenResponse.HttpResponse.Content.ReadAsStringAsync();
                var errorDto = JsonSerializer.Deserialize<ErrorDto>(responseContent,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }); //jsonserializer casesensitive çalışır buna policy ekleyerek insensitive yapıyoruz
                return ResponseDto<bool>.Failure(errorDto.Errors, 400);
            }

            //userinfo endpointine istek yaparsak o bize token değerlerini verir
            var userInfoRequest = new UserInfoRequest
            {
                Token = tokenResponse.AccessToken,
                Address = discovery.UserInfoEndpoint
            };


            var userInfo = await httpClient.GetUserInfoAsync(userInfoRequest);

            if (userInfo.IsError)
            {
                throw userInfo.Exception;
            }

            //identity-model'in bize sunduğu güzellikler
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(userInfo.Claims,CookieAuthenticationDefaults.AuthenticationScheme,"name","role"); //cookie içerisindeki değerleri oluşturuyoruz, cookie kimliği

            ClaimsPrincipal principal = new ClaimsPrincipal(claimsIdentity); //cookie oluşturmak için gerekli sınıf, temeli

            var authProperties = new AuthenticationProperties();
            authProperties.StoreTokens(new List<AuthenticationToken>()
            {
                new AuthenticationToken{Name = OpenIdConnectParameterNames.AccessToken,Value = tokenResponse.AccessToken},
                new AuthenticationToken{Name = OpenIdConnectParameterNames.RefreshToken,Value = tokenResponse.RefreshToken},
                new AuthenticationToken{Name = OpenIdConnectParameterNames.ExpiresIn,Value = DateTime.Now.AddSeconds(tokenResponse.ExpiresIn).ToString("o",CultureInfo.InvariantCulture)} //culture'ı yok saydık
            });

            authProperties.IsPersistent = signIn.IsRemember; //kalıcı bir token mı yapıyoruz eğer isremember açık ise kullanıcı bilgileri hatırlanacak 60 gün kalıcı cookie durur

            //cookie oluşturmaya hazırız
            await httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,principal,authProperties);

            return ResponseDto<bool>.Success(200);
        }
    }
}