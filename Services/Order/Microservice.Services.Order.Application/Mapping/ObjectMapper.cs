using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microservice.Services.Order.Application.Mapping
{
    public static class ObjectMapper
    {
        //bu sınıf ne zaman kullanılır o zaman initialize olsun dersek Lazy sınıfı kullanırız
        private static readonly Lazy<IMapper> lazy = new Lazy<IMapper>(() =>
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CustomMapping>();
            });
            return config.CreateMapper();
        }
        );

        //ne zaman çağırırsak o zaman çalışacak
        public static IMapper Mapper => lazy.Value;

    }
}
