using System;
using System.Threading.Tasks;
using SolidifyProject.Engine.Infrastructure.Services;

namespace SolidifyProject.Engine.Services.HtmlMinificationService
{
    public class NoMinificationService : IHtmlMinificationService
    {
        public async Task<string> CompressHtmlAsync(string html)
        {
            if (html == null)
            {
                throw new ArgumentNullException(nameof(html));
            }
            
            return html;
        }
    }
}