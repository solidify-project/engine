using System.Threading.Tasks;

namespace SolidifyProject.Engine.Infrastructure.Services
{
    public interface IHtmlMinificationService
    {
        Task<string> CompressHtmlAsync(string html);
    }
}