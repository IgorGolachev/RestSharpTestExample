using System;
using System.ComponentModel;

namespace core.Util
{
    public static class EnumHelper
    {
        public static T GetValueByDescription<T>(string description)
        {
            var type = typeof(T);
            if (!typeof(T).IsEnum)
                throw new ArgumentException("T must be an enumerated type");

            foreach (var field in type.GetFields())
            {
                if (Attribute.GetCustomAttribute(field,
                    typeof(DescriptionAttribute)) is DescriptionAttribute attribute)
                {
                    if (attribute.Description == description)
                        return (T)field.GetValue(null);
                }
                else
                {
                    if (field.Name == description)
                        return (T)field.GetValue(null);
                }
            }
            throw new ArgumentException("Not found.", nameof(description));
            // or return default(T);
        }

        public static string GetDescription<T>(T item)
            where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
                throw new ArgumentException("T must be an enumerated type");

            DescriptionAttribute[] attributes = (DescriptionAttribute[])item
               .GetType()
               .GetField(item.ToString())
               .GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : string.Empty;
        }
    }
}