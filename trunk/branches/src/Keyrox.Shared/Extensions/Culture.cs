using System.Globalization;

namespace Keyrox.Shared.Culture {
    /// <summary>
    /// Class used for managing Cultures.
    /// </summary>
    public static class CultureManager {

        /// <summary>
        /// Português Brasil.
        /// </summary>
        public static CultureInfo Portuguese {
            get {
                var ci = new CultureInfo("pt-br");
                ci.NumberFormat.CurrencyDecimalDigits = 2;
                ci.NumberFormat.CurrencyDecimalSeparator = ",";
                ci.NumberFormat.CurrencyGroupSeparator = ".";
                ci.NumberFormat.CurrencyGroupSizes = new[] { 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 };
                ci.NumberFormat.CurrencySymbol = "R$";
                ci.NumberFormat.NegativeSign = "-";
                ci.NumberFormat.NumberDecimalDigits = 2;
                ci.NumberFormat.NumberDecimalSeparator = ",";
                ci.NumberFormat.NumberGroupSeparator = ".";
                ci.NumberFormat.NumberGroupSizes = new[] { 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 };
                ci.NumberFormat.PercentDecimalDigits = 2;
                ci.NumberFormat.PercentDecimalSeparator = ",";
                ci.NumberFormat.PercentGroupSeparator = ".";
                ci.NumberFormat.PercentGroupSizes = new[] { 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 };
                ci.NumberFormat.PercentSymbol = "%";

                ci.DateTimeFormat.DateSeparator = "/";
                ci.DateTimeFormat.AbbreviatedDayNames = new[] { "Dom", "Seg", "Ter", "Qua", "Qui", "Sex", "Sáb" };
                ci.DateTimeFormat.AbbreviatedMonthNames = new[] { string.Empty, "Jan", "Fev", "Mar", "Abr", "Mai", "Jun", "Jul", "Ago", "Set", "Out", "Nov", "Dez" };
                ci.DateTimeFormat.AMDesignator = "AM";
                ci.DateTimeFormat.DayNames = new[] { "Domingo", "Segunda-Feira", "Terça-Feira", "Quarta-Feira", "Quinta-Feira", "Sexta-Feira", "Sábado" };
                ci.DateTimeFormat.FullDateTimePattern = "dd/MM/yyyy HH:mm:ss";
                ci.DateTimeFormat.LongDatePattern = "dd/MM/yyyy";
                ci.DateTimeFormat.LongTimePattern = "HH:mm:ss";
                ci.DateTimeFormat.MonthNames = new[] { string.Empty, "Janeiro", "Feveireiro", "Março", "Abril", "Maio", "Junho", "Julho", "Agosto", "Setembro", "Outubro", "Novembro", "Dezembro" };
                ci.DateTimeFormat.PMDesignator = "PM";
                ci.DateTimeFormat.TimeSeparator = ":";

                return ci;
            }
        }

        /// <summary>
        /// English US.
        /// </summary>
        public static CultureInfo English {
            get {
                var ci = new CultureInfo("en-us");
                ci.NumberFormat.CurrencyDecimalDigits = 2;
                ci.NumberFormat.CurrencyDecimalSeparator = ".";
                ci.NumberFormat.CurrencyGroupSeparator = ",";
                ci.NumberFormat.CurrencyGroupSizes = new[] { 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 };
                ci.NumberFormat.CurrencySymbol = "$";
                ci.NumberFormat.NegativeSign = "-";
                ci.NumberFormat.NumberDecimalDigits = 2;
                ci.NumberFormat.NumberDecimalSeparator = ".";
                ci.NumberFormat.NumberGroupSeparator = ",";
                ci.NumberFormat.NumberGroupSizes = new[] { 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 };
                ci.NumberFormat.PercentDecimalDigits = 2;
                ci.NumberFormat.PercentDecimalSeparator = ".";
                ci.NumberFormat.PercentGroupSeparator = ",";
                ci.NumberFormat.PercentGroupSizes = new[] { 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 };
                ci.NumberFormat.PercentSymbol = "%";

                ci.DateTimeFormat.DateSeparator = "/";
                ci.DateTimeFormat.AbbreviatedDayNames = new[] { "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat" };
                ci.DateTimeFormat.AbbreviatedMonthNames = new[] { string.Empty, "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
                ci.DateTimeFormat.AMDesignator = "AM";
                ci.DateTimeFormat.DayNames = new[] { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };
                ci.DateTimeFormat.FullDateTimePattern = "MM/dd/yyyy HH:mm:ss";
                ci.DateTimeFormat.LongDatePattern = "MM/dd/yyyy";
                ci.DateTimeFormat.LongTimePattern = "HH:mm:ss";
                ci.DateTimeFormat.MonthNames = new[] { string.Empty, "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
                ci.DateTimeFormat.PMDesignator = "PM";
                ci.DateTimeFormat.TimeSeparator = ":";

                return ci;
            }
        }
    }
}
