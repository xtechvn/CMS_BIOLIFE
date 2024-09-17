using System;
using System.Collections.Generic;
using System.Text;

namespace Utilities
{
    public static class DateUtil
    {
        public static DateTime? Parse(string dateStr, string dateFormat = "MM/dd/yyyy")
        {
            try
            {
                return DateTime.ParseExact(dateStr, dateFormat, null);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static string DateToString(DateTime? date, string dateFormat = "dd/MM/yyyy")
        {
            try
            {
                return date == null ? "" : ((DateTime)date).ToString(dateFormat);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static string DateTimeToString(DateTime? date, string dateFormat = "dd/MM/yyyy HH:mm:ss")
        {
            try
            {
                return date == null ? "" : ((DateTime)date).ToLocalTime().ToString(dateFormat);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static DateTime? StringToDate(string dateStr, string dateFormat = "dd/MM/yyyy")
        {
            try
            {
                return DateTime.SpecifyKind(DateTime.ParseExact(dateStr, dateFormat, null), DateTimeKind.Local);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static DateTime? StringToDateTime(string dateStr, string dateFormat = "dd/MM/yyyy HH:mm:ss")
        {
            try
            {
                return DateTime.SpecifyKind(DateTime.ParseExact(dateStr, dateFormat, null), DateTimeKind.Local);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static DateTime Now
        {
            get
            {
                return DateTime.Now;
            }
        }
        ///
        public static Boolean checkOverlapping(DateTime tStartA, DateTime tEndA, DateTime tStartB, DateTime tEndB)
        {
            bool overlap = tStartA < tEndB && tStartB < tEndA;
            return overlap;
        }

        /// <summary>
        /// Giả định các giá trị hợp lệ start &lt= end
        /// </summary>
        /// <param name="s1"> giá trị đầu khoảng 1</param>
        /// <param name="e1">giá trị kết thúc khoảng 1</param>
        /// <param name="s2">giá trị đầu khoảng 2</param>
        /// <param name="e2">iá trị kết thúc khoảng 2</param>
        /// <returns>-1: độc lập, 0: tiếp xúc, 1 giao nhau</returns>
        public static int checkTwoRangeIntersect(DateTime? s1, DateTime? e1, DateTime? s2, DateTime? e2)
        {
            int res = -1;
            if (s1 == null && e1 == null || s2 == null && e2 == null) return 1;
            if (s1 == null)
            {
                if ((s2 == null || e1 > s2)) return 1;
                if (e1 == s2) return 0;
                if (e1 < s2) return -1;
            }
            if (e1 == null)
            {
                if (e2 == null || e2 > s1) return 1;
                if (e2 == s1) return 0;
                if (e2 < s1) return -1;
            }
            if (s2 == null)
            {
                if (s1 < e2) return 1;
                if (s1 == e2) return 0;
                if (s1 > e2) return -1;
            }
            if (e2 == null)
            {
                if (e1 > s2) return 1;
                if (e1 == s2) return 0;
                if (e1 < s2) return -1;
            }
            if (s1 < e2 && s2 < e1) return 1;
            if (s1 == e2 || s2 == e1) return 0;

            return res;

        }

        public static double getInnerRangeMinute(DateTime s1, DateTime e1, DateTime s2, DateTime e2)
        {
            var totalmin = 0d;
            if (s1 <= e2 && s2 <= e1)
            {
                var d1 = e1.Subtract(s1).TotalMinutes;
                var d2 = e2.Subtract(s2).TotalMinutes;
                var dt1 = e2.Subtract(s1).TotalMinutes;
                var dt2 = e1.Subtract(s2).TotalMinutes;
                List<double> lstVal = new List<double>();
                lstVal.Add(d1);
                lstVal.Add(d2);
                lstVal.Add(dt1);
                lstVal.Add(dt2);
                lstVal.Sort();
                totalmin = lstVal[0];

            }

            return totalmin;

        }

        public static string DateTimeToStringNotLocal(DateTime? date, string dateFormat = "dd/MM/yyyy HH:mm:ss")
        {
            try
            {
                return date == null ? "" : ((DateTime)date).ToString(dateFormat, System.Globalization.CultureInfo.InvariantCulture);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// chuyển từ định dạng timespan sang datetime
        /// </summary>
        /// <param name="unixTimeStamp"></param>
        /// <returns></returns>
        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }
    }
}
