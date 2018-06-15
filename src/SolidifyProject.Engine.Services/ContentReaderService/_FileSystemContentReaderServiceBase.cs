using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using SolidifyProject.Engine.Infrastructure.Interfaces;

namespace SolidifyProject.Engine.Services.ContentReaderService
{
    public abstract class _FileSystemContentReaderServiceBase<T> : IContentReaderService<T> where T : class
    {
        public readonly string[] IGNORED_FILES = {"README.md", ".DS_Store"};
        protected readonly string _root;

        protected _FileSystemContentReaderServiceBase(string root = null)
        {
            _root = string.IsNullOrEmpty(root) ? 
                Directory.GetCurrentDirectory() :
                root;
        }

        private async Task<IEnumerable<string>> LoadContentsIdsAsync(string root)
        {
            var files = Directory
                .GetFiles(root)
                .ToList();

            var folders = Directory.GetDirectories(root);

            foreach (var folder in folders)
            {
                files.AddRange(await LoadContentsIdsAsync(folder));
            }

            return files;
        }

        public async Task<IEnumerable<string>> LoadContentsIdsAsync(bool includeIgnored = false)
        {
            var query = (await LoadContentsIdsAsync(_root))
                .Select(x => x.Replace(_root, string.Empty).Trim('\\', '/'));

            if (!includeIgnored)
            {
                query = IGNORED_FILES.Aggregate(query, (current, ignored) => current.Where(x => !x.EndsWith(ignored, StringComparison.OrdinalIgnoreCase)));
            }

            return query;

        }

        public abstract Task<T> LoadContentByIdAsync(string id);
    }
}