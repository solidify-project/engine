using System.Collections.Generic;
using System.Threading.Tasks;

namespace SolidifyProject.Engine.Infrastructure.Services
{
    public interface IContentReaderService<T> where T : class
    {
        Task<IEnumerable<string>> LoadContentsIdsAsync();
        Task<T> LoadContentByIdAsync(string id);
    }
}