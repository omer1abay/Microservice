using AutoMapper;
using MediatR;
using Microservice.Services.Order.Application.DTOs;
using Microservice.Services.Order.Application.Mapping;
using Microservice.Services.Order.Application.Queries;
using Microservice.Services.Order.Infrastructure;
using Microservice.Shared.Dtos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Microservice.Services.Order.Application.Handlers
{
    //db'ye gidip veriyi alacak sınıf
    public class GetOrdersByUserIdQueryHandler : IRequestHandler<GetOrdersByUserIdQuery, ResponseDto<List<OrderDto>>>
    {
        private readonly OrderDbContext _context;

        public GetOrdersByUserIdQueryHandler(OrderDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseDto<List<OrderDto>>> Handle(GetOrdersByUserIdQuery request, CancellationToken cancellationToken)
        {
            var orders = await _context.Orders.Include(x=> x.OrderItems).Where(x=> x.BuyerId == request.UserId).ToListAsync();

            if (!orders.Any())
            {
                return ResponseDto<List<OrderDto>>.Success(new List<OrderDto>(),200);
            }

            var ordersDto = ObjectMapper.Mapper.Map<List<OrderDto>>(orders);
            return ResponseDto<List<OrderDto>>.Success(ordersDto, 200);
        }
    }
}
