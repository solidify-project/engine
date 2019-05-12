using System.Diagnostics;
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

            command.OnExecute(() =>
            {
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                
                LoggerService.WriteLogMessage("Bootstraping folder structure").Wait();

                var configurationService = new ConfigurationService();
                var examplePath = Path.Combine(Directory.GetCurrentDirectory(), configurationService.Configuration.Engine.Path, "Data", "src");
                var sourcePath = Path.Combine(Directory.GetCurrentDirectory(), configurationService.Configuration.Source.Path);
                
                LoggerService.WriteLogMessage($"Example from: {examplePath}").Wait();
                LoggerService.WriteLogMessage($"Bootstraped to: {sourcePath}").Wait();

                var dataReader = new FileSystemBinaryContentReaderService<BinaryContentModel>(examplePath);
                var dataWriter = new FileSystemBinaryContentWriterService<BinaryContentModel>(sourcePath);

                var taskGetList = dataReader.LoadContentsIdsAsync(true);
                taskGetList.Wait();
                
                var files = taskGetList.Result;
                
                foreach (var file in files)
                {
                    var taskLoadContent = dataReader.LoadContentByIdAsync(file);
                    taskLoadContent.Wait();
                    
                    var content = taskLoadContent.Result;
                    dataWriter.SaveContentAsync(file, content).Wait();
                    
                    LoggerService.WriteLogMessage($"Bootstraped file: {file}").Wait();
                }
                
                LoggerService.WriteLogMessage("Bootstraping finished successfully").Wait();
                
                stopwatch.Stop();
                LoggerService.WriteLogMessage($"Time spent: {stopwatch.Elapsed:G}").Wait();

                return 0;
            });
        }
    }
}