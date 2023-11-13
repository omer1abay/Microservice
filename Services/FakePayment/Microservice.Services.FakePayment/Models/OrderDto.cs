using System.Collections.Generic;

namespace Microservice.Services.FakePayment.Models
{
    public class OrderDto
    {
        public OrderDto()
        {
            OrderItems = new List<OrderItemDto>();
        }

        public string BuyerId { get; set; }
        public List<OrderItemDto> OrderItems { get; set; }

        public AddressDto Address { get; set; }

    }

    public class OrderItemDto
    {
        public string ProductId { get; private set; }
        public string ProductName { get; private set; }
        public string PictureUrl { get; private set; }
        public decimal Price { get; private set; }

    }

    public class AddressDto
    {
        public string Province { get; set; } 
        public string District { get; set; }
        public string Street { get; set; }
        public string ZipCode { get; set; }
        public string Line { get; set; }
    }

}
