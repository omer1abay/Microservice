using Microservice.Web.Models;
using Microservice.Web.Services.Interfaces;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Microservice.Web.Services
{
    public class UserService : IUserService
    {
        private readonly HttpClient httpClient;

        public UserService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<UserViewModel> GetUser()
        {
            return await httpClient.GetFromJsonAsync<UserViewModel>("/api/user/getuser"); //getfromjson yeni gelen bir özellik otomatik deserialzie işlemi gerçekleştiriyor
        }
    }
}