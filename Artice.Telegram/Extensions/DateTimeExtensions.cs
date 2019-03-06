using System;

namespace Artice.Telegram.Extensions
{
    /// <summary>
    /// Extension Methods
    /// </summary>
    public static class DateTimeExtensions
    {
        private static readonly DateTime UnixStart = new DateTime(1970, 1, 1);

        public static DateTime FromUnixTimeSeconds(this long dateTime) => UnixStart.AddSeconds(dateTime);

        public static long ToUnixTimeSeconds(this DateTime dateTime)
        {
            if (dateTime == DateTime.MinValue)
                return 0;

            var delta = dateTime - UnixStart;

            if (delta.TotalSeconds < 0)
                throw new ArgumentOutOfRangeException(nameof(dateTime), "Unix epoc starts January 1st, 1970");

            return (long)delta.TotalSeconds;
        }

        public static DateTime FromUnixTimeMilliseconds(this long dateTime) => UnixStart.AddMilliseconds(dateTime);

        public static long ToUnixTimeMilliseconds(this DateTime dateTime)
        {
            if (dateTime == DateTime.MinValue)
                return 0;

            var delta = dateTime - UnixStart;

            if (delta.TotalMilliseconds < 0)
                throw new ArgumentOutOfRangeException(nameof(dateTime), "Unix epoc starts January 1st, 1970");

            return (long)delta.TotalMilliseconds;
        }
    }
}
