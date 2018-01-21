using System.IO;
using McMaster.Extensions.CommandLineUtils;
using SolidifyProject.Engine.Configuration;
using SolidifyProject.Engine.Infrastructure.Models;
using SolidifyProject.Engine.Infrastructure.Models.Base;
using SolidifyProject.Engine.Infrastructure.Services;
using SolidifyProject.Engine.Services;
using SolidifyProject.Engine.Services.ContentReaderService;
using SolidifyProject.Engine.Services.ContentWriterService;
using SolidifyProject.Engine.Services.HtmlMinificationService;
using SolidifyProject.Engine.Services.MarkupService;
using SolidifyProject.Engine.Services.TemplateService;

namespace SolidifyProject.Engine.CLI.Commands
{
    public static partial class CliCommands
    {
        public static void RenderCommand(CommandLineApplication command)
        {
            command.Description = "Renders static website";
                
            command.HelpOption("-?|-h|--help");

//            var source = command.Option("-s|--source <SOURCE>", "Path to folder with source files", CommandOptionType.SingleValue);
//            var destination = command.Option("-d|--destination <DESTINATION>", "Path to folder where static website will be generated", CommandOptionType.SingleValue);
            
            command.OnExecute(() =>
            {
                LoggerService.WriteLogMessage("Rendering static website").Wait();

//                var sourcePath = source.HasValue() ? source.Value() : Directory.GetCurrentDirectory();
//                var destinationPath = destination.HasValue() ? destination.Value() : Path.Combine(Directory.GetCurrentDirectory(), "WWW");

                var configurationService = new ConfigurationService();
                var sourcePath = configurationService.Configuration.Source.Path;
                var outputPath = configurationService.Configuration.Output.Path;
                
                LoggerService.WriteLogMessage($"Source: {sourcePath}").Wait();
                LoggerService.WriteLogMessage($"Output: {outputPath}").Wait();

                var orchestrationService = new OrchestrationService();
                
                orchestrationService.LoggerService = LoggerService;
                orchestrationService.TemplateService = new MustacheTemplateService(new FileSystemTextContentReaderService<TextContentModel>(Path.Combine(sourcePath, "layout", "partials")));
                orchestrationService.MarkupService = new MarkdownMarkupService();
                orchestrationService.HtmlMinificationService = new GoogleHtmlMinificationService();
                
                orchestrationService.AssetsReaderService = new FileSystemBinaryContentReaderService<BinaryContentModel>(Path.Combine(sourcePath, "assets"));
                orchestrationService.PageModelReaderService = new FileSystemTextContentReaderService<PageModel>(Path.Combine(sourcePath, "pages"));
                orchestrationService.TemplateReaderService = new FileSystemTextContentReaderService<TemplateModel>(Path.Combine(sourcePath, "layout"));
                orchestrationService.DataReaderService = new FileSystemTextContentReaderService<CustomDataModel>(Path.Combine(sourcePath, "data"));
                
                orchestrationService.PageModelWriterService = new FileSystemTextContentWriterService<TextContentModel>(Path.Combine(outputPath));
                orchestrationService.AssetsWriterService = new FileSystemBinaryContentWriterService<BinaryContentModel>(Path.Combine(outputPath, "assets"));
    
                orchestrationService.RenderProject().Wait();

                LoggerService.WriteLogMessage("Rendering finished successfully").Wait();

                return 0;
            });
        }
    }
}