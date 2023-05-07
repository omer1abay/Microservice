using AutoMapper;
using Microservice.Services.Order.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microservice.Services.Order.Application.Mapping
{
    public class CustomMapping:Profile
    {
        //entity - dto maplemesi
        public CustomMapping()
        {
            //tam tersi bir mapleme de olabileceği için ReverseMap yaptık
            CreateMap<Order.Domain.OrderAggregate.Order, OrderDto>().ReverseMap();
            CreateMap<Order.Domain.OrderAggregate.OrderItem, OrderItemDto>().ReverseMap();
            CreateMap<Order.Domain.OrderAggregate.Address, AddressDto>().ReverseMap();
        }
    }
}
