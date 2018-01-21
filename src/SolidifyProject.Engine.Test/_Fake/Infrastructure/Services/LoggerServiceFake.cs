using System.Collections.Generic;
using System.Threading.Tasks;
using SolidifyProject.Engine.Infrastructure.Interfaces;

namespace SolidifyProject.Engine.Test._Fake.Infrastructure.Services
{
    public class LoggerServiceFake : ILoggerService
    {
        private readonly List<string> _logs = new List<string>();
        
        public Task WriteLogMessage(string message)
        {
            _logs.Add(message);
            
            return Task.FromResult<object>(null);
        }
    }
}