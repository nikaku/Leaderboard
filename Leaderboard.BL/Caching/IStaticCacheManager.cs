using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Leaderboard.BL.Caching
{
    public interface IStaticCacheManager
    {
        void Set(string key, object data, int cacheTime);
        T Get<T>(CacheKey key);
    }
}
