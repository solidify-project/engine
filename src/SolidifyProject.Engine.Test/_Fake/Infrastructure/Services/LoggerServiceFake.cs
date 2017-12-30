using System.Collections.Generic;
using System.Threading.Tasks;
using SolidifyProject.Engine.Infrastructure.Services;

namespace SolidifyProject.Engine.Test._Fake.Infrastructure.Services
{
    public class LoggerServiceFake : ILoggerService
    {
        private List<string> _logs = new List<string>();
        
        public async Task WriteLogMessage(string message)
        {
            _logs.Add(message);
        }
    }
}