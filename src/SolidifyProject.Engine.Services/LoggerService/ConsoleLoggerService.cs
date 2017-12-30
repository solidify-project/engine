using System;
using System.Threading.Tasks;
using SolidifyProject.Engine.Infrastructure.Services;

namespace SolidifyProject.Engine.Services.LoggerService
{
    public class ConsoleLoggerService : ILoggerService
    {
        public async Task WriteLogMessage(string message)
        {
            Console.WriteLine(message);
        }
    }
}