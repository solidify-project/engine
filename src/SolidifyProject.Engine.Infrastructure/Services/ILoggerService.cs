using System.Threading.Tasks;

namespace SolidifyProject.Engine.Infrastructure.Services
{
    public interface ILoggerService
    {
        Task WriteLogMessage(string message);
    }
}