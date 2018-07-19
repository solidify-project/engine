using System.IO;
using System.Threading.Tasks;
using SolidifyProject.Engine.Infrastructure.Interfaces;

namespace SolidifyProject.Engine.Services.ContentWriterService
{
    public abstract class _FileSystemContentWriterServiceBase<T> : IContentWriterService<T> where T : class 
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

        public abstract Task SaveContentAsync(string id, T content);
    }
}