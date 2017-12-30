using System.IO;
using System.Threading.Tasks;
using SolidifyProject.Engine.Infrastructure.Services;

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

        public abstract Task SaveContentAsync(string id, T content);
    }
}