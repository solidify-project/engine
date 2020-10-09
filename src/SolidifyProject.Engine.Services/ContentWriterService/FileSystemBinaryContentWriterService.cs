using System.IO;
using System.Threading.Tasks;
using SolidifyProject.Engine.Infrastructure.Models.Base;

namespace SolidifyProject.Engine.Services.ContentWriterService
{
    public class FileSystemBinaryContentWriterService<T> : _FileSystemContentWriterServiceBase<T> where T : BinaryContentModel
    {
        public FileSystemBinaryContentWriterService(string root)
            : base(root)
        {
        }
        
        protected override async Task SaveContentAsyncInternal(string id, T content)
        {
            var path = Path.Combine(_root, id);
            EnsureDirectoryExists(path);
            
            using (var file = new FileStream(path, FileMode.Create))
            {
                await file.WriteAsync(content.ContentRaw, 0, content.ContentRaw.Length).ConfigureAwait(false);
            }
        }
    }
}