using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Specialized;

namespace Keyrox.Shared.Objects {
    public static class Objects {

        /// <summary>
        /// Método de Serialização de Objetos.
        /// </summary>
        /// <param name="obj">Objeto para serialização.</param>
        /// <returns>Representação XML do Objeto serializado.</returns>
        public static string Serialize(this object obj) {
            if (obj != null) {
                var serializer = new XmlSerializer(obj.GetType());
                var xmlContent = new StringBuilder();
                var writer = XmlWriter.Create(xmlContent, new XmlWriterSettings() { Indent = true });
                if (writer == null) return "";
                serializer.Serialize(writer, obj);
                return xmlContent.ToString();
            }
            throw new NullReferenceException();
        }

        /// <summary>
        /// Método de Deserialização de Objetos.
        /// </summary>
        /// <param name="xml">Conteúdo para deserializar.</param>
        /// <returns>Objeto deserializado.</returns>
        public static T Deserialize<T>(this string xml) {
            var serializer = new XmlSerializer(typeof(T));
            if (serializer.CanDeserialize(XmlReader.Create(new StringReader(xml)))) {
                return (T)serializer.Deserialize(new StringReader(xml));
            }
            throw new FormatException();
        }

        #region "[rgn] Verification "
        /// <summary>
        /// Determines whether the specified value is null.
        /// </summary>
        /// <param name="value">The value.</param>
        public static bool IsNull(this object value) {
            try {
                return value == null;
            }
            catch { return true; }
        }

        /// <summary>
        /// Determines whether the specified value is empty.
        /// </summary>
        /// <param name="value">The value.</param>
        public static bool IsEmpty(this object value) {
            try {
                return value.ToString() == "";
            }
            catch { return false; }
        }

        /// <summary>
        /// Toes the positive.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static int ToPositive(this int value) {
            var result = value;
            if (result < 0) return result * (-1);
            return result;
        }

        /// <summary>
        /// Toes the positive.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static uint ToPositive(this uint value) {
            return ToPositive(value.ToInt32()).ToUInt32();
        }

        /// <summary>
        /// Determines whether [is date time] [the specified value].
        /// </summary>
        /// <param name="value">The value.</param>
        public static bool IsDateTime(this object value) {
            try {
                DateTime outvalue;
                return DateTime.TryParse(value.ToString(), out outvalue);
            }
            catch { return false; }
        }

        /// <summary>
        /// Determines whether the specified value is numeric.
        /// </summary>
        /// <param name="value">The value.</param>
        public static bool IsNumeric(this object value) {
            try {
                double outvalue;
                return double.TryParse(value.ToString(), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out outvalue);
            }
            catch { return false; }
        }
        #endregion

        #region "[rgn] Conversion   "
        /// <summary>
        /// Toes the date time.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static DateTime ToDateTime(this object value) {
            try {
                if (value.IsDateTime()) {
                    DateTime outvalue;
                    DateTime.TryParse(value.ToString(), out outvalue);
                    return outvalue;
                }
                return DateTime.MinValue;
            }
            catch { return DateTime.MinValue; }
        }

        /// <summary>
        /// Toes the int32.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static int ToInt32(this object value) {
            try {
                if (value.IsNumeric()) {
                    int outvalue;
                    int.TryParse(value.ToString(), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out outvalue);
                    return outvalue;
                }
                return 0;
            }
            catch { return 0; }
        }

        /// <summary>
        /// Toes the int32.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static uint ToUInt32(this object value) {
            try {
                if (value.IsNumeric()) {
                    uint outvalue;
                    uint.TryParse(value.ToString(), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out outvalue);
                    return outvalue;
                }
                return 0;
            }
            catch { return 0; }
        }

        /// <summary>
        /// Toes the long.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static double ToDouble(this object value) {
            try {
                if (value.IsNumeric()) {
                    double outvalue;
                    double.TryParse(value.ToString(), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out outvalue);
                    return outvalue;
                }
                return 0;
            }
            catch { return 0; }
        }

        /// <summary>
        /// Toes the long.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static long ToLong(this object value) {
            try {
                if (value.IsNumeric()) {
                    long outvalue;
                    long.TryParse(value.ToString(), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out outvalue);
                    return outvalue;
                }
                return 0;
            }
            catch { return 0; }
        }


        /// <summary>
        /// Toes the boolean.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static bool ToBoolean(this object value) {
            try {
                bool outvalue;
                bool.TryParse(value.ToString(), out outvalue);
                return outvalue;
            }
            catch { return false; }
        }

        /// <summary>
        /// Toes the specified value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static T To<T>(this object value) {
            try { return (T)value; }
            catch (InvalidCastException) { return default(T); }
        }
        #endregion

        /// <summary>
        /// Determines whether [contains] [the specified text].
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="items">The items.</param>
        /// <returns>
        /// 	<c>true</c> if [contains] [the specified text]; otherwise, <c>false</c>.
        /// </returns>
        public static bool Contains(this string text, StringCollection items) {
            foreach (var item in items) {

                var words = item.Split(new[]{ " ".ToChar() }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var word in words) {
                    if (!text.ToLower().Contains(word.ToLower())) {
                        words = null; break;
                    }
                }
                if (words != null) return true;
            }
            return false;
        }

        /// <summary>
        /// Toes the char.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static char ToChar(this object value) {
            try {
                return Convert.ToChar(value);
            }
            catch (Exception) { return default(char); }
        }

        /// <summary>
        /// Exists the in.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value.</param>
        /// <param name="list">The list.</param>
        /// <returns></returns>
        public static bool ExistIn<T>(this T value, List<T> list) {
            return list.Contains(value);
        }

        /// <summary>
        /// Toes the time span format.
        /// </summary>
        /// <param name="valor">The valor.</param>
        /// <returns></returns>
        public static TimeSpan ToTimeSpanFormat(this double valor) {
            var hours = Convert.ToInt32(((int)valor).ToString("00"));
            var minutes = Convert.ToInt32(((int)Math.Round(((valor - (int)valor) * 60), 0)).ToString("00"));
            return new TimeSpan(hours, minutes, 0);
        }

        /// <summary>
        /// Fills the zeros left.
        /// </summary>
        /// <param name="number">The number.</param>
        /// <param name="zeros">The zeros.</param>
        /// <returns></returns>
        public static string FillZerosLeft(this int number, int zeros) {
            var n = "";
            for (var i = 0; i < zeros; i++) n += "0";
            n += number.ToString();
            return n;
        }

        /// <summary>
        /// Toes the date time.
        /// </summary>
        /// <param name="valor">The valor.</param>
        /// <returns></returns>
        public static DateTime ToDateTime(this TimeSpan valor) {
            return new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, valor.Hours, valor.Minutes,
                                valor.Seconds);
        }

        /// <summary>
        /// Fits the with zero left.
        /// </summary>
        /// <param name="number">The number.</param>
        /// <param name="size">The size.</param>
        /// <returns></returns>
        public static string FitWithZeroLeft(this int number, int size) {
            return FillZerosLeft(number, size - number.ToString().Length);
        }


        /// <summary>
        /// Fits the with zero left.
        /// </summary>
        /// <param name="number">The number.</param>
        /// <param name="size">The size.</param>
        /// <returns></returns>
        public static string FitWithZeroLeft(this double number, int size) {
            return FillZerosLeft(number.ToInt32(), size - number.ToString().Length);
        }

        /// <summary>
        /// Redims the specified text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="newSize">The new size.</param>
        /// <returns></returns>
        public static string Redim(this string text, int newSize) {
            if (text.Length > newSize) {
                return text.Substring(0, newSize);
            }
            return text;
        }
    }
}
