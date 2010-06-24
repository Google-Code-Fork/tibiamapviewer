using System;
using Keyrox.Shared.Culture;
using Keyrox.Shared.Objects;

namespace Keyrox.Shared.DateTimeUtils {
    public static class DateTimeUtils {

        /// <summary>
        /// Toes the time span.
        /// </summary>
        /// <param name="valor">The valor.</param>
        /// <returns></returns>
        public static TimeSpan ToTimeSpan(this decimal valor) {
            var hours = Convert.ToInt32(((int)valor).ToString("00", CultureManager.Portuguese));
            var minutes = Convert.ToInt32(((int)Math.Round(((valor - (int)valor) * 60), 0)).ToString("00", CultureManager.Portuguese));
            return new TimeSpan(hours, minutes, 0);
        }

        /// <summary>
        /// Toes the time span format.
        /// </summary>
        /// <param name="valor">The valor.</param>
        /// <returns></returns>
        public static string ToTimeSpanFormat(this decimal valor) {
            var hours = Convert.ToInt32(((int)valor).ToString("00", CultureManager.Portuguese));
            var minutes = Convert.ToInt32(((int)Math.Round(((valor - (int)valor) * 60), 0)).ToString("00", CultureManager.Portuguese));
            var ts = new TimeSpan(hours, minutes, 0);

            var totalHours = ts.TotalHours.ToInt32().ToString().Length == 1 ? "0" + ts.TotalHours.ToInt32() : ts.TotalHours.ToInt32().ToString();
            var totalMinutes = ts.Minutes.ToInt32().ToString().Length == 1 ? "0" + ts.Minutes.ToInt32() : ts.Minutes.ToInt32().ToString();
            return totalHours + ":" + totalMinutes;
        }

        /// <summary>
        /// Toes the time span.
        /// </summary>
        /// <param name="valor">The valor.</param>
        /// <param name="intervalo">if set to <c>true</c> [intervalo].</param>
        /// <returns></returns>
        public static TimeSpan ToTimeSpan(this decimal valor, bool intervalo) {
            return intervalo ? new TimeSpan(0, Convert.ToInt32(valor), 0) : ToTimeSpan(valor);
        }

        /// <summary>
        /// Toes the date time.
        /// </summary>
        /// <param name="valor">The valor.</param>
        /// <returns></returns>
        public static DateTime ToDateTime(this decimal valor) {
            var sHora = ((int)valor).ToString("00", CultureManager.Portuguese) + ":" + ((int)Math.Round(((valor - (int)valor) * 60), 0)).ToString("00", CultureManager.Portuguese);
            return Convert.ToDateTime(sHora, CultureManager.Portuguese);
        }

        /// <summary>
        /// Toes the date time.
        /// </summary>
        /// <param name="valor">The valor.</param>
        /// <returns></returns>
        public static DateTime ToDateTime(this TimeSpan valor) {
            try { return new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, valor.Hours, valor.Minutes, valor.Seconds); }
            catch { return DateTime.MinValue; }
        }

        /// <summary>
        /// Toes the decimal.
        /// </summary>
        /// <param name="valor">The valor.</param>
        /// <returns></returns>
        public static decimal ToDecimal(this TimeSpan valor) {
            return (decimal)(valor.TotalMinutes / 60);
        }

        /// <summary>
        /// Toes the decimal.
        /// </summary>
        /// <param name="valor">The valor.</param>
        /// <param name="intervalo">if set to <c>true</c> [intervalo].</param>
        /// <returns></returns>
        public static decimal ToDecimal(this TimeSpan valor, bool intervalo) {
            return intervalo ? Convert.ToDecimal(valor.TotalMinutes) : ToDecimal(valor);
        }

        /// <summary>
        /// Totals the horas.
        /// </summary>
        /// <param name="entrada">The entrada.</param>
        /// <param name="saida">The saida.</param>
        /// <param name="intervalo">The intervalo.</param>
        /// <returns></returns>
        public static TimeSpan TotalHoras(this decimal entrada, decimal saida, decimal intervalo) {
            return ToTimeSpan((saida - entrada) - intervalo);
        }
    }
}
