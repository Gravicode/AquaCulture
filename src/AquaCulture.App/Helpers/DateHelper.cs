using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace AquaCulture.App.Helpers
{
    public class DateHelper
    {

        public static int GetSemesterFromDate(DateTime date)
        {
            var startDate = new DateTime(date.Year,1,1);
            var endDate = new DateTime(date.Year,6,1).AddMonths(1).AddDays(-1);
            return date >= startDate && date <= endDate ? 1 : 2;
        }
        static CultureInfo ci = new CultureInfo("id-ID");
        public static string GetMonthName(int month)
        {
            if (month < 1 || month > 12) return "";
            var monthName = ci.DateTimeFormat.GetMonthName(month);
            return monthName;
        }
        public static string GetDayName(DayOfWeek day)
        {
            var dayName = ci.DateTimeFormat.GetDayName(day);
            return dayName;
        }
        public static DateTime GetLocalTimeNow()
        {
            var localTimeZoneId = "SE Asia Standard Time";
            var localTimeZone = TimeZoneInfo.FindSystemTimeZoneById(localTimeZoneId);
            return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, localTimeZone);
        }
    }
}
