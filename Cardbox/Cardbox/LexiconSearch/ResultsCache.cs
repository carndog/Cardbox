using System.Collections.Generic;
using System.Runtime.Caching;

namespace Cardbox.LexiconSearch
{
    public class ResultsCache : IResultsCache<string, IList<string>>
    {
        private readonly CacheItemPolicy _cacheItemPolicy = new CacheItemPolicy();
        private readonly MemoryCache _cache = MemoryCache.Default;

        public IList<string> Get(string key)
        {
            return _cache[key] as IList<string>;
        }

        public void Add(string key, IList<string> results)
        {
            if (!_cache.Contains(key))
            {
                _cache.Add(key, results, _cacheItemPolicy);
            }
        }
    }
}