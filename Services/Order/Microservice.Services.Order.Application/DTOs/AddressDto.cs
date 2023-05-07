using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microservice.Services.Order.Application.DTOs
{
    //Applcation katmanı UI ile haberleşen katmandır. Dto oluşturmamızın nedeni ise gerçek entity'deki bazı alanları UI'a göstermek istemeyebiliriz.
    public class AddressDto
    {
        public string Province { get;  set; } //set etme kapalı
        public string District { get;  set; }
        public string Street { get;  set; }
        public string ZipCode { get;  set; }
        public string Line { get;  set; }
    }
}
