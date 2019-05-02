using System;
using System.Collections.Concurrent;
using System.Threading;

namespace SolidifyProject.Engine.Utils.Cache
{
    public sealed class LazyCache<T> where T : class 
    {
        public delegate T LoadToCacheAsyncDelegate(string key);

        private readonly LoadToCacheAsyncDelegate _loadToCache;
        
        private readonly ConcurrentDictionary<string, Lazy<T>> _cache= new ConcurrentDictionary<string, Lazy<T>>();

        public LazyCache(LoadToCacheAsyncDelegate loadToCache)
        {
            _loadToCache = loadToCache;
        }

        public T GetFromCache(string key)
        {
            if (_cache.TryGetValue(key, out var result))
            {
                return result.Value;
            }

            result = new Lazy<T>(() => _loadToCache(key), LazyThreadSafetyMode.ExecutionAndPublication);
            
            _cache.AddOrUpdate(key, result, (k, r) => result);

            return GetFromCache(key);
        }
    }
}