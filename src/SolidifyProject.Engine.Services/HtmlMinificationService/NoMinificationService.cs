using System;
using System.Threading.Tasks;
using SolidifyProject.Engine.Infrastructure.Interfaces;

namespace SolidifyProject.Engine.Services.HtmlMinificationService
{
    public class NoMinificationService : IHtmlMinificationService
    {
        public Task<string> CompressHtmlAsync(string html)
        {
            if (html == null)
            {
                throw new ArgumentNullException(nameof(html));
            }
            
            return Task.FromResult(html);
        }
    }
}