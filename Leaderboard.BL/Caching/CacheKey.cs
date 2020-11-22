using System;
using System.Collections.Generic;
using System.Text;

namespace Leaderboard.BL.Caching
{
    public class CacheKey
    {
        public CacheKey(CacheKey key)
        {
            Key = key.Key;
        }

        public CacheKey(string key)
        {
            Key = key;
        }

        public CacheKey(CacheKey key, string suffix)
        {
            Key = $"{key.Key}-{suffix}";
        }

        public string Key { get; set; }
        public int CacheTime { get; set; } = 20;
    }
}
