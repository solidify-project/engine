using SolidifyProject.Engine.Infrastructure.Interfaces;

namespace SolidifyProject.Engine.CLI.Commands
{
    public static partial class CliCommands
    {
        public static ILoggerService LoggerService { get; set; }
    }
}