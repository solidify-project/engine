using System.IO;
using McMaster.Extensions.CommandLineUtils;
using SolidifyProject.Engine.Infrastructure.Models.Base;
using SolidifyProject.Engine.Services.ContentWriterService;

namespace SolidifyProject.Engine.CLI.Commands
{
    public static partial class CliCommands
    {
        public static void BootstrapCommand(CommandLineApplication command)
        {
            command.Description = "Bootstraps folders structure for static website";
                
            command.HelpOption("-?|-h|--help");

            var destination = command.Option("-d|--destination <DESTINATION>", "Path to folder where static website will be generated", CommandOptionType.SingleValue);
            
            command.OnExecute(() =>
            {
                LoggerService.WriteLogMessage("Bootstraping folder strcuture").Wait();

                var files = new[]
                {
                    "Assets/README.md",
                    "Data/README.md",
                    "Layout/README.md",
                    "Layout/Partials/README.md",
                    "Pages/README.md",
                    "WWW/README.md",
                    "README.md"
                };
                var destinationPath = destination.HasValue() ? destination.Value() : Directory.GetCurrentDirectory();
                
                LoggerService.WriteLogMessage($"Destination: {destinationPath}").Wait();

                foreach (var file in files)
                {
                    var writer = new FileSystemTextContentWriterService<TextContentModel>(Path.Combine(destinationPath));
                    writer.SaveContentAsync(file, new TextContentModel{ Id = file, ContentRaw = string.Empty}).Wait();
                }
                
                LoggerService.WriteLogMessage("Bootstraping finished successfully").Wait();

                return 0;
            });
        }
    }
}