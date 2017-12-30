using System.Threading.Tasks;
using Markdig;
using SolidifyProject.Engine.Infrastructure.Services;

namespace SolidifyProject.Engine.Services.MarkupService
{
    public class MarkdownMarkupService : IMarkupService
    {
        private static readonly MarkdownPipeline defaultPipeline = new MarkdownPipelineBuilder()
            .UsePipeTables()
            .Build();
        
        public async Task<string> RenderMarkupAsync(string markup)
        {
            return Markdown.ToHtml(markup, defaultPipeline);
        }
    }
}