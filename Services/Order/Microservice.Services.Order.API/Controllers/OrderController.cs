using MediatR;
using Microservice.Services.Order.Application.Command;
using Microservice.Services.Order.Application.Queries;
using Microservice.Shared.ControllerBases;
using Microservice.Shared.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservice.Services.Order.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : CustomControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ISharedIdentityService _sharedIdentityService;

        public OrderController(IMediator mediator, ISharedIdentityService sharedIdentityService)
        {
            _mediator = mediator;
            _sharedIdentityService = sharedIdentityService;
        }

        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            //It finds the IRequestHandler of this query type and executes the Handle method.
            var response = await _mediator.Send(new GetOrdersByUserIdQuery { UserId = _sharedIdentityService.GetUserId});
            return CreateActionResultInstance(response);
        }

        [HttpPost]
        public async Task<IActionResult> SaveOrder(CreateOrderCommand command)
        {
            //It finds the IRequestHandler of this command type and executes the Handle method.
            var response = await _mediator.Send(command);
            return CreateActionResultInstance(response);
        }

    }
}
