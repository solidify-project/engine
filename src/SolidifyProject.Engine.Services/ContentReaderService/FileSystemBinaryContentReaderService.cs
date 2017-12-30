using System.IO;
using System.Threading.Tasks;
using SolidifyProject.Engine.Infrastructure.Models.Base;

namespace SolidifyProject.Engine.Services.ContentReaderService
{
    public class FileSystemBinaryContentReaderService<T> : _FileSystemContentReaderServiceBase<T> where T : BinaryContentModel, new()
    {
        public FileSystemBinaryContentReaderService(string root)
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
            {
                var model = new T();
                model.Id = id;
                model.ContentRaw = new byte[file.Length];
                
                await file.ReadAsync(model.ContentRaw, 0, (int)file.Length);
                model.Parse();
                
                return model;
            }
        }
    }
}