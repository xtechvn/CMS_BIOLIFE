using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Utilities
{
    public static class ObjectExtension
    {
        /// <summary>
        /// copy properties of object
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        public static void CopyProperties(this object source, object destination, List<string> ignoreFields = null)
        {
            // If any this null throw an exception
            if (source == null || destination == null)
                return;
            //throw new Exception("Source or/and Destination Objects are null");
            // Getting the Types of the objects
            Type typeDest = destination.GetType();
            Type typeSrc = source.GetType();

            // Iterate the Properties of the source instance and  
            // populate them from their desination counterparts  
            PropertyInfo[] srcProps = typeSrc.GetProperties();
            foreach (PropertyInfo srcProp in srcProps)
            {
                if (!srcProp.CanRead)
                {
                    continue;
                }

                if (ignoreFields != null && ignoreFields.Contains(srcProp.Name))
                {
                    continue;
                }

                PropertyInfo targetProperty = typeDest.GetProperties().FirstOrDefault(p => p.Name == srcProp.Name);//GetProperty(srcProp.Name,BindingFlags.);
                if (targetProperty == null)
                {
                    continue;
                }
                if (!targetProperty.CanWrite)
                {
                    continue;
                }
                if (targetProperty.GetSetMethod(true) != null && targetProperty.GetSetMethod(true).IsPrivate)
                {
                    continue;
                }
                if ((targetProperty.GetSetMethod().Attributes & MethodAttributes.Static) != 0)
                {
                    continue;
                }
                if (!targetProperty.PropertyType.IsAssignableFrom(srcProp.PropertyType))
                {
                    continue;
                }
                // Passed all tests, lets set the value
                targetProperty.SetValue(destination, srcProp.GetValue(source, null), null);
            }
        }

        /// <summary>
        /// Convert to json string
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static JToken ToJsonString(this object source, bool microsoftDateFormat = true)
        {
            if (microsoftDateFormat)
            {
                JsonSerializerSettings microsoftDateFormatSettings = new JsonSerializerSettings
                {
                    //DateFormatHandling = DateFormatHandling.MicrosoftDateFormat,
                    DateTimeZoneHandling = DateTimeZoneHandling.Local
                };
                return JsonConvert.SerializeObject(source, microsoftDateFormatSettings);
            }
            else
            {
                return JsonConvert.SerializeObject(source);
            }

        }

        public static List<T> ToList<T>(this DataTable data) where T : new()
        {
            List<T> dtReturn = new List<T>();
            if (data == null)
                return dtReturn;

            Type typeParameterType = typeof(T);

            var props = typeParameterType.GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(x => x.GetGetMethod() != null);

            foreach (DataRow item in data.AsEnumerable())
            {
                dtReturn.Add(GetValueT<T>(item, props));
            }
            return dtReturn;
        }

        public static List<T> ToListBasic<T>(this DataTable data)
        {
            List<T> dtReturn = new List<T>();
            if (data == null)
                return dtReturn;
            Type typeParameterType = typeof(T);
            foreach (DataRow item in data.AsEnumerable())
            {
                dtReturn.Add((T)Convert.ChangeType(item[0], typeParameterType));
            }
            return dtReturn;
        }

        private static T GetValueT<T>(DataRow row, IEnumerable<PropertyInfo> props) where T : new()
        {
            T objRow = new T();
            foreach (var field in props)
            {
                string fieldName = field.Name;
                var columnName = field.CustomAttributes.FirstOrDefault(x => x.AttributeType.UnderlyingSystemType.Name == "ColumnAttribute");
                if (columnName != null)
                    fieldName = columnName.ConstructorArguments[0].Value.ToString();
                if (row.Table.Columns.Contains(fieldName) && row[fieldName] != DBNull.Value)
                {
                    Type t = Nullable.GetUnderlyingType(field.PropertyType) ?? field.PropertyType;
                    if (field.PropertyType.IsValueType)
                        field.SetValue(objRow, Convert.ChangeType(row[fieldName] == DBNull.Value ? Activator.CreateInstance(t) : row[fieldName], t));
                    else
                    {
                        field.SetValue(objRow, Convert.ChangeType(row[fieldName], t));
                    }
                }
            }
            return objRow;
        }

    }
}
