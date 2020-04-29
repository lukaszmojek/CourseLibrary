using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Reflection;

namespace CourseLibrary.API.Helpers
{
    public static class IEnumerableExtensions
    {
        public static IEnumerable<ExpandoObject> ShapeData<TSource>(this IEnumerable<TSource> source, string fields)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            var expandoObjects = new List<ExpandoObject>();

            var propertyInfos = new List<PropertyInfo>();

            if (string.IsNullOrWhiteSpace(fields))
            {
                var propertyInfo = typeof(TSource).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                propertyInfos.AddRange(propertyInfo);
            }
            else
            {
                var splittedFields = fields.Split(',');

                foreach (var field in splittedFields)
                {
                    var propertyName = field.Trim();

                    var propertyInfo = typeof(TSource).GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                    if (propertyInfo == null)
                    {
                        throw new Exception($"Property {propertyName} wasn't found on {typeof(TSource)}");
                    }

                    propertyInfos.Add(propertyInfo);
                }
            }

            foreach (TSource sourceObject in source)
            {
                var dataShapedObject = new ExpandoObject();

                foreach (var propertyInfo in propertyInfos)
                {
                    var propertyValue = propertyInfo.GetValue(sourceObject);
                    if (!dataShapedObject.TryAdd(propertyInfo.Name, propertyValue))
                    {
                        throw new Exception($"Couldn't add property {propertyInfo.Name} with value {propertyValue}");
                    } 
                }

                expandoObjects.Add(dataShapedObject);
            }

            return expandoObjects;
        }
    }
}
