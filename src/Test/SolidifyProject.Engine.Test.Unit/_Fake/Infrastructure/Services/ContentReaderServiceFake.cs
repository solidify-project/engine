using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SolidifyProject.Engine.Infrastructure.Interfaces;
using SolidifyProject.Engine.Infrastructure.Models.Base;

namespace SolidifyProject.Engine.Test.Unit._Fake.Infrastructure.Services
{
    internal class ContentReaderServiceFake<T, K> : IContentReaderService<T> where T : ContentModelBase<K>
    {
        private readonly List<T> _storage;
        public List<T> Storage => _storage;

        public ContentReaderServiceFake(List<T> storage = null)
        {
            _storage = storage ?? new List<T>();
        }
        
        public Task<IEnumerable<string>> LoadContentsIdsAsync(bool includeIgnored = false)
        {
            var result = _storage
                .Select(x => x.Id);

            if (!includeIgnored)
            {
                result = result.Where(x => !x.EndsWith("README.md", StringComparison.OrdinalIgnoreCase));
            }

            return Task.FromResult(result);
        }

        public Task<T> LoadContentByIdAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentException(nameof(id));
            }

            var result = _storage
                .SingleOrDefault(x => x.Id.Equals(id, StringComparison.Ordinal));

            return Task.FromResult(result);
        }
    }
}