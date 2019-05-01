using System.Threading.Tasks;

namespace SolidifyProject.Engine.Infrastructure.Interfaces
{
    public interface IRemoteContentReaderService<in T> where T : class
    {
        Task<string> LoadContentAsync(T parameters);
    }
}