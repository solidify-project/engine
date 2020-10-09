using System;
using System.IO;
using System.Threading.Tasks;
using SolidifyProject.Engine.Infrastructure.Interfaces;

namespace SolidifyProject.Engine.Services.ContentWriterService
{
    public abstract class _FileSystemContentWriterServiceBase<T> : _FileSystemContentServiceBase, IContentWriterService<T> where T : class 
    {
        protected readonly string _root;

        protected _FileSystemContentWriterServiceBase(string root = null)
        {
            _root = string.IsNullOrEmpty(root) ? 
                Directory.GetCurrentDirectory() :
                root;
        }

        public void EnsureDirectoryExists(string path)
        {
            var directory = Path.HasExtension(path)?
                Path.GetDirectoryName(path) :
                path;
            
            Directory.CreateDirectory(directory);
        }

        public Task CleanFolderAsync(string path)
        {
            DirectoryInfo di = new DirectoryInfo(Path.Combine(_root, path));
            if (di.Exists)
            {
                foreach (var file in di.GetFiles())
                {
                    file.Delete();
                }

                foreach (var directory in di.GetDirectories())
                {
                    directory.Delete(true);
                }
            }

            return Task.CompletedTask;
        }

        public async Task SaveContentAsync(string id, T content)
        {
            LockFileByKey(id);

            try
            {
                await SaveContentAsyncInternal(id, content);
            }
            finally
            {
                ReleaseFileByKey(id);
            }
        }

        protected abstract Task SaveContentAsyncInternal(string id, T content);
    }
}