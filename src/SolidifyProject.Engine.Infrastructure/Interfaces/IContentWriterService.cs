using System.Threading.Tasks;

namespace SolidifyProject.Engine.Infrastructure.Interfaces
{
    public interface IContentWriterService<T> where T : class
    {
        Task SaveContentAsync(string id, T content);

        Task CleanFolderAsync(string path);
    }
}