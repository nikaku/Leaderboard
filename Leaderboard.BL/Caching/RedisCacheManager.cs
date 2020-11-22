using Leaderboard.BL.Configuration;
using Leaderboard.BL.Interfaces;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Leaderboard.BL.Caching
{
    public class RedisCacheManager : IStaticCacheManager
    {
        private IDatabase _db;
        private IServer _server;

        public RedisCacheManager(AppSettings appSettings)
        {
            _server = ConnectionMultiplexer.Connect($"{appSettings.RedisConnectionString},allowAdmin=true").GetServer("127.0.0.1:6379");
            _db = ConnectionMultiplexer.Connect(appSettings.RedisConnectionString).GetDatabase();
        }

        public void Set(CacheKey key, object data)
        {
            if (data == null)
                return;

            var expiresIn = TimeSpan.FromMinutes(key.CacheTime);
            var serializedItem = JsonConvert.SerializeObject(data);
            _db.StringSetAsync(key.Key, serializedItem, expiresIn);
        }

        public T Get<T>(CacheKey key)
        {
            var serializedItem = _db.StringGet(key.Key);
            if (!serializedItem.HasValue)
                return default;

            var item = JsonConvert.DeserializeObject<T>(serializedItem);
            if (item == null)
                return default;

            return item;
        }

        public void ClearCache()
        {
            _server.FlushDatabase();
        }

        public void ClearCache(string pattern, IEnumerable<string> items)
        {
            List<RedisKey> keys = new List<RedisKey>();
            foreach (var item in items)
            {
                keys.Add(new RedisKey($"{pattern}-{item}"));
            }
            _db.KeyDeleteAsync(keys.ToArray());
        }

        public void ClearCache(string pattern)
        {
            RedisKey[] keys = _server.Keys(pattern: pattern + "*").ToArray();
            _db.KeyDeleteAsync(keys);
        }
    }
}
