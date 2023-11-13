using MediatR;
using Microservice.Services.Order.Application.Command;
using Microservice.Services.Order.Application.DTOs;
using Microservice.Services.Order.Infrastructure;
using Microservice.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Microservice.Services.Order.Application.Handlers
{
    //OrderDto orderDto = new(); //If there is a default constructor, we can use it directly with the new keyword Feature introduced with C#9.0
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, ResponseDto<CreatedOrderDto>>
    {
        private readonly OrderDbContext _context;

        public CreateOrderCommandHandler(OrderDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseDto<CreatedOrderDto>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var newAddress = new Domain.OrderAggregate.Address(request.Address.Province,
                request.Address.District,request.Address.Street,
                request.Address.ZipCode,request.Address.Line);

            var orders = new Domain.OrderAggregate.Order(request.BuyerId,newAddress);

            request.OrderItems.ForEach(x=>
                orders.AddOrderItem(x.ProductId,x.ProductName,x.Price,x.PictureUrl)
            );

            _context.Orders.Add(orders);
            await _context.SaveChangesAsync();
            return ResponseDto<CreatedOrderDto>.Success(new CreatedOrderDto { OrderId=orders.Id} ,200);

        }
    }
}
