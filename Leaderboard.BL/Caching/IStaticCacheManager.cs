using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Leaderboard.BL.Caching
{
    public interface IStaticCacheManager
    {
        void Set(CacheKey key, object data);
        T Get<T>(CacheKey key);
        void ClearCache();
        void ClearCache(string pattern, IEnumerable<string> items);
        void ClearCache(string patern);
    }
}
