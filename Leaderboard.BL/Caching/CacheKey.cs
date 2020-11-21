using System;
using System.Collections.Generic;
using System.Text;

namespace Leaderboard.BL.Caching
{
    public class CacheKey
    {
        public CacheKey(string key)
        {
            Key = key;
        }

        public string Key { get; set; }
        public int CacheTime { get; set; }
    }
}
