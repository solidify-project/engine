using System.Dynamic;
using System.Threading.Tasks;
using SolidifyProject.Engine.Infrastructure.Models;

namespace SolidifyProject.Engine.Infrastructure.Services
{
    public interface ITemplateService
    {
        /// <summary>
        /// Renders HTML using template and data model.
        /// </summary>
        /// <param name="template"></param>
        /// <param name="pageModel"></param>
        /// <param name="dataModel"></param>
        /// <returns>HTML string</returns>
        Task<string> RenderTemplateAsync(string template, PageModel pageModel, ExpandoObject dataModel);
    }
}