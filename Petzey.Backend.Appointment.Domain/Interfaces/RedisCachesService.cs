using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Newtonsoft.Json;
using System.Text.Json;
using Petzey.Backend.Appointment.Domain.Entities;





namespace Petzey.Backend.Appointment.Domain.Interfaces
{
    public class RedisCachesService: ICacheService
    {
        private readonly IDatabase _cache;
        public RedisCachesService()
        {
            var options = ConfigurationOptions.Parse("localhost:6379,password=1234,abortConnect=false");
            var connectionMultiplexer = ConnectionMultiplexer.Connect("localhost:6379,password=1234,abortConnect=false");
            _cache = connectionMultiplexer.GetDatabase();
        }
        public T Get<T>(string key)
        {
            var value = _cache.StringGet(key);
            if (value.HasValue)
            {
              /* return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(value);*/
               /* return System.Text.Json.JsonSerializer.Deserialize<T>(value);*/
                return System.Text.Json.JsonSerializer.Deserialize<T>(value);
                /* return System.Text.Json.JsonSerializer.Deserialize<T>(value, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });*/
            }
            return default(T);
        }
        public void Set<T>(string key, T value, TimeSpan expiry)
        {
            var serializedValue = System.Text.Json.JsonSerializer.Serialize(value);
            _cache.StringSet(key, serializedValue, expiry);
        }

        
    }
}
