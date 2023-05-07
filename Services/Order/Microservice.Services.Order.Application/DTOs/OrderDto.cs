using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microservice.Services.Order.Application.DTOs
{
    public class OrderDto
    {
        public int Id { get; set; } //entity'den gelen id
        public DateTime CreatedDate { get; set; }
        public AddressDto Address { get; set; } //Biz eğer EFCore'a ayrıyeten belirtmezsek EFCore bu feature içindeki her prop için bir kolon oluşturur ancak biz belirtirsek(feature class üzerine veya dbcontext içinde [Owned] attribute'u eklenmeli) bu feature'u bir tablo olarak oluşturur fk ilişkisi ile ekler. EFCore'da böyle tiplere Owned Entity Type denir internetten araştır.
        public string BuyerId { get; set; } //userid
        public List<OrderItemDto> OrderItems; //sadece okuma işlemi gerçekleşsin, IReadOnlyCollection sadece okuma gerçekleştirir

    }
}
