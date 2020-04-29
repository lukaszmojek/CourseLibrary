using CourseLibrary.API.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace CourseLibrary.API.Helpers
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> ApplySorting<T>(this IQueryable<T> source, string orderBy, Dictionary<string, PropertyMappingValue> mappingDictionary)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (mappingDictionary == null)
            {
                throw new ArgumentNullException(nameof(mappingDictionary));
            }

            if (string.IsNullOrWhiteSpace(orderBy))
            {
                return source;
            }

            var orderByAfterSplitting = orderBy.Split(',');

            var orderByString = "";

            foreach (var orderByClause in orderByAfterSplitting.Reverse())
            {
                var trimmedOrderByCaluse = orderByClause.Trim();
                var orderDescending = trimmedOrderByCaluse.EndsWith(" desc");
                var indexOfFirstSpace = trimmedOrderByCaluse.IndexOf(" ");
                
                var propertyName = indexOfFirstSpace == -1 ?
                    trimmedOrderByCaluse : trimmedOrderByCaluse.Remove(indexOfFirstSpace);

                if (!mappingDictionary.ContainsKey(propertyName))
                {
                    throw new ArgumentException($"Key mapping for {propertyName} is missing");
                }

                var propertyMappingValue = mappingDictionary[propertyName];

                if (propertyMappingValue == null)
                {
                    throw new ArgumentNullException(nameof(propertyMappingValue));
                }

                

                foreach (var destinationProperty in propertyMappingValue.DestinationProperties)
                {
                    if (propertyMappingValue.Revert)
                    {
                        orderDescending = !orderDescending;
                    }

                    orderByString = orderByString 
                        + (string.IsNullOrWhiteSpace(orderByString) ? string.Empty : ", ") 
                        + destinationProperty 
                        + (orderDescending ? " descending" : " ascending");
                }
            }

            return source.OrderBy(orderByString);
        }
    }
}
