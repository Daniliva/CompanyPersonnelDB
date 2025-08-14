using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Reflection;

namespace Project.DataAccess
{
    public class DataMapper<T> : IDataMapper<T> where T : class, new()
    {
        public virtual T Map(IDataRecord reader)
        {
            var obj = new T();
            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            for (int i = 0; i < reader.FieldCount; i++)
            {
                var fieldName = reader.GetName(i).ToLower();
                PropertyInfo property = properties.FirstOrDefault(p =>
                    p.GetCustomAttribute<ColumnAttribute>()?.Name?.ToLower() == fieldName)!;

                if (property == null)
                {
                    property = properties.FirstOrDefault(p => p.Name.ToLower() == fieldName)!;
                }

                if (property != null && !reader.IsDBNull(i))
                {
                    var value = reader.GetValue(i);
                    try
                    {
                        var targetType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
                        var convertedValue = value is DBNull ? null : Convert.ChangeType(value, targetType);
                        property.SetValue(obj, convertedValue);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error mapping field {fieldName} to property {property.Name}: {ex.Message}");
                    }
                }
            }

            return obj;
        }
    }
}