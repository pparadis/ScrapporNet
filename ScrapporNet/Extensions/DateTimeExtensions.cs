using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScrapporNet.Extensions
{
    public static class DateTimeExtensions
    {
        public static String GetTimestamp(this DateTime value)
        {
            return value.ToString("yyyyMMddHHmmssffff");
        }
    }
}
