using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SolidifyProject.Engine.Infrastructure.Models.Base;
using SolidifyProject.Engine.Infrastructure.Services;

namespace SolidifyProject.Engine.Test._Fake.Infrastructure.Services
{
    internal class ContentReaderServiceFake<T, K> : IContentReaderService<T> where T : ContentModelBase<K>
    {
        private readonly List<T> _storage;
        public List<T> Storage => _storage;

        public ContentReaderServiceFake(List<T> storage = null)
        {
            _storage = storage ?? new List<T>();
        }
        
        public async Task<IEnumerable<string>> LoadContentsIdsAsync()
        {
            return _storage
                .Select(x => x.Id)
                .Where(x => !x.EndsWith("README.md", StringComparison.OrdinalIgnoreCase));
        }

        public async Task<T> LoadContentByIdAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentException(nameof(id));
            }

            return _storage.SingleOrDefault(x => x.Id.Equals(id, StringComparison.Ordinal));
        }
    }
}