using Microservice.Web.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Microservice.Web.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        public async  Task<IActionResult> Index()
        {
            return View(await userService.GetUser());
        }

    }
}
