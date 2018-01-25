using System.Collections.Generic;
using System.Linq;

namespace SolidifyProject.Engine.Infrastructure.Services
{
    public sealed class QueryService
    {
        private readonly dynamic _data;
        
        public QueryService(dynamic data)
        {
            _data = data;
        }
        
        public IEnumerable<dynamic> Query(
            string filterString = null,
            string orderString = null,
            int? top = null,
            int? skip = null)
        {
            IEnumerable<dynamic> results = _data.__collection;
            
            if (skip.HasValue)
            {
                results = results.Skip(skip.Value);
            }

            if (top.HasValue)
            {
                results = results.Take(top.Value);
            }

            yield return results;
        }

        public int Count(string filterString = null)
        {
            var results = (IEnumerable<dynamic>) _data.__collection;

            if (results.Any())
            {
                return results.Count();
            }

            return 0;
        }
    }
}