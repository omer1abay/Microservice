using MassTransit;
using Microservice.Services.FakePayment.Models;
using Microservice.Shared.ControllerBases;
using Microservice.Shared.Dtos;
using Microservice.Shared.Messages;
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
        private readonly ISendEndpointProvider sendEndpointProvider;

        public FakePaymentController(ISendEndpointProvider sendEndpointProvider)
        {
            this.sendEndpointProvider = sendEndpointProvider;
        }

        [HttpPost]
        public async Task<IActionResult> ReceivePayment(PaymentDto paymentDto)
        {
            var sendEndpoint = await sendEndpointProvider.GetSendEndpoint(new Uri("queue:order-created-service"));

            var createOrderMessageCommand = new CreateOrderMessageCommand()
            {
                BuyerId = paymentDto.Order.BuyerId,
                Province = paymentDto.Order.Address.Province,
                District = paymentDto.Order.Address.District,
                Street = paymentDto.Order.Address.Street,
                Line = paymentDto.Order.Address.Line,
                ZipCode = paymentDto.Order.Address.ZipCode,
            };

            paymentDto.Order.OrderItems.ForEach(item =>
            {
                createOrderMessageCommand.OrderItems.Add(new OrderItem
                {
                    PictureUrl = item.PictureUrl,
                    Price = item.Price,
                    ProductId = item.ProductId,
                    ProductName = item.ProductName,
                }
                );
            });

            //mesaj gönderme
            await sendEndpoint.Send<CreateOrderMessageCommand>(createOrderMessageCommand);

            return CreateActionResultInstance<NoContent>(ResponseDto<NoContent>.Success(200));
        }
    }
}
