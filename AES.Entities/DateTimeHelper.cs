using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AES.Entities
{
    public class DateTimeHelper
    {
        public static TimeSpan ConvertDateTime(DateTime dateTime)
        {
            return new TimeSpan(dateTime.Hour, dateTime.Minute, dateTime.Second);
        }

        public static DateTime ConvertTimeSpan(TimeSpan timeSpan)
        {

            return new DateTime(1970, 1, 1, timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
        }
    }
}
