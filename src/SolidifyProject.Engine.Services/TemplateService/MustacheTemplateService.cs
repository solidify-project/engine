using System;
using System.Dynamic;
using System.IO;
using System.Threading.Tasks;
using SolidifyProject.Engine.Infrastructure.Interfaces;
using SolidifyProject.Engine.Infrastructure.Models;
using SolidifyProject.Engine.Infrastructure.Models.Base;
using Stubble.Core.Contexts;
using Stubble.Core.Exceptions;
using Stubble.Core.Interfaces;
using Stubble.Core.Renderers.StringRenderer;
using Stubble.Core.Settings;

namespace SolidifyProject.Engine.Services.TemplateService
{
    public sealed class MustacheTemplateService : ITemplateService
    {
        private readonly IContentReaderService<TextContentModel> _partialsLocator;

        internal RendererSettings RendererSettings = new RendererSettingsBuilder().BuildSettings();

        
        public MustacheTemplateService(IContentReaderService<TextContentModel> partialsLocator = null)
        {
            _partialsLocator = partialsLocator;
        }
        
        public async Task<string> RenderTemplateAsync(string template, PageModel pageModel, ExpandoObject dataModel)
        {
            if (pageModel == null)
            {
                throw new ArgumentNullException(nameof(pageModel));
            }

            var model = new { Page = pageModel, Data = dataModel };
            
            var loader = new StubbleLoader(_partialsLocator);
            
            return await RenderAsync(template, model, loader, new RenderSettings
            {
                SkipHtmlEncoding = true,
                ThrowOnDataMiss = true
            });
        }

        private async Task<string> RenderAsync(string template, object view, IStubbleLoader partialsLoader, RenderSettings settings)
        {
            var loadedTemplate = await RendererSettings.TemplateLoader.LoadAsync(template);

            if (loadedTemplate == null)
            {
                throw new UnknownTemplateException("No template was found with the name '" + template + "'");
            }

            var document = RendererSettings.Parser.Parse(loadedTemplate, RendererSettings.DefaultTags, pipeline: RendererSettings.ParserPipeline);

            using (var textwriter = new StringWriter())
            {
                var renderer = new StringRender(textwriter, RendererSettings.RendererPipeline, RendererSettings.MaxRecursionDepth);

                await renderer.RenderAsync(document, new Context(view, RendererSettings, partialsLoader, settings ?? RendererSettings.RenderSettings));

                renderer.Writer.Flush();
                return ((StringWriter)renderer.Writer).ToString();
            }
        }
    }
}