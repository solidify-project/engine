using System.Collections.Generic;
using System.Linq;
using SolidifyProject.Engine.Helpers.PredicateParser;

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

            if (!string.IsNullOrWhiteSpace(orderString))
            {
                results = results.OrderCollectionBy(orderString);
            }

            if (skip.HasValue)
            {
                results = results.Skip(skip.Value);
            }

            if (top.HasValue)
            {
                results = results.Take(top.Value);
            }

            return results;
        }

        public int Count(string filterString = null)
        {
            IEnumerable<dynamic> results = _data.__collection;

            if (results.Any())
            {
                return results.Count();
            }

            return 0;
        }
    }
}