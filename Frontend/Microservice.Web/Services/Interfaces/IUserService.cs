using Microservice.Web.Models;
using System.Threading.Tasks;

namespace Microservice.Web.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserViewModel> GetUser();
    }
}
