using System.Collections.Concurrent;
using System.Threading;

namespace SolidifyProject.Engine.Services
{
    public abstract class _FileSystemContentServiceBase
    {
        private static readonly ConcurrentDictionary<string, SemaphoreSlim> _semaphores = new ConcurrentDictionary<string, SemaphoreSlim>();

        protected _FileSystemContentServiceBase(string root = null)
        {
        }

        protected void LockFileByKey(string key)
        {
            var fileLock = _getLockByKey(key);
            fileLock.Wait();
        }

        protected void ReleaseFileByKey(string key)
        {
            var fileLock = _getLockByKey(key);
            fileLock.Release(); 
        }

        private static SemaphoreSlim _getLockByKey(string key)
        {
            SemaphoreSlim result;
            if (_semaphores.TryGetValue(key, out result))
            {
                return result;
            }

            result = new SemaphoreSlim(1, 1);
            if (_semaphores.TryAdd(key, result))
            {
                return result;
            }

            return _getLockByKey(key);
        }
    }
}