using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace SolidifyProject.Engine.Utils.Cache
{
    public sealed class LazyCache<T> where T : class 
    {
        public delegate Task<T> LoadToCacheAsyncDelegate(string key);

        private readonly LoadToCacheAsyncDelegate _loadToCache;
        
        private readonly ConcurrentDictionary<string, Lazy<Task<T>>> _cache= new ConcurrentDictionary<string, Lazy<Task<T>>>();

        public LazyCache(LoadToCacheAsyncDelegate loadToCache)
        {
            _loadToCache = loadToCache;
        }

        public async Task<T> GetFromCacheAsync(string key)
        {
            if (_cache.TryGetValue(key, out var result))
            {
                return await result.Value.ConfigureAwait(false);
            }

            result = new Lazy<Task<T>>(() => _loadToCache(key));
            
            _cache.AddOrUpdate(key, result, (k, r) => result);

            return await GetFromCacheAsync(key).ConfigureAwait(false);
        }
    }
}