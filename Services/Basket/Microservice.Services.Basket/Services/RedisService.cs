using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservice.Services.Basket.Services
{
    public class RedisService
    {
        private readonly string _host;
        private readonly int _port;

        //bağlantı sınıfı
        private ConnectionMultiplexer _connectionMultiplexer;

        public RedisService(string host, int port)
        {
            _host = host;
            _port = port;
        }

        //bağlantı
        public void Connect() => _connectionMultiplexer = ConnectionMultiplexer.Connect($"{_host}:{_port}");

        //veritabanı, birden fazla veritabanı geliyor biz 1 nolu olanı kullanacağız
        public IDatabase GetDb(int db = 1)  => _connectionMultiplexer.GetDatabase(db);


    }
}
