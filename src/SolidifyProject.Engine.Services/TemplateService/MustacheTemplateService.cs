using System;
using System.Dynamic;
using System.IO;
using System.Threading.Tasks;
using Nustache.Core;
using SolidifyProject.Engine.Infrastructure.Interfaces;
using SolidifyProject.Engine.Infrastructure.Models;
using SolidifyProject.Engine.Infrastructure.Models.Base;
using SolidifyProject.Engine.Utils.Cache;

namespace SolidifyProject.Engine.Services.TemplateService
{
    public sealed class MustacheTemplateService : ITemplateService
    {
        private readonly Encoders.HtmlEncoder _noHtmlEncoding = delegate(string text) { return text; };
        private readonly IContentReaderService<TextContentModel> _partialsLocator;

        private readonly LazyCache<Template> _cache;
        
        public MustacheTemplateService(IContentReaderService<TextContentModel> partialsLocator = null)
        {
            _partialsLocator = partialsLocator;
            _cache = new LazyCache<Template>(loadTemplateAsync);
        }
        
        public Task<string> RenderTemplateAsync(string template, PageModel pageModel, ExpandoObject dataModel)
        {
            if (pageModel == null)
            {
                throw new ArgumentNullException(nameof(pageModel));
            }


            var model = new { Page = pageModel, Data = dataModel };
            
            var result = Render.StringToString(template, model, getTemplate, new RenderContextBehaviour
            {
                HtmlEncoder = _noHtmlEncoding
            });

            return Task.FromResult(result);
        }

        private async Task<Template> loadTemplateAsync(string name)
        {
            if (_partialsLocator == null)
            {
                return null;
            }

            var content = await _partialsLocator.LoadContentByIdAsync(name);

            if (content == null)
            {
                throw new FileNotFoundException(name);
            }

            var template = new Template();
            template.Load(new StringReader(content.ContentRaw));

            return template;
        }

        private Template getTemplate(string key)
        {
            var task = _cache.GetFromCacheAsync(key);
            task.Wait();

            return task.Result;
        }

    }
}