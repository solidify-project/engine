using McMaster.Extensions.CommandLineUtils;
using SolidifyProject.Engine.CLI.Commands;
using SolidifyProject.Engine.Infrastructure.Services;
using SolidifyProject.Engine.Services.LoggerService;

namespace SolidifyProject.Engine.CLI
{
    public class Program
    {
        private static readonly ILoggerService LoggerService = new ConsoleLoggerService();
        
        public static void Main(string[] args)
        {
            CliCommands.LoggerService = LoggerService;
            
            var app = new CommandLineApplication();
            app.Name = "SSG.CLI";
            app.Description = "Static Site Generator";

            app.HelpOption("-?|-h|--help");
            
            app.OnExecute(() =>
            {
                app.ShowHelp();

                return 0;
            });
            
            app.Command("render", CliCommands.RenderCommand);
            app.Command("bootstrap", CliCommands.BootstrapCommand);
            
            app.Execute(args);
        }
    }
}