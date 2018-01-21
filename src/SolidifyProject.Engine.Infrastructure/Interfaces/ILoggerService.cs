using System.Threading.Tasks;

namespace SolidifyProject.Engine.Infrastructure.Interfaces
{
    public interface ILoggerService
    {
        Task WriteLogMessage(string message);
    }
}