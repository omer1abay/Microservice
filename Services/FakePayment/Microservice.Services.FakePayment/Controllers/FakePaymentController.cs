using Microservice.Shared.ControllerBases;
using Microservice.Shared.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservice.Services.FakePayment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FakePaymentController : CustomControllerBase
    {
        [HttpPost]
        public IActionResult ReceivePayment()
        {
            return CreateActionResultInstance<NoContent>(ResponseDto<NoContent>.Success(200));
        }
    }
}
