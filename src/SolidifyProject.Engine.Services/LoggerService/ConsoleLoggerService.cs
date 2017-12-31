using System;
using System.Threading.Tasks;
using SolidifyProject.Engine.Infrastructure.Services;

namespace SolidifyProject.Engine.Services.LoggerService
{
    public class ConsoleLoggerService : ILoggerService
    {
        public Task WriteLogMessage(string message)
        {
            Console.WriteLine(message);
            
            return Task.FromResult<object>(null);
        }
    }
}