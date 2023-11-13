using System;
using System.Collections.Generic;
using System.Text;

namespace Microservice.Shared.Messages
{
    public class CreateOrderMessageCommand
    {

        public CreateOrderMessageCommand()
        {
            OrderItems = new List<OrderItem>();
        }

        //mesajı kim işleyecek? order bir siparişin oluşması için gerekli alanlar
        public string BuyerId { get; set; }
        public List<OrderItem> OrderItems { get; set; }
        public string Province { get; set; } //set etme kapalı
        public string District { get; set; }
        public string Street { get; set; }
        public string ZipCode { get; set; }
        public string Line { get; set; }

    }

    public class OrderItem
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public string PictureUrl { get; set; }
        public Decimal Price { get; set; }
    }

}
