using Leaderboard.BL.Configuration;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Threading.Tasks;

namespace Leaderboard.BL.Caching
{
    public class RedisCacheManager : IStaticCacheManager
    {
        private IDatabase _db;

        public RedisCacheManager(AppSettings appSettings)
        {
            _db = ConnectionMultiplexer.Connect(appSettings.RedisConnectionString).GetDatabase();
        }

        public void Set(string key, object data, int cacheTime)
        {
            if (data == null)
                return;

            //set cache time
            var expiresIn = TimeSpan.FromMinutes(cacheTime);

            //serialize item
            var serializedItem = JsonConvert.SerializeObject(data);

            //and set it to cache
            _db.StringSetAsync(key, serializedItem, expiresIn);
        }

        public virtual T Get<T>(CacheKey key)
        {
            //get serialized item from cache
            var serializedItem = _db.StringGet(key.Key);
            if (!serializedItem.HasValue)
                return default;

            //deserialize item
            var item = JsonConvert.DeserializeObject<T>(serializedItem);
            if (item == null)
                return default;

            return item;

        }

        public virtual T Get<T>(CacheKey key, Func<T> acquire)
        {
            var result = acquire();
            return result;
        }
    }
}
