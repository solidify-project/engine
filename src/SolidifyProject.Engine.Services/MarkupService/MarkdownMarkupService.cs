using System.Threading.Tasks;
using Markdig;
using SolidifyProject.Engine.Infrastructure.Interfaces;

namespace SolidifyProject.Engine.Services.MarkupService
{
    public class MarkdownMarkupService : IMarkupService
    {
        private static readonly MarkdownPipeline defaultPipeline = new MarkdownPipelineBuilder()
            .UsePipeTables()
            .Build();
        
        public Task<string> RenderMarkupAsync(string markup)
        {
            var result = Markdown.ToHtml(markup, defaultPipeline);

            return Task.FromResult(result);
        }
    }
}