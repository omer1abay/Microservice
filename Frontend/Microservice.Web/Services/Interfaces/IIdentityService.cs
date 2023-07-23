using IdentityModel.Client;
using Microservice.Shared.Dtos;
using Microservice.Web.Models;
using System.Threading.Tasks;

namespace Microservice.Web.Services.Interfaces
{
    public interface IIdentityService
    {
        Task<ResponseDto<bool>> SignIn(SignInInput signIn);
        Task<TokenResponse> GetAccessTokenByRefreshToken();
        Task RevokeRefreshToken();
    }
}
