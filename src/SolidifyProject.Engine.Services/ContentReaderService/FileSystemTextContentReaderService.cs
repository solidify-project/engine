using System.IO;
using System.Threading.Tasks;
using SolidifyProject.Engine.Infrastructure.Models.Base;

namespace SolidifyProject.Engine.Services.ContentReaderService
{
    public class FileSystemTextContentReaderService<T> : _FileSystemContentReaderServiceBase<T> where T : TextContentModel, new()
    {
        public FileSystemTextContentReaderService(string root)
            : base(root)
        {
        }
        
        public override async Task<T> LoadContentByIdAsync(string id)
        {
            var path = Path.Combine(_root, id);
            
            if (!File.Exists(path))
            {
                return null;
            }

            using (var file = new FileStream(path, FileMode.Open))
            using (var reader = new StreamReader(file))
            {
                var model = new T();
                model.Id = id;
                model.ContentRaw = await reader.ReadToEndAsync();
                
                model.Parse();
            
                return model;
            }
        }
    }
}