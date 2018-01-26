using System;
using System.Collections.Generic;
using System.Linq;

namespace SolidifyProject.Engine.Helpers.PredicateParser
{
    public static class OrderCollection
    {
        public const string ORDER_SEPARATOR = ",";
        public const string ORDER_ASCENDING = "asc";
        public const string ORDER_DESCENDING = "desc";
        public const string ORDER_CLAUSE_SEPARATOR = " ";
        
        private class OrderClause
        {
            public bool IsAscending { get; set; }
            public string PropertyName { get; set; }

            public Func<object, object> GetSortingFunc()
            {
                return item => item
                    .GetType()
                    .GetProperty(PropertyName)
                    .GetValue(item);
            }
        }
        
        public static IEnumerable<object> OrderCollectionBy(this IEnumerable<object> source, string orderString)
        {
            if (!source.Any())
            {
                return source;
            }

            var clauses = orderString
                .Split(new[] {ORDER_SEPARATOR}, StringSplitOptions.RemoveEmptyEntries)
                .Select(GetOrderClause)
                .ToList();

            if (!clauses.Any())
            {
                return source;
            }
            
            var firstClause = clauses.First();
            var orderedSource = firstClause.IsAscending ?
                source.OrderBy(firstClause.GetSortingFunc()) :
                source.OrderByDescending(firstClause.GetSortingFunc());

            orderedSource = clauses
                .Skip(1)
                .Aggregate(orderedSource, (ordered, currentClause) =>
                {
                    return currentClause.IsAscending ?
                        ordered.ThenBy(currentClause.GetSortingFunc()) :
                        ordered.ThenByDescending(currentClause.GetSortingFunc());
                });

            source = orderedSource;

            return source;
        }

        private static OrderClause GetOrderClause(string chunk)
        {
            var oClause = new OrderClause();
            var clauses = chunk.Split(new[] {ORDER_CLAUSE_SEPARATOR}, StringSplitOptions.RemoveEmptyEntries);
            switch (clauses.Length)
            {
                case 1:
                    oClause.IsAscending = true;
                    oClause.PropertyName = clauses[0];
                    break;
                case 2:
                    if (clauses[1].Equals(ORDER_ASCENDING, StringComparison.InvariantCultureIgnoreCase))
                    {
                        oClause.IsAscending = true;
                    }
                    else if (clauses[1].Equals(ORDER_DESCENDING, StringComparison.InvariantCultureIgnoreCase))
                    {
                        oClause.IsAscending = false;
                    }
                    else
                    {
                        throw new FormatException();
                    }
                    oClause.PropertyName = clauses[0];
                    break;
                default:
                    throw new FormatException();
            }
            return oClause;
        }
    }
}