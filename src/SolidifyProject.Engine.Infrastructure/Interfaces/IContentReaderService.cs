using System.Collections.Generic;
using System.Threading.Tasks;

namespace SolidifyProject.Engine.Infrastructure.Interfaces
{
    public interface IContentReaderService<T> where T : class
    {
        Task<IEnumerable<string>> LoadContentsIdsAsync(bool includeIgnored = false);
        Task<T> LoadContentByIdAsync(string id);
    }
}