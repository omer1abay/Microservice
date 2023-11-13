using IdentityModel.Client;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Microservice.Gateway.DelegateHandler
{
    public class TokenExchangeDelegateHandler:DelegatingHandler
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration configuration;
        private string _token;

        public TokenExchangeDelegateHandler(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            this.configuration = configuration;
        }

        //token al
        private async Task<string> GetToken(string RequestToken)
        {
            if (!string.IsNullOrEmpty(_token))
            {
                return _token;
            }

            var disco = await _httpClient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
            {
                Address = configuration["IdentityServerURL"],
                Policy = new DiscoveryPolicy { RequireHttps = false },
            });

            if (disco.IsError)
            {
                throw disco.Exception;
            }

            TokenExchangeTokenRequest tokenExchangeTokenRequest = new TokenExchangeTokenRequest
            {
                Address = disco.TokenEndpoint,
                ClientId = configuration["ClientId"],
                ClientSecret = configuration["ClientSecret"],
                GrantType = configuration["GrantType"],
                SubjectToken=RequestToken,
                SubjectTokenType = "urn:ietf:params:oauth:token-type:access-token",
                Scope = "openid discount_fullpermission fake_payment_fullpermission"
            };

            var tokenResponse = await _httpClient.RequestTokenExchangeTokenAsync(tokenExchangeTokenRequest);

            if (tokenResponse.IsError)
            {
                throw tokenResponse.Exception;
            }

            _token = tokenResponse.AccessToken;

            return _token;


        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var requestToken = request.Headers.Authorization.Parameter; //eski token

            var newToken = await GetToken(requestToken); //yeni token

            request.SetBearerToken(newToken);

            return await base.SendAsync(request, cancellationToken);
        }

    }
}
