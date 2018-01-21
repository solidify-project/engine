using System.Dynamic;
using System.Threading.Tasks;

namespace SolidifyProject.Engine.Infrastructure.Interfaces
{
    public interface IDataService
    {
        Task<ExpandoObject> GetDataModelAsync();
    }
}