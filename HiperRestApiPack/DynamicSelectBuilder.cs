using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace HiperRestApiPack
{
    public class DynamicSelectBuilder<T>
    {
        private static ConcurrentDictionary<Type, PropertyInfo[]> typePropertyInfoMappings =
            new ConcurrentDictionary<Type, PropertyInfo[]>();
        private static ConcurrentDictionary<string, Func<T, T>> compiledSelects =
            new ConcurrentDictionary<string, Func<T, T>>();
        private readonly Type typeOfBaseClass = typeof(T);
        

        private Dictionary<string, List<string>> GetFieldMapping(string fields)
        {
            var selectedFields = new Dictionary<string, List<string>>();

            if (string.IsNullOrEmpty(fields))
            {
                return null;
            }

            foreach (var s in fields.Split(','))
            {
                var nestedFields = s.Split('.').Select(f => f.Trim()).ToArray();
                var nestedValue = nestedFields.Length > 1 ? nestedFields[1] : null;

                if (selectedFields.Keys.Any(key => key == nestedFields[0]))
                {
                    selectedFields[nestedFields[0]].Add(nestedValue);
                }
                else
                {
                    selectedFields.Add(nestedFields[0], new List<string> { nestedValue });
                }
            }

            return selectedFields;
        }

        public Func<T, T> CreateNewStatement(string fields)
        {
            var selectFields = GetFieldMapping(fields);
            if (selectFields == null)
            {
                return s => s;
            }
            string currentSelectKey = $"{typeOfBaseClass}_{fields}";
            return compiledSelects.GetOrAdd(currentSelectKey, (e) =>
            {
                ParameterExpression xParameter = Expression.Parameter(typeOfBaseClass, "s");
                NewExpression xNew = Expression.New(typeOfBaseClass);

                var shpNestedPropertyBindings = new List<MemberAssignment>();
                foreach (var keyValuePair in selectFields)
                {
                    PropertyInfo[] propertyInfos =
                    typePropertyInfoMappings.GetOrAdd(typeOfBaseClass, (r) =>
                    {
                        return typeOfBaseClass.GetProperties();
                    });

                    var propertyType = propertyInfos
                        .FirstOrDefault(p => p.Name.ToLowerInvariant().Equals(keyValuePair.Key.ToLowerInvariant()))
                        .PropertyType;
                        Expression mbr = xParameter;
                        mbr = Expression.PropertyOrField(mbr, keyValuePair.Key);

                        PropertyInfo mi = typeOfBaseClass.GetProperty(((MemberExpression)mbr).Member.Name);

                        var xOriginal = Expression.Property(xParameter, mi);

                        shpNestedPropertyBindings.Add(Expression.Bind(mi, xOriginal));
                    
                }

                var xInit = Expression.MemberInit(xNew, shpNestedPropertyBindings);
                var lambda = Expression.Lambda<Func<T, T>>(xInit, xParameter);

                return lambda.Compile();
            });
        }
    }
}
