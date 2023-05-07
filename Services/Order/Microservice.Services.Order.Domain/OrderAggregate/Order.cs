using Microservice.Services.Order.Domain.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microservice.Services.Order.Domain.OrderAggregate
{
    public class Order:Entity, IAggregateRoot
    {
        public DateTime CreatedDate { get; set; }
        public Address Address { get; set; } //Biz eğer EFCore'a ayrıyeten belirtmezsek EFCore bu feature içindeki her prop için bir kolon oluşturur ancak biz belirtirsek(feature class üzerine veya dbcontext içinde [Owned] attribute'u eklenmeli) bu feature'u bir tablo olarak oluşturur fk ilişkisi ile ekler. EFCore'da böyle tiplere Owned Entity Type denir internetten araştır.
        public string BuyerId { get; set; } //userid

        //field
        private readonly List<OrderItem> _orderItems; //backing fields:encapsulation'ı arttırır, public yaparsak buraya data eklenebilir
        public IReadOnlyCollection<OrderItem> OrderItems => _orderItems; //sadece okuma işlemi gerçekleşsin, IReadOnlyCollection sadece okuma gerçekleştirir

        //default constructor gerekir migration için
        public Order()
        {

        }

        public Order(string buyerId,Address address)
        {
            _orderItems = new List<OrderItem>();
            CreatedDate = DateTime.Now;
            BuyerId = buyerId;
            Address = address;
        }

        //set etme işlemi
        public void AddOrderItem(string productId,string productName, decimal price, string pictureUrl)
        {
            var existProduct = _orderItems.Any(x=>x.ProductId == productId);

            if (!existProduct)
            {
                var newOrderItem = new OrderItem(productId,productName,pictureUrl,price);
                _orderItems.Add(newOrderItem);
            }

        }

        public decimal GetTotalPrice => _orderItems.Sum(x=> x.Price);


    }
}
