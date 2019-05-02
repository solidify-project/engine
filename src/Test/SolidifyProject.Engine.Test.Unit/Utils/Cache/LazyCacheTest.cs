using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;
using SolidifyProject.Engine.Utils.Cache;

namespace SolidifyProject.Engine.Test.Unit.Utils.Cache
{
    [TestFixture]
    public class LazyCacheTest
    {
        private readonly Dictionary<string, string> _storage = new Dictionary<string, string>
        {
            {"key", "value"}
        };

        private LazyCacheAsync<string> _cacheAsync;

        [OneTimeSetUp]
        public void Init()
        {
            _cacheAsync = new LazyCacheAsync<string>((key) =>
            {
                return _storage.ContainsKey(key) ?
                    Task.FromResult(_storage[key]) :
                    Task.FromResult<string>(null);
            });
        }

        [Test]
        public async Task GetByNonCachedKey()
        {
            var value = await _cacheAsync.GetFromCacheAsync("key");
            
            Assert.AreEqual("value", value);
        }
        
        [Test]
        public async Task GetByCachedKey()
        {
            // cache is empty
            var value = await _cacheAsync.GetFromCacheAsync("key");
            
            // cache is not empty
            value = await _cacheAsync.GetFromCacheAsync("key");
            
            Assert.AreEqual("value", value);
        }
        
        [Test]
        public async Task GetByNonExistingKey()
        {
            var value = await _cacheAsync.GetFromCacheAsync("non existing key");
            
            Assert.IsNull(value);
        }
    }
}