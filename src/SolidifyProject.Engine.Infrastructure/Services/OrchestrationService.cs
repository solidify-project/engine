using System;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using SolidifyProject.Engine.Infrastructure.Interfaces;
using SolidifyProject.Engine.Infrastructure.Models;
using SolidifyProject.Engine.Infrastructure.Models.Base;

namespace SolidifyProject.Engine.Infrastructure.Services
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

        public IDataService DataService { get; set; }

        public IMarkupService MarkupService { get; set; }

        public IHtmlMinificationService HtmlMinificationService { get; set; }
        
        public async Task RenderProjectAsync()
        {
            var tasksGroupContent = Task.Run(async () =>
            {
                var dataModel = await DataService.GetDataModelAsync();

                var pages = await PageModelReaderService.LoadContentsIdsAsync();
                var pageTasks = pages.Select(pageId => ProcessPageByIdAsync(pageId, dataModel)).ToList();
            
                await Task.WhenAll(pageTasks);
            });
            
            var tasksGroupAssets = Task.Run(async () =>
            {
                var assets = await AssetsReaderService.LoadContentsIdsAsync();

                var tasks = assets.Select(CopyAssetByIdAsync).ToList();

                await Task.WhenAll(tasks);
            });

            await Task.WhenAll(tasksGroupContent, tasksGroupAssets);
        }

        private async Task ProcessPageByIdAsync(string pageId, ExpandoObject dataModel)
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

        private async Task CopyAssetByIdAsync(string id)
        {
            await LoggerService.WriteLogMessage($"{DateTime.Now.ToLongTimeString()}: Started copying of asset \"{id}\"");
            
            var content = await AssetsReaderService.LoadContentByIdAsync(id);

            await AssetsWriterService.SaveContentAsync(id, content);
            
            await LoggerService.WriteLogMessage($"{DateTime.Now.ToLongTimeString()}: Finished copying of asset \"{id}\"");
        }
    }
}