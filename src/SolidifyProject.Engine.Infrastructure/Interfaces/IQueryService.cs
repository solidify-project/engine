using System.Collections.Generic;
using System.Threading.Tasks;

namespace SolidifyProject.Engine.Infrastructure.Interfaces
{
    public interface IQueryService
    {
        Task<IEnumerable<dynamic>> ExecuteQueryAsync(
            string prefix = null,
            int? top = null,
            int? skip = null);
    }
}