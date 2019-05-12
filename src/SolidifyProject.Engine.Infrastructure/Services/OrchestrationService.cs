using System;
using System.Collections.Generic;
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
            await CleanOutputAsync()
                .ConfigureAwait(false);
            
            var tasksGroupContent = Task.Run(async () =>
            {
                var dataModel = await DataService.GetDataModelAsync()
                    .ConfigureAwait(false);

                var pages = await PageModelReaderService.LoadContentsIdsAsync()
                    .ConfigureAwait(false);
                
                var pageTasks = pages.Select(pageId => ProcessPageByIdAsync(pageId, dataModel));
            
                await Task.WhenAll(pageTasks)
                    .ConfigureAwait(false);
                
//                var dataModelTask = DataService.GetDataModelAsync();
//                var pagesLoadTask = LoadPagesAsync();
//
//                await Task.WhenAll(dataModelTask, pagesLoadTask)
//                    .ConfigureAwait(false);
//
//                var pages = pagesLoadTask.Result;
//                
//                var populateFeedsTask = pages
//                    .Select(PopulateFeedsAsync);
//
//                await Task.WhenAll(populateFeedsTask)
//                    .ConfigureAwait(false);
//                    
//                var renderPageTask = pages
//                    .Select(page => RenderPageAsync(page, dataModelTask.Result));
//
//                await Task.WhenAll(renderPageTask)
//                    .ConfigureAwait(false);
            });
            
            var tasksGroupAssets = Task.Run(async () =>
            {
                var assets = await AssetsReaderService.LoadContentsIdsAsync()
                    .ConfigureAwait(false);

                var tasks = assets.Select(CopyAssetByIdAsync);

                await Task.WhenAll(tasks)
                    .ConfigureAwait(false);
            });

            Task.WhenAll(tasksGroupAssets, tasksGroupContent)
                .ConfigureAwait(false);
        }

        #region Pages

        private async Task<List<PageModel>> LoadPagesAsync()
        {
            var pages = await PageModelReaderService.LoadContentsIdsAsync()
                .ConfigureAwait(false);

            var pagesContent = pages
                .Select(pageId => PageModelReaderService.LoadContentByIdAsync(pageId));

            await Task.WhenAll(pagesContent)
                .ConfigureAwait(false);

            return pagesContent
                .Select(x => x.Result)
                .ToList();
        }

        private async Task PopulateFeedsAsync(PageModel model)
        {
            if (model.IsFeedItem())
            {
            }

            if (model.IsFeedHost())
            {
            }
        }

        private async Task RenderPageAsync(PageModel page, ExpandoObject dataModel)
        {
            page.MapDataToModel(dataModel);
            
            var html = await TemplateService.RenderTemplateAsync(page.Content, page, dataModel)
                .ConfigureAwait(false);
            html = await MarkupService.RenderMarkupAsync(html)
                .ConfigureAwait(false);
            page.Content = html;
                
            var template = await TemplateReaderService.LoadContentByIdAsync(page.TemplateId)
                .ConfigureAwait(false);
            html = await TemplateService.RenderTemplateAsync(template.Template, page, dataModel)
                .ConfigureAwait(false);

            html = await HtmlMinificationService.CompressHtmlAsync(html)
                .ConfigureAwait(false);

            var result = new TextContentModel
            {
                Id = page.Id,
                ContentRaw = html
            };

            await PageModelWriterService.SaveContentAsync(page.Url, result)
                .ConfigureAwait(false);
        }

        private async Task ProcessPageByIdAsync(string pageId, ExpandoObject dataModel)
        {
            await LoggerService.WriteLogMessage($"{DateTime.Now.ToLongTimeString()}: [Page:Started] \"{pageId}\"")
                .ConfigureAwait(false);
            
            var page = await PageModelReaderService.LoadContentByIdAsync(pageId)
                .ConfigureAwait(false);
            
            page.MapDataToModel(dataModel);
            
            var html = await TemplateService.RenderTemplateAsync(page.Content, page, dataModel)
                .ConfigureAwait(false);
            html = await MarkupService.RenderMarkupAsync(html)
                .ConfigureAwait(false);
            page.Content = html;
                
            var template = await TemplateReaderService.LoadContentByIdAsync(page.TemplateId)
                .ConfigureAwait(false);
            html = await TemplateService.RenderTemplateAsync(template.Template, page, dataModel)
                .ConfigureAwait(false);

            html = await HtmlMinificationService.CompressHtmlAsync(html)
                .ConfigureAwait(false);

            var result = new TextContentModel
            {
                Id = page.Id,
                ContentRaw = html
            };

            await PageModelWriterService.SaveContentAsync(page.Url, result)
                .ConfigureAwait(false);
            
            await LoggerService.WriteLogMessage($"{DateTime.Now.ToLongTimeString()}: [Page:Finished] \"{pageId}\"")
                .ConfigureAwait(false);
        }

        #endregion

        #region Assets

        private async Task CopyAssetByIdAsync(string id)
        {
            await LoggerService.WriteLogMessage($"{DateTime.Now.ToLongTimeString()}: [Asset:Started] \"{id}\"")
                .ConfigureAwait(false);
            
            var content = await AssetsReaderService.LoadContentByIdAsync(id)
                .ConfigureAwait(false);

            await AssetsWriterService.SaveContentAsync(id, content)
                .ConfigureAwait(false);
            
            await LoggerService.WriteLogMessage($"{DateTime.Now.ToLongTimeString()}: [Asset:Finished] \"{id}\"")
                .ConfigureAwait(false);
        }

        #endregion
        
        private async Task CleanOutputAsync()
        {
            await LoggerService.WriteLogMessage($"{DateTime.Now.ToLongTimeString()}: Starting cleaning output folder")
                .ConfigureAwait(false);

            await Task.WhenAll
            (
                PageModelWriterService.CleanFolderAsync(String.Empty),
                AssetsWriterService.CleanFolderAsync(String.Empty)
            ).ConfigureAwait(false);

            await LoggerService.WriteLogMessage($"{DateTime.Now.ToLongTimeString()}: Cleaning output folder finished")
                .ConfigureAwait(false);
        }
    }
}