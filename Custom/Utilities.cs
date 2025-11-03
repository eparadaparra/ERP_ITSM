using System.Globalization;

namespace ERP_ITSM.Custom
{
    public class Utilities
    {
        private static readonly string[] formatosEntrada =
        {
            "yyyy-MM-dd HH:mm:ss.fff",
            "yyyy-MM-ddTHH:mm:ss",
            "yyyy-MM-dd HH:mm:ss",
            "yyyy-MM-ddTHH:mm:ss.fff",
            "yyyy-MM-dd"
        };

        public static string UTC_UTC6(string fechaUTC, string timeZoneID = "Central Standard Time")
        {
            timeZoneID = TimeZoneID(timeZoneID);
            DateTime fecha = ParsearFecha(fechaUTC);
            DateTime fechaUtc = DateTime.SpecifyKind(fecha, DateTimeKind.Utc);

            TimeZoneInfo zonaUTCMenos6 = TimeZoneInfo.FindSystemTimeZoneById(timeZoneID);
            DateTime fechaLocal = TimeZoneInfo.ConvertTimeFromUtc(fechaUtc, zonaUTCMenos6);

            // Formato fijo de salida
            return fechaLocal.ToString("yyyy-MM-dd HH:mm:ss.fff");
        }

        public static string UTC6_UTC(string fechaUTCMenos6, string timeZoneID = "Central Standard Time")
        {
            timeZoneID = TimeZoneID(timeZoneID);
            DateTime fecha = ParsearFecha(fechaUTCMenos6);
            DateTime fechaLocal = DateTime.SpecifyKind(fecha, DateTimeKind.Unspecified);

            TimeZoneInfo zonaUTCMenos6 = TimeZoneInfo.FindSystemTimeZoneById(timeZoneID);
            DateTime fechaUtc = TimeZoneInfo.ConvertTimeToUtc(fechaLocal, zonaUTCMenos6);

            // Formato fijo de salida
            return fechaUtc.ToString("yyyy-MM-dd HH:mm:ss.fff");
        }
        
        private static string TimeZoneID(string timeZoneID)
        {
            return timeZoneID switch
            {
                "Central Standard Time" => $"{timeZoneID} (Mexico)",
                _ => $"{timeZoneID} (Mexico)"
            };
        }

        private static DateTime ParsearFecha(string fechaString)
        {
            // Normalizar reemplazando 'T' por espacio
            string fechaNormalizada = fechaString.Replace('T', ' ');

            if (DateTime.TryParseExact(fechaNormalizada, formatosEntrada, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime fecha))
            {
                return fecha;
            }

            // Intentar parseo genérico como fallback
            if (DateTime.TryParse(fechaString, CultureInfo.InvariantCulture, DateTimeStyles.None, out fecha))
            {
                return fecha;
            }

            throw new ArgumentException($"Formato de fecha no reconocido: {fechaString}");
        }

    }
}
