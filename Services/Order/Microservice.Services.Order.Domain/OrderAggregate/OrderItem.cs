using Microservice.Services.Order.Domain.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microservice.Services.Order.Domain.OrderAggregate
{
    //Bir aggregateroot bir entity'i kullanıyorsa başka bir aggregate root kullanmamalı
    public class OrderItem:Entity
    {
        //Buraya orderid eklemiyoruz çünkü eğer eklersek bu entity tek başına db'ye ekleyebiliriz ama DDD'de aggregate root üzerinden eklenmesi lazım
        //burada orderid yoksa nasıl bireçok ilişki olacak? EFCore kendi bu id'yi oluşturacak ama bu entity'de karşılığı olmayacak
        //buna da shadow property denir..

        public string ProductId { get; private set; }
        public string ProductName { get; private set; }
        public string PictureUrl { get; private set; }
        public Decimal Price { get; private set; }

        public OrderItem()
        {

        }
        public OrderItem(string productId, string productName, string pictureUrl, decimal price)
        {
            ProductId = productId;
            ProductName = productName;
            PictureUrl = pictureUrl;
            Price = price;
        }

        //business kurallarımı uygulatabilmek için kontrolü elimize alıyoruz
        public void UpdateOrderItem(string productName, string pictureUrl, decimal price)
        {
            ProductName = productName;
            PictureUrl = pictureUrl;
            Price = price;
        }

    }
}
