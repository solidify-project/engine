using System.Collections.Generic;
using System.Linq;

namespace SolidifyProject.Engine.Helpers.PredicateParser
{
    public static class FilterCollection
    {
        public static IEnumerable<object> FilterCollectionBy(this IEnumerable<object> source, string filterString)
        {
            if (!source.Any())
            {
                return source;
            }
            
            return source;
        }
    }
}