using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using SolidifyProject.Engine.Infrastructure.Models;
using SolidifyProject.Engine.Infrastructure.Models.Base;
using SolidifyProject.Engine.Infrastructure.Services;

namespace SolidifyProject.Engine.Services
{
    public sealed class OrchestrationService
    {
        public ILoggerService LoggerService { get; set; }

        public IContentReaderService<BinaryContentModel> AssetsReaderService { get; set; }
        public IContentWriterService<BinaryContentModel> AssetsWriterService { get; set; }
        
        public IContentReaderService<PageModel> PageModelReaderService { get; set; }
        public IContentWriterService<TextContentModel> PageModelWriterService { get; set; }
        
        public IContentReaderService<TemplateModel> TemplateReaderService { get; set; }
        public ITemplateService TemplateService { get; set; }

        public IContentReaderService<CustomDataModel> DataReaderService { get; set; }

        public IMarkupService MarkupService { get; set; }

        public IHtmlMinificationService HtmlMinificationService { get; set; }
        
        public async Task RenderProject()
        {
            var tasksGroupContent = Task.Run(async () =>
            {
                var data = await DataReaderService.LoadContentsIdsAsync();
                var dataTasks = data.Select(GetDataById).ToList();
            
                await Task.WhenAll(dataTasks);
            
                var dataModel = new ExpandoObject();
                foreach (var task in dataTasks)
                {
                    var sections = task.Result.Key.Split(new [] {'.'}, StringSplitOptions.RemoveEmptyEntries);
                    if (sections.Length > 1)
                    {
                        var obj = new ExpandoObject();
                        var lastSection = sections.Last();
                        
                        ((ICollection<KeyValuePair<string, object>>) obj)
                            .Add(new KeyValuePair<string, object>(lastSection, task.Result.Value));

                        foreach (var section in sections.Reverse().Skip(2))
                        {
                            var tmp = new ExpandoObject();
                            
                            ((ICollection<KeyValuePair<string, object>>) tmp)
                                .Add(new KeyValuePair<string, object>(section, obj));

                            obj = tmp;
                        }
                        
                        var firstSection = sections.First();
                        
                        ((ICollection<KeyValuePair<string, object>>) dataModel)
                            .Add(new KeyValuePair<string, object>(firstSection, obj));
                    }
                    else
                    {
                        ((ICollection<KeyValuePair<string, object>>) dataModel)
                            .Add(new KeyValuePair<string, object>(task.Result.Key, task.Result.Value));
                    }
                }

                var pages = await PageModelReaderService.LoadContentsIdsAsync();
                var pageTasks = pages.Select(pageId => ProcessPageById(pageId, dataModel)).ToList();
            
                await Task.WhenAll(pageTasks);
            });
            
            var tasksGroupAssets = Task.Run(async () =>
            {
                var assets = await AssetsReaderService.LoadContentsIdsAsync();

                var tasks = assets.Select(CopyAssetById).ToList();

                await Task.WhenAll(tasks);
            });

            await Task.WhenAll(tasksGroupContent, tasksGroupAssets);
        }

        private async Task<KeyValuePair<string, object>> GetDataById(string dataId)
        {
            await LoggerService.WriteLogMessage($"{DateTime.Now.ToLongTimeString()}: Started retrieval of data \"{dataId}\"");
            
            var customData = await DataReaderService.LoadContentByIdAsync(dataId);

            try
            {
                return new KeyValuePair<string, object>(customData.SanitezedId, customData.CustomData);
            }
            finally
            {
                await LoggerService.WriteLogMessage($"{DateTime.Now.ToLongTimeString()}: Finished data retrieval \"{dataId}\"");
            }
        }

        private async Task ProcessPageById(string pageId, ExpandoObject dataModel)
        {
            await LoggerService.WriteLogMessage($"{DateTime.Now.ToLongTimeString()}: Started precessing of page \"{pageId}\"");
            
            var page = await PageModelReaderService.LoadContentByIdAsync(pageId);
                
            var html = await TemplateService.RenderTemplateAsync(page.Content, page, dataModel);
            html = await MarkupService.RenderMarkupAsync(html);
            page.Content = html;
                
            var template = await TemplateReaderService.LoadContentByIdAsync(page.TemplateId);
            html = await TemplateService.RenderTemplateAsync(template.Template, page, dataModel);

            html = await HtmlMinificationService.CompressHtmlAsync(html);

            var result = new TextContentModel();
            result.Id = page.Id;
            result.ContentRaw = html;
                
            await PageModelWriterService.SaveContentAsync(page.Url, result);
            
            await LoggerService.WriteLogMessage($"{DateTime.Now.ToLongTimeString()}: Finished page precessing \"{pageId}\"");
        }

        private async Task CopyAssetById(string id)
        {
            await LoggerService.WriteLogMessage($"{DateTime.Now.ToLongTimeString()}: Started copying of asset \"{id}\"");
            
            var content = await AssetsReaderService.LoadContentByIdAsync(id);

            await AssetsWriterService.SaveContentAsync(id, content);
            
            await LoggerService.WriteLogMessage($"{DateTime.Now.ToLongTimeString()}: Finished copying of asset \"{id}\"");
        }
    }
}