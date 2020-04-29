using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;

namespace CourseLibrary.API.Services
{
    public class PropertyCheckerService : IPropertyCheckerService
    {
        public bool TypeHasProperties<T>(string fields)
        {
            if (string.IsNullOrWhiteSpace(fields))
            {
                return true;
            }

            var splittedFields = fields.Split(',');

            foreach (var field in splittedFields)
            {
                var propertyName = fields.Trim();

                var propertyInfo = typeof(T)
                    .GetProperty(name: propertyName,
                    bindingAttr: BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                if (propertyInfo == null)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
