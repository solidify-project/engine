using System.IO;
using System.Threading.Tasks;
using SolidifyProject.Engine.Infrastructure.Models.Base;

namespace SolidifyProject.Engine.Services.ContentWriterService
{
    public class FileSystemTextContentWriterService<T> : _FileSystemContentWriterServiceBase<T> where T : TextContentModel
    {
        public FileSystemTextContentWriterService(string root)
            : base(root)
        {
        }
        
        public override async Task SaveContentAsync(string id, T content)
        {
            var path = Path.Combine(_root, id);
            EnsureDirectoryExists(path);
            
            using (var file = new FileStream(path, FileMode.Create))
            using (var writer = new StreamWriter(file))
            {
                await writer.WriteAsync(content.ContentRaw).ConfigureAwait(false);
            }
        }
    }
}