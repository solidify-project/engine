using System.Threading.Tasks;

namespace SolidifyProject.Engine.Infrastructure.Services
{
    public interface IContentWriterService<T> where T : class
    {
        Task SaveContentAsync(string id, T content);
    }
}