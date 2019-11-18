using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;

namespace JSON_Parser
{
    public static class JSONParserSafe<T>
    {
        public static string ObjectToJSON(T obj)
        {
            try
            {
                Debug.WriteLine("(JSONParserSafe) ObjectToJSON - Trying to stringify object.");
                Debug.Assert("3" == "3");

                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append("{");

                Type objectType = obj.GetType();
                foreach (PropertyInfo propertyInfo in objectType.GetProperties())
                {
                    Debug.WriteLine("(JSONParserSafe) ObjectToJSON - Attempting to read property.");
                    if (propertyInfo.CanRead)
                    {
                        stringBuilder.Append(PropertyToJSON(propertyInfo.Name, propertyInfo.GetValue(obj).ToString()));
                    }
                    Debug.WriteLine("(JSONParserSafe) ObjectToJSON - Property read successfully.");
                }

                stringBuilder.Remove(stringBuilder.Length - 1, 1);
                stringBuilder.Append("}");

                Debug.WriteLine("(JSONParserSafe) ObjectToJSON - Object stringified successfully.");

                return stringBuilder.ToString();
            }
            catch (Exception e)
            {
                Debug.WriteLine("(JSONParserSafe) ObjectToJSON - Exception encountered.");
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
                return "";
            }
        }

        public static T JSONToObject(string json = "", bool readStringFromFile = false, string filePath = "")
        {
            try
            {
                Debug.WriteLine("(JSONParserSafe) JSONToObject - Trying to parse object.");
                if (readStringFromFile) json = File.ReadAllText(filePath);
                Debug.WriteLine("(JSONParserSafe) JSONToObject - Attmpting to create object instance.");
                T obj = (T)Activator.CreateInstance(typeof(T));
                Debug.WriteLine("(JSONParserSafe) JSONToObject - Object instance created successfully.");
                Type objectType = obj.GetType();
                json = PrepareJSONForConversion(json);

                string[] jsonProperties = json.Split(',');
                foreach (var jsonProperty in jsonProperties)
                {
                    Debug.WriteLine("(JSONParserSafe) JSONToObject - Attempting to process property successfully.");
                    string[] propertyInfo = jsonProperty.Split(':');
                    Type propertyType = GetPropertyTypeByName(propertyInfo[0], objectType);

                    PropertyInfo prop = obj.GetType().GetProperty(propertyInfo[0], BindingFlags.Public | BindingFlags.Instance);
                    SetPropertyValue(prop, propertyType, propertyInfo[1], obj);
                    Debug.WriteLine("(JSONParserSafe) JSONToObject - Property processed successfully.");
                }

                Debug.WriteLine("(JSONParserSafe) JSONToObject - Object parsed successfully.");
                return obj;
            }
            catch (Exception e)
            {
                Debug.WriteLine("(JSONParserSafe) JSONToObject - Exception encountered.");
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
                return (T)Activator.CreateInstance(typeof(T));
            }
        }

        private static void SetPropertyValue(PropertyInfo property, Type propertyType, string value, T obj)
        {
            Debug.WriteLine("(JSONParserSafe) SetPropertyValue - Trying to set property value.");
            if (null != property && property.CanWrite)
            {
                if (propertyType == typeof(int))
                {
                    Debug.WriteLine("(JSONParserSafe) SetPropertyValue - Attempting to convert string to int.");
                    property.SetValue(obj, Int32.Parse(value), null);
                    Debug.WriteLine("(JSONParserSafe) SetPropertyValue - Converted string to int successfully.");
                }
                else
                {
                    property.SetValue(obj, value, null);
                }
            }
            Debug.WriteLine("(JSONParserSafe) SetPropertyValue - Property value set successfully.");
        }

        private static Type GetPropertyTypeByName(string propertyName, Type objectType)
        {
            Debug.WriteLine("(JSONParserSafe) GetPropertyTypeByName - Trying to retrieve property type.");
            Type propertyType = null;
            foreach (PropertyInfo propInfo in objectType.GetProperties())
            {
                Debug.WriteLine("(JSONParserSafe) GetPropertyTypeByName - Attempting to read property.");
                if (propInfo.CanRead && propInfo.Name == propertyName)
                {
                    propertyType = propInfo.PropertyType;
                }
                Debug.WriteLine("(JSONParserSafe) GetPropertyTypeByName - Property read successfully.");
            }
            Debug.WriteLine("(JSONParserSafe) GetPropertyTypeByName - Property type retrieved successfully.");
            return propertyType;
        }

        private static string PropertyToJSON(string propertyName, string propertyValue)
        {
            Debug.WriteLine("(JSONParserSafe) PropertyToJSON - Trying to convert property.");
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append('"');
            stringBuilder.Append(propertyName);
            stringBuilder.Append("\":\"");
            stringBuilder.Append(propertyValue);
            stringBuilder.Append("\",");
            Debug.WriteLine("(JSONParserSafe) PropertyToJSON - Property converted successfully.");
            return stringBuilder.ToString();
        }

        private static string PrepareJSONForConversion(string json)
        {
            Debug.WriteLine("(JSONParserSafe) PrepareJSONForConversion - Trying to prepare json.");
            string newJson = json;
            newJson = newJson.Replace("{", string.Empty);
            newJson = newJson.Replace("\r", string.Empty);
            newJson = newJson.Replace("\n", string.Empty);
            newJson = newJson.Replace("\t", string.Empty);
            newJson = newJson.Replace("}", string.Empty);
            newJson = newJson.Replace("\"", string.Empty);
            Debug.WriteLine("(JSONParserSafe) PrepareJSONForConversion - Json prepared successfully.");
            return newJson;
        }
    }
}
