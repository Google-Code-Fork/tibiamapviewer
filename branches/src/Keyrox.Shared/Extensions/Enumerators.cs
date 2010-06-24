using System;
using System.ComponentModel;
using System.Text;
using System.Collections.Generic;

namespace Keyrox.Shared.Enumerators {
    public static class Enumerators {
        /// <summary>
        /// Returns the text description for a enum value, using the DescriptionAttribute if it exists or the enum name if not.
        /// </summary>
        /// <param name="value">Enum value.</param>
        public static string Description(this Enum value) {
            return GetEnumText(value.GetType(), value.ToString());
        }


        /// <summary>
        /// Value of the specified enum.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static int Value(this Enum value) {
            return value.GetHashCode();
        }

        /// <summary>
        /// Text of the specified enum.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static string Text(this Enum value) {
            return value.ToString();
        }

        /// <summary>
        /// Returns the text description for a enum value, using the DescriptionAttribute if it exists or the enum name if not.
        /// </summary>
        /// <param name="type">Enum type to be used.</param>
        /// <param name="value">Enum value.</param>
        public static string GetEnumText(this Type type, int value) {
            var name = Enum.GetName(type, value);
            return GetEnumText(type, name);
        }

        /// <summary>
        /// Returns the text description for a enum name, using the DescriptionAttribute if it exists or the enum name if not.
        /// </summary>
        /// <param name="type">Enum type to be used.</param>
        /// <param name="name">Enum name.</param>
        public static string GetEnumText(this Type type, string name) {
            var fi = type.GetField(name);
            var attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes
                (typeof(DescriptionAttribute), false);
            return (attributes.Length > 0) ? attributes[0].Description : name;
        }

        /// <summary>
        /// Gets the enum items.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static Dictionary<string, T> GetEnumItems<T>() {
            return GetEnumItems<T>(false);
        }

        /// <summary>
        /// Gets the enum items.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="useDescription">if set to <c>true</c> [use description].</param>
        /// <returns></returns>
        public static Dictionary<string, T> GetEnumItems<T>(bool useDescription) {
            var result = new Dictionary<string, T>();
            int iterator = 0;
            var fields = typeof(T).GetFields();
            foreach (var field in fields) {
                if (iterator > 0) {
                    if(useDescription)
                        result.Add(((Enum)field.GetValue(null)).Description(), ((T)field.GetValue(null)));
                    else
                        result.Add(((Enum)field.GetValue(null)).Text(), ((T)field.GetValue(null)));
                }
                iterator++;
            }
            return result;
        }

        /// <summary>
        /// Gets the enum list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="keys">The keys.</param>
        /// <returns></returns>
        public static List<T> GetEnumList<T>(this List<string> keys, bool description) {
            var result = new List<T>();
            var enumItems = GetEnumItems<T>(description);
            foreach (var key in keys) {
                result.Add(enumItems[key]);
            }
            return result;
        }

        /// <summary>
        /// Gets the enum item.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="description">The description.</param>
        /// <returns></returns>
        public static T GetEnumItem<T>(this string description) {
            var enumItems = GetEnumItems<T>(true);
            if(enumItems.ContainsKey(description)) return enumItems[description];
            return default(T);
        }
        
        /// <summary>
        /// Returns a xml with the name, value and description for a given enum.
        /// </summary>
        /// <param name="type">Enum type.</param>
        public static string GetXMLFromEnum(this Type type) {
            var sb = new StringBuilder();
            var items = Enum.GetValues(type);

            sb.Append("<EnumValues>");
            foreach(Enum item in items) {
                sb.Append("<EnumValue>");
                sb.AppendFormat("<Text>{0}</Text>", Description(item));
                sb.AppendFormat("<Name>{0}</Name>", item);
                sb.AppendFormat("<Value>{0}</Value>", item.GetHashCode());
                sb.Append("</EnumValue>");
            }
            sb.Append("</EnumValues>");

            return sb.ToString();
        }
    }
}
