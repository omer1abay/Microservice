using Microservice.Web.Exceptions;
using Microservice.Web.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Microservice.Web.Handler
{
    public class ResourceOwnerPasswordTokenHandler:DelegatingHandler
    {
        //her istek yapıldığında access token ve refresh token'ı getirip kontrol edecek olan handler'ımız

        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IIdentityService identityService;
        private readonly ILogger<ResourceOwnerPasswordTokenHandler> logger;

        public ResourceOwnerPasswordTokenHandler(IHttpContextAccessor httpContextAccessor, IIdentityService identityService, ILogger<ResourceOwnerPasswordTokenHandler> logger)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.identityService = identityService;
            this.logger = logger;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            //istek başlayınca araya girecek metot
            var accesToken = await httpContextAccessor.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);


            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer",accesToken);
            
            var response = await base.SendAsync(request, cancellationToken);

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                var tokenResponse = await identityService.GetAccessTokenByRefreshToken();

                if (tokenResponse != null)
                {
                    request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer",tokenResponse.AccessToken);
                    response = await base.SendAsync(request, cancellationToken);
                }

            }

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                //hata fırlat login ekranına göndermesi için burada redirect olmaz

                throw new UnauthorizedException(); //middleware üzerinde bu tipte hata gelirse kontrolünü sağlayıp yönlendirme yapacağız

            }


            return response;
        }


    }
}
