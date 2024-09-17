using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Utilities
{
    // Hàm này sẽ chứa những method mà các nơi khác sẽ sử dụng

    public static class PresentationUtils
    {
        //Quy ước kiểu biểu thức chính quy cho Url và Email
        public const string UrlPattern = @"(^$)|(^(https?|ftp):\/\/(((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:)*@)?(((\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5]))|((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?)(:\d*)?)(\/((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)+(\/(([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)*)*)?)?(\?((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)|[\uE000-\uF8FF]|\/|\?)*)?(\#((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)|\/|\?)*)?$)";
        public const string EmailPattern = @"^(\s*)([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)(\s*)$";
        public const string MobilePattern = @"[0-9]{10}";


        // Hàm này dùng để mã hóa chuỗi password theo chuẩn MD5
        public static string Encrypt(this string value)
        {
            try
            {
                if (string.IsNullOrEmpty(value))
                    return string.Empty;
                var md5 = new MD5CryptoServiceProvider();
                byte[] valueArray = Encoding.ASCII.GetBytes(value);
                valueArray = md5.ComputeHash(valueArray);
                var sb = new StringBuilder();
                for (int i = 0; i < valueArray.Length; i++)
                    sb.Append(valueArray[i].ToString("x2").ToLower());
                return sb.ToString();
            }
            catch (Exception ex)
            {
                //ErrorWriter.WriteLog(HttpContext.Current.Server.MapPath("~"), String.Format("Encrypt(Pass={0})", value), ex.ToString());
                throw;
            }

        }
        public static string UrlEncode(this string text)
        {
            return HttpUtility.UrlEncode(text);
        }

        public static string UrlDecode(this string text)
        {
            return HttpUtility.UrlDecode(text);
        }

        ////Hàm này dùng để Xác định xem User hiện tại có phải là Owner blog hay không ?
        //public static bool IsBlogOwner(this HttpRequest request, IPrincipal user)
        //{
        //    return request.IsAuthenticated && ((EnhancedPrincipal)user).Data.IsOwner;
        //}

        //// Hàm này nhận vào 1 chuỗi và tách nó ra tương ứng với mỗi cái Tag
        public static IEnumerable<string> ParseTagsString(string tagsString)
        {
            return tagsString.Split(',').Select(name => name.ToLowerInvariant().Trim()).Distinct();
        }       

    }
}
