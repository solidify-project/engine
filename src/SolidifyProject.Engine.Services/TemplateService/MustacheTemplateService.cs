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
            
            matchDataToPageModel(pageModel.Model, dataModel);

            var model = new { Page = pageModel, Data = dataModel, Model = pageModel.Model };
            
            var result = Render.StringToString(template, model, getTemplate, new RenderContextBehaviour
            {
                HtmlEncoder = _noHtmlEncoding
            });

            return Task.FromResult(result);
        }

        private Task<Template> loadTemplate(string name)
        {
            if (_partialsLocator == null)
            {
                return null;
            }

            var content = _partialsLocator.LoadContentByIdAsync(name).Result.ContentRaw;

            var template = new Template();
            template.Load(new StringReader(content));

            return Task.FromResult(template);
        }

        private Template getTemplate(string key)
        {
            var task = _cache.GetFromCacheAsync(key);
            task.Wait();

            return task.Result;
        }

        private void matchDataToPageModel(ExpandoObject model, ExpandoObject data)
        {
            IDictionary<string, object> modelDict = model;
            foreach (var keyValuePair in model)
            {
                if (keyValuePair.Value is ExpandoObject o)
                {
                    matchDataToPageModel(o, data);
                }
                else
                {
                    modelDict[keyValuePair.Key] = getValueFromDataObject(keyValuePair.Value as string, data);
                }
            }
        }

        private object getValueFromDataObject(string path, ExpandoObject data)
        {
            var attributeNames = path.Split('.');
            if (attributeNames.Length == 0)
            {
                return null;
            }

            if (attributeNames.First() == "Data")
            {
                if (attributeNames.Length == 1)
                {
                    return null;
                }

                attributeNames = attributeNames.Skip(1).ToArray();
            }

            object value = data;
            foreach (var attribute in attributeNames)
            {
                if (value is ExpandoObject o)
                {
                    value = ((dynamic) o)[attribute];
                }
                else
                {
                    return null;
                }
            }

            return value;
        }
    }
}