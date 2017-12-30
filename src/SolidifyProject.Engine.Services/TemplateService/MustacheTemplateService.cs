using System;
using System.Dynamic;
using System.IO;
using System.Threading.Tasks;
using Nustache.Core;
using SolidifyProject.Engine.Infrastructure.Models;
using SolidifyProject.Engine.Infrastructure.Models.Base;
using SolidifyProject.Engine.Infrastructure.Services;

namespace SolidifyProject.Engine.Services.TemplateService
{
    public sealed class MustacheTemplateService : ITemplateService
    {
        private readonly Encoders.HtmlEncoder _noHtmlEncoding = delegate(string text) { return text; };

        private readonly IContentReaderService<TextContentModel> _partialsLocator;
        
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
            
            return Render.StringToString(template, model, getPartialTemplate, new RenderContextBehaviour
            {
                HtmlEncoder = _noHtmlEncoding
            });
        }

        private Template getPartialTemplate(string name)
        {
            if (_partialsLocator == null)
            {
                return null;
            }

            var content = _partialsLocator.LoadContentByIdAsync(name).Result.ContentRaw;

            var template = new Template();
            template.Load(new StringReader(content));

            return template;
        }
    }
}