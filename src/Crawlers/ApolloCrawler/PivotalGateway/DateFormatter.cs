using System;
using System.Globalization;

namespace Apollo.PivotalGateway
{
    public class DateFormatter
    {
        public static DateTime? FormatNullable( string dateString, string[] formatStrings )
        {
            foreach (var formatString in formatStrings)
            {
                DateTime result;

                if( DateTime.TryParseExact(dateString, formatString, CultureInfo.CurrentCulture,DateTimeStyles.None, out result))
                {
                    return result;
                }
            }

            return null;
        }

        public static DateTime? FormatNullable(string dateString)
        {
            return FormatNullable(dateString, new[] { "yyyy/MM/dd HH:mm:ss EDT", "yyyy/MM/dd HH:mm:ss EST" });
        }

        public static DateTime Format(string dateString, string[] formatStrings)
        {
            foreach (var formatString in formatStrings)
            {
                DateTime result;

                if (DateTime.TryParseExact(dateString, formatString, CultureInfo.CurrentCulture, DateTimeStyles.None, out result))
                {
                    return result;
                }
            }

            return DateTime.MinValue;
        }

        public static DateTime Format(string dateString)
        {
            return Format(dateString, new[] { "yyyy/MM/dd HH:mm:ss EDT", "yyyy/MM/dd HH:mm:ss EST" });
        }
    }
}
