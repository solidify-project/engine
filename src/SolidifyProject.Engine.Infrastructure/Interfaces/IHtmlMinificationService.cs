using System.Threading.Tasks;

namespace SolidifyProject.Engine.Infrastructure.Interfaces
{
    public interface IHtmlMinificationService
    {
        Task<string> CompressHtmlAsync(string html);
    }
}