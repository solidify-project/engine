using System.IO;
using McMaster.Extensions.CommandLineUtils;
using SolidifyProject.Engine.Configuration;
using SolidifyProject.Engine.Infrastructure.Models.Base;
using SolidifyProject.Engine.Services.ContentReaderService;
using SolidifyProject.Engine.Services.ContentWriterService;

namespace SolidifyProject.Engine.CLI.Commands
{
    public static partial class CliCommands
    {
        public static void BootstrapCommand(CommandLineApplication command)
        {
            command.Description = "Bootstraps folders structure for static website";
                
            command.HelpOption("-?|-h|--help");

//            var destination = command.Option("-d|--destination <DESTINATION>", "Path to folder where static website will be generated", CommandOptionType.SingleValue);
            
            command.OnExecute(() =>
            {
                LoggerService.WriteLogMessage("Bootstraping folder strcuture").Wait();

//                var files = new[]
//                {
//                    "Assets/README.md",
//                    "Data/README.md",
//                    "Layout/README.md",
//                    "Layout/Partials/README.md",
//                    "Pages/README.md",
//                    "WWW/README.md",
//                    "README.md"
//                };
//                var destinationPath = destination.HasValue() ? destination.Value() : Directory.GetCurrentDirectory();
                
                var configurationService = new ConfigurationService();
                var enginePath = configurationService.Configuration.Engine.Path;
                var sourcePath = configurationService.Configuration.Source.Path;
                
                LoggerService.WriteLogMessage($"Engine: {enginePath}").Wait();
                LoggerService.WriteLogMessage($"Source: {sourcePath}").Wait();

                var dataReader = new FileSystemBinaryContentReaderService<BinaryContentModel>(Path.Combine(enginePath, "Data", "src"));
                var dataWriter = new FileSystemBinaryContentWriterService<BinaryContentModel>(Path.Combine(sourcePath, "src"));

                var files = dataReader.LoadContentsIdsAsync().Result;
                
                foreach (var file in files)
                {
                    var content = dataReader.LoadContentByIdAsync(file).Result;

                    dataWriter.SaveContentAsync(file, content).Wait();
                }
                
                LoggerService.WriteLogMessage("Bootstraping finished successfully").Wait();

                return 0;
            });
        }
    }
}