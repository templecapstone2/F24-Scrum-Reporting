using Capstone.Models;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;

namespace Capstone.Services
{
    public class ExportService
    {
        public ExportService() { }

        public string GenerateCSV<T>(List<T> list)
        {
            StringBuilder sb = new StringBuilder();
            PropertyInfo[] properties = typeof(T).GetProperties();

            foreach (var property in properties)
            {
                var jsonPropertyName = property.GetCustomAttributes(typeof(JsonPropertyNameAttribute), false)
           .FirstOrDefault() as JsonPropertyNameAttribute;

                // Use json attribute name if present, otherwise use property name
                string columnName = jsonPropertyName != null ? jsonPropertyName.Name : property.Name;

                sb.Append(columnName + ",");
            }
            sb.Remove(sb.Length - 1, 1);
            sb.AppendLine();

            foreach (T item in list)
            {
                foreach (var property in properties)
                {
                    string value = property.GetValue(item)?.ToString() ?? "";  // Use empty string for null values
                    value = value.Contains(",") ? $"\"{value.Replace("\"", "\"\"")}\"" : value;
                    sb.Append(value + ",");
                }
                sb.Remove(sb.Length - 1, 1);
                sb.AppendLine();
            }

            return sb.ToString();
        }
    }
}
