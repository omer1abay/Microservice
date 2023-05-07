﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservice.Services.Basket.DTOs
{
    public class BasketDto
    {
        public string UserId { get; set; }
        public string DiscountCode { get; set; }
        public List<BasketItemDto> BasketItems { get; set; }
        public decimal TotalPrice 
        { 
            get => BasketItems.Sum(x=> x.Quantity * x.Price); 
        }
    }
}
