using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
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
            _cache = new LazyCache<Template>(loadTemplate);
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

        private Template loadTemplate(string name)
        {
            if (_partialsLocator == null)
            {
                return null;
            }

            var content = _partialsLocator.LoadContentByIdAsync(name).Result;

            using (var stream = new StringReader(content.ContentRaw))
            {
                var template = new Template();
                template.Load(stream);
                
                return template;
            }
        }

        private Template getTemplate(string key)
        {
            return _cache.GetFromCache(key);
        }
    }
}