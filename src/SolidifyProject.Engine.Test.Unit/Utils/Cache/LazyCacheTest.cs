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

        private LazyCache<string> Cache;

        [OneTimeSetUp]
        public void Init()
        {
            Cache = new LazyCache<string>((key) =>
            {
                return _storage.ContainsKey(key) ?
                    Task.FromResult(_storage[key]) :
                    Task.FromResult<string>(null);
            });
        }

        [Test]
        public async Task GetByNonCachedKey()
        {
            var value = await Cache.GetFromCacheAsync("key");
            
            Assert.AreEqual("value", value);
        }
        
        [Test]
        public async Task GetByCachedKey()
        {
            // cache is empty
            var value = await Cache.GetFromCacheAsync("key");
            
            // cache is not empty
            value = await Cache.GetFromCacheAsync("key");
            
            Assert.AreEqual("value", value);
        }
        
        [Test]
        public async Task GetByNonExistingKey()
        {
            var value = await Cache.GetFromCacheAsync("non existing key");
            
            Assert.IsNull(value);
        }
    }
}