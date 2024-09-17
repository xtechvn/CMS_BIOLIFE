using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Utilities.ViewModels.Article;

namespace Utilities
{
    public class StringHelpers
    {

        private static readonly string[] VietNamChar = new string[]{
           "aAeEoOuUiIdDyY",
           "áàạảãâấầậẩẫăắằặẳẵ",
           "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ",
           "éèẹẻẽêếềệểễ",
           "ÉÈẸẺẼÊẾỀỆỂỄ",
           "óòọỏõôốồộổỗơớờợởỡ",
           "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ",
           "úùụủũưứừựửữ",
           "ÚÙỤỦŨƯỨỪỰỬỮ",
           "íìịỉĩ",
           "ÍÌỊỈĨ",
           "đ",
           "Đ",
           "ýỳỵỷỹ",
           "ÝỲỴỶỸ"
        };
        private static readonly string CharacterVN = @"á à ạ ả ã â ấ ầ ậ ẩ ẫ ă ắ ằ ặ ẳ ẵ Á À Ạ Ả Ã Â Ấ Ầ Ậ Ẩ Ẫ Ă Ắ Ằ Ặ Ẳ Ẵ é è ẹ ẻ ẽ ê ế ề ệ ể ễ É È Ẹ Ẻ Ẽ Ê Ế Ề Ệ Ể Ễ ó ò ọ ỏ õ ô ố ồ ộ ổ ỗ ơ ớ ờ ợ ở ỡ Ó Ò Ọ Ỏ Õ Ô Ố Ồ Ộ Ổ Ỗ Ơ Ớ Ờ Ợ Ở Ỡ ú ù ụ ủ ũ ư ứ ừ ự ử ữ Ú Ù Ụ Ủ Ũ Ư Ứ Ừ Ự Ử Ữ í ì ị ỉ ĩ Í Ì Ị Ỉ Ĩ";
        private static readonly string[] SpecialCharacter = new string[] { " ", "%", "/", "*", "+", "_", "@", "&", "$", "#", "%", ",", ";" };

        public static bool CheckString(string str)
        {
            var listStr = CharacterVN.Split(' ');
            for (int i = 0; i < listStr.Count(); i++)
            {
                if (string.IsNullOrEmpty(listStr[i]))
                    continue;
                if (str.Contains(listStr[i]))
                {
                    return false;
                }
            }
            return true;
        }
        public static string CreateRandomPassword(int length = 6)
        {
            string validChars = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            Random random = new Random();
            char[] chars = new char[length];
            for (int i = 0; i < length; i++)
            {
                chars[i] = validChars[random.Next(0, validChars.Length)];
            }
            return new string(chars);
        }

        public static string CreateRandomNumb(int length)
        {
            string validChars = "0123456789";
            Random random = new Random();
            char[] chars = new char[length];
            for (int i = 0; i < length; i++)
            {
                chars[i] = validChars[random.Next(0, validChars.Length)];
            }
            return new string(chars);
        }

        public static string ConvertStringToNoSymbol(string str)
        {
            for (int i = 1; i < VietNamChar.Length; i++)
            {
                for (int j = 0; j < VietNamChar[i].Length; j++)
                    str = str.Replace(VietNamChar[i][j], VietNamChar[0][i - 1]);
            }
            return str;
        }

        public static string ConvertNewsUrlToNoSymbol(string str)
        {
            for (int i = 1; i < VietNamChar.Length; i++)
            {
                for (int j = 0; j < VietNamChar[i].Length; j++)
                    str = str.Replace(VietNamChar[i][j], VietNamChar[0][i - 1]);
            }
            str = CommonHelper.RemoveSpecialCharacters(str);

            str = str.Replace(" ", "-");

            foreach (var item in SpecialCharacter)
            {
                str = str.Replace(item, String.Empty);
            }

            return str.ToLower();
        }

        public static string ReplaceUrlPathSpecialCharacter(string str)
        {
            var result = ConvertStringToNoSymbol(str);
            foreach (var item in SpecialCharacter)
            {
                result = result.Replace(item, "-");
            }
            RegexOptions options = RegexOptions.None;
            Regex regex = new Regex("[-]{2,}", options);
            result = regex.Replace(result, "-");
            return result;
        }

        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;
            try
            {
                // Normalize the domain
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper, RegexOptions.None, TimeSpan.FromMilliseconds(200));

                // Examines the domain part of the email and normalizes it.
                string DomainMapper(Match match)
                {
                    // Use IdnMapping class to convert Unicode domain names.
                    var idn = new IdnMapping();

                    // Pull out and process domain name (throws ArgumentException on invalid)
                    var domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException e)
            {
                return false;
            }
            catch (ArgumentException e)
            {
                return false;
            }

            try
            {
                return Regex.IsMatch(email,
                    @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                    @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-0-9a-z]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }

        public static string FormatCurrency(double value)
        {
            var data = Math.Round(value, 2);
            string result = data.ToString(System.Globalization.CultureInfo.InvariantCulture);
            if (result.IndexOf('.') == -1)
            {
                return value.ToString("N0");
            }
            else
            {
                return data.ToString("N2").TrimEnd('0', '.');
            }
        }

        public static string FormatWeight(double value)
        {
            string result = value.ToString(System.Globalization.CultureInfo.InvariantCulture);
            if (result.IndexOf('.') == -1)
            {
                return value.ToString("N0");
            }
            else
            {
                return Math.Round(value, 4).ToString("N4").TrimEnd('0', '.');
            }
        }

        public static bool TryGetFromBase64String(string input, out byte[] output)
        {
            output = null;
            try
            {
                output = Convert.FromBase64String(input);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        public static string SaveBase64StringAsFile(string imgSrc, string folderName, string directoryName)
        {
            try
            {
                if (!string.IsNullOrEmpty(imgSrc))
                {
                    if (imgSrc.StartsWith("data:image"))
                    {
                        var base64Data = imgSrc.Split(',')[0];
                        var imageString = imgSrc.Split(',')[1];
                        var imageExtension = base64Data.Split(';')[0].Split('/')[1];

                        var IsValid = TryGetFromBase64String(imageString, out byte[] ImageByte);
                        if (IsValid)
                        {
                            string _FileName = Guid.NewGuid() + "." + imageExtension;
                            File.WriteAllBytes(Path.Combine(directoryName, _FileName), ImageByte);
                            return "/" + folderName + "/" + _FileName;
                        }
                    }
                    else
                    {
                        return imgSrc;
                    }
                }
            }
            catch (FormatException)
            {

            }
            return string.Empty;
        }

        public static ImageBase64 GetImageSrcBase64Object(string imgSrc)
        {
            try
            {
                if (!string.IsNullOrEmpty(imgSrc) && imgSrc.StartsWith("data:image"))
                {
                    var ImageBase64 = new ImageBase64();
                    var base64Data = imgSrc.Split(',')[0];
                    ImageBase64.ImageData = imgSrc.Split(',')[1];
                    ImageBase64.ImageExtension = base64Data.Split(';')[0].Split('/')[1];
                    return ImageBase64;
                }
            }
            catch (FormatException)
            {

            }
            return null;
        }

        public static async Task<string> ReplaceEditorImage(string html_string, string img_domain)
        {
            try
            {
                string regexImgSrc = @"<img[^>]*?src\s*=\s*[""']?([^'"" >]+?)[ '""][^>]*?>";
                MatchCollection matchesImgSrc = Regex.Matches(html_string, regexImgSrc, RegexOptions.IgnoreCase | RegexOptions.Singleline);
                if (matchesImgSrc != null && matchesImgSrc.Count > 0)
                {
                    foreach (Match m in matchesImgSrc)
                    {
                        var ImageApiLink = string.Empty;
                        string srcImage = m.Groups[1].Value;
                        if (srcImage.StartsWith("data:image"))
                        {
                            ImageApiLink = await UpLoadHelper.UploadBase64Src(srcImage, img_domain);
                        }
                        else if (!srcImage.StartsWith(img_domain))
                        {
                            var base64img = ConvertImageURLToBase64(WebUtility.HtmlDecode(srcImage));
                            if (!string.IsNullOrEmpty(base64img))
                            {
                                ImageApiLink = await UpLoadHelper.UploadBase64Src(base64img, img_domain);
                            }
                        }

                        if (!string.IsNullOrEmpty(ImageApiLink))
                        {
                            html_string = html_string.Replace(srcImage, img_domain + ImageApiLink);
                        }
                    }
                }
            }
            catch
            {

            }
            return html_string;
        }

        public static string ConvertImageURLToBase64(String url)
        {
            try
            {
                StringBuilder _sb = new StringBuilder();
                _sb.Append("data:image/jpg;base64,");

                Byte[] _byte = GetImage(url);

                _sb.Append(Convert.ToBase64String(_byte, 0, _byte.Length));
                return _sb.ToString();
            }
            catch
            {
                return null;
            }
        }

        private static byte[] GetImage(string url)
        {
            Stream stream = null;
            byte[] buf;

            try
            {
                WebProxy myProxy = new WebProxy();
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);

                HttpWebResponse response = (HttpWebResponse)req.GetResponse();
                stream = response.GetResponseStream();

                using (BinaryReader br = new BinaryReader(stream))
                {
                    int len = (int)(response.ContentLength);
                    buf = br.ReadBytes(len);
                    br.Close();
                }

                stream.Close();
                response.Close();
            }
            catch (Exception exp)
            {
                buf = null;
            }
            return (buf);
        }
         
        public static string GenFileName(string base_name, int user_id,string extension)
        {
            return base_name + "_" + user_id + "_" + DateTime.Now.ToString("yyyyMMddhhmmss") + "." + extension;
        }
     
    }
}
