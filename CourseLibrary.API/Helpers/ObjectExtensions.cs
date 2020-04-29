using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace CourseLibrary.API.Helpers
{
    public static class ObjectExtensions
    {
        public static ExpandoObject ShapeData<TSource>(this TSource source, string fields)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            var dataShapedObject = new ExpandoObject();

            if (string.IsNullOrWhiteSpace(fields))
            {
                var propertyInfos = typeof(TSource).GetProperties(BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                foreach (var propertyInfo in propertyInfos)
                {
                    var propertyValue = propertyInfo.GetValue(source);
                    if (!dataShapedObject.TryAdd(propertyInfo.Name, propertyValue))
                    {
                        throw new Exception($"Couldn't add property {propertyInfo.Name} with value {propertyValue}");
                    }
                }
            }
            else
            {
                var splittedFields = fields.Split(',');

                foreach (var field in splittedFields)
                {
                    var propertyName = field.Trim();

                    var propertyInfo = typeof(TSource).GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                    var propertyValue = propertyInfo.GetValue(source);
                    if (!dataShapedObject.TryAdd(propertyInfo.Name, propertyValue))
                    {
                        throw new Exception($"Couldn't add property {propertyInfo.Name} with value {propertyValue}");
                    }
                }
            }

            return dataShapedObject;
        }
    }
}
