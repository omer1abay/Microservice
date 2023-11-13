using Microservice.Web.Models;
using Microservice.Web.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Microservice.Web.Controllers
{
    public class AuthController : Controller
    {

        private readonly IIdentityService _identityService;

        public AuthController(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(SignInInput signInInput)
        {
            if (!ModelState.IsValid) //modelstate geçerliliği için required olarak işaretlemeliyiz hata mesajını da verebiliriz biz default olanı kullandık
            {

                return View();
            }

            var response = await _identityService.SignIn(signInInput);

            if (!response.IsSuccess)
            {
                response.Errors.ForEach(x =>
                {
                    ModelState.AddModelError(string.Empty, x); //string empty olmasının sebebi hangisi yanlış ise onu dönmemesi(şifre ve kullanıcı adı yanlış gibi genel bir kullanım dönmeliyiz)
                });
                
                return View();
            }


            return RedirectToAction(nameof(Index), "Home");
            
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            //refresh token silü
            await _identityService.RevokeRefreshToken();
            return RedirectToAction(nameof(HomeController.Index), "Home"); //anasayfaya gönder
        }

    }
}
