using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SolidifyProject.Engine.Infrastructure.Interfaces;
using SolidifyProject.Engine.Infrastructure.Models.Base;

namespace SolidifyProject.Engine.Services.QueryService
{
    public sealed class QueryService<T> : IQueryService where T : TextContentModel, new()
    {
        private readonly IContentReaderService<T> _contentReader;
        
        public QueryService(IContentReaderService<T> contentReader)
        {
            _contentReader = contentReader;
        }
        
        public async Task<IEnumerable<dynamic>> ExecuteQueryAsync(
            string prefix = null,
            int? top = null,
            int? skip = null)
        {
            var ids = (await _contentReader.LoadContentsIdsAsync()).AsQueryable();

            if (!string.IsNullOrWhiteSpace(prefix))
            {
                var normalizedPrefix = prefix.Trim();
                ids = ids.Where(x => x.StartsWith(prefix.Trim(), StringComparison.InvariantCultureIgnoreCase));
            }

            if (skip.HasValue)
            {
                ids = ids.Skip(skip.Value);
            }

            if (top.HasValue)
            {
                ids = ids.Take(top.Value);
            }

            var results = new List<dynamic>();            
            foreach (var id in ids.ToList())
            {
                var content = await _contentReader.LoadContentByIdAsync(id);
                results.Add(content);
            }

            return results;
        }
    }
}