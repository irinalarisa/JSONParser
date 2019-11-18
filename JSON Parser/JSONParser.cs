using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace JSON_Parser
{
    public static class JSONParser<T>
    {
        public static string ObjectToJSON(T obj)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("{");

            Type objectType = obj.GetType();
            foreach (PropertyInfo propertyInfo in objectType.GetProperties())
            {
                if (propertyInfo.CanRead)
                {
                    stringBuilder.Append(PropertyToJSON(propertyInfo.Name, propertyInfo.GetValue(obj).ToString()));
                }
            }

            stringBuilder.Remove(stringBuilder.Length - 1, 1);
            stringBuilder.Append("}");
            return stringBuilder.ToString();
        }

        public static T JSONToObject(string json = "", bool readStringFromFile = false, string filePath = "")
        {
            if (readStringFromFile) json = File.ReadAllText(filePath);
            T obj = (T)Activator.CreateInstance(typeof(T));
            Type objectType = obj.GetType();
            json = PrepareJSONForConversion(json);

            string[] jsonProperties = json.Split(',');
            foreach (var jsonProperty in jsonProperties)
            {
                string[] propertyInfo = jsonProperty.Split(':');
                Type propertyType = GetPropertyTypeByName(propertyInfo[0], objectType);

                PropertyInfo prop = obj.GetType().GetProperty(propertyInfo[0], BindingFlags.Public | BindingFlags.Instance);
                SetPropertyValue(prop, propertyType, propertyInfo[1], obj);
            }

            return obj;
        }

        private static void SetPropertyValue(PropertyInfo property, Type propertyType, string value, T obj)
        {
            if (null != property && property.CanWrite)
            {
                if (propertyType == typeof(int))
                {
                    property.SetValue(obj, Int32.Parse(value), null);
                }
                else
                {
                    property.SetValue(obj, value, null);
                }
            }
        }

        private static Type GetPropertyTypeByName(string propertyName, Type objectType)
        {
            Type propertyType = null;
            foreach (PropertyInfo propInfo in objectType.GetProperties())
            {
                if (propInfo.CanRead && propInfo.Name == propertyName)
                {
                    propertyType = propInfo.PropertyType;
                }
            }
            return propertyType;
        }

        private static string PropertyToJSON(string propertyName, string propertyValue)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append('"');
            stringBuilder.Append(propertyName);
            stringBuilder.Append("\":\"");
            stringBuilder.Append(propertyValue);
            stringBuilder.Append("\","
                );
            return stringBuilder.ToString();
        }

        private static string PrepareJSONForConversion(string json)
        {
            string newJson = json;
            newJson = newJson.Replace("{", string.Empty);
            newJson = newJson.Replace("\r", string.Empty);
            newJson = newJson.Replace("\n", string.Empty);
            newJson = newJson.Replace("\t", string.Empty);
            newJson = newJson.Replace("}", string.Empty);
            newJson = newJson.Replace("\"", string.Empty);
            return newJson;
        }
    }
}
