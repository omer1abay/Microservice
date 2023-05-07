using Microservice.IdentityServer.DTOs;
using Microservice.IdentityServer.Models;
using Microservice.Shared.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using static IdentityServer4.IdentityServerConstants;

namespace Microservice.IdentityServer.Controllers
{
    [Authorize(LocalApi.PolicyName)]
    [Route("api/[controller]/[action]")] //api/user/metotAdi
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly UserManager<ApplicationUser> _userManager;

        public UserController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }


        [HttpPost]
        public async Task<IActionResult> SignUp(SignupDto signupDto)
        {
            var user = new ApplicationUser
            {
                UserName = signupDto.Username,
                City = signupDto.City,
                Email = signupDto.Email
            };

            var result = await _userManager.CreateAsync(user,signupDto.Password); //şifreyi burada verdik çünkü işlenmesi gerekiyor

            if (!result.Succeeded)
            {
                return BadRequest(ResponseDto<NoContent>.Failure(result.Errors.Select(x=> x.Description).ToList(),400));
            }

            return NoContent();

        }

        [HttpGet]
        public async Task<IActionResult> GetUser()
        {
            var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub); //sub bilgisi token içerisinden bize id'yi verir bu tarz bilgileri jwt.io'da token'ı decrypt edip görebiliriz 

            if (userIdClaim == null)
            {
                return BadRequest();
            }

            var user = await _userManager.FindByIdAsync(userIdClaim.Value);

            if (user == null)
            {
                return BadRequest();
            }

            return Ok(new { Id = user.Id, UserName = user.UserName, Email = user.Email, City = user.City });

        }

    }
}
