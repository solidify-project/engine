using System.Threading.Tasks;

namespace SolidifyProject.Engine.Infrastructure.Services
{
    public interface IMarkupService
    {
        /// <summary>
        /// Renders HTML from markup.
        /// </summary>
        /// <param name="markup"></param>
        /// <returns>HTML string</returns>
        Task<string> RenderMarkupAsync(string markup);
    }
}