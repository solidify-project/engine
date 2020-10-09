using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace SolidifyProject.Engine.Utils.Cache
{
    public sealed class LazyCache<T> where T : class 
    {
        public delegate Task<T> LoadToCacheAsyncDelegate(string key);

        private readonly LoadToCacheAsyncDelegate _loadToCache;
        
        private readonly ConcurrentDictionary<string, Lazy<Task<T>>> _cache= new ConcurrentDictionary<string, Lazy<Task<T>>>();
        private static readonly ConcurrentDictionary<string, Mutex> _mutexes = new ConcurrentDictionary<string, Mutex>();
        
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

            result = new Lazy<Task<T>>(() =>
            {
                using (var mutex = _getMutexByKey(key))
                {
                    mutex.WaitOne();
                    var task = _loadToCache(key);
                    mutex.ReleaseMutex();
                    return task;
                }
            });
            
            _cache.AddOrUpdate(key, result, (k, r) => result);

            return await GetFromCacheAsync(key).ConfigureAwait(false);
        }

        private static Mutex _getMutexByKey(string key)
        {
            Mutex result;
            if (_mutexes.TryGetValue(key, out result))
            {
                return result;
            }

            result = new Mutex();
            if (_mutexes.TryAdd(key, result))
            {
                return result;
            }

            return _getMutexByKey(key);
        }
    }
}