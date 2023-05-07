using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservice.Services.Discount.Model
{
    [Dapper.Contrib.Extensions.Table("discount")] //postgre içinde tablolar genelde küçük harfle oluşturulur map'leme işlemi yaptık
    public class Discount
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int Rate { get; set; }
        public string Code { get; set; }
        public DateTime CreatedTime { get; set; }
    }
}
