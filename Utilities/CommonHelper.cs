using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using Tesseract;
using Utilities.Captcha;
using Utilities.Contants;

namespace Utilities
{
    public static class CommonHelper
    {
        public static bool GetParamWithKey(string Token, out JArray objParr, string EncryptApi)
        {
            objParr = null;
            try
            {

                Token = Token.Replace(" ", "+");
                // var serializer = new JavaScriptSerializer();                
                var jsonContent = GetContentObject(Token, EncryptApi);
                objParr = JArray.Parse("[" + jsonContent + "]");
                if (objParr != null && objParr.Count > 0)
                {
                    return true;
                }
                else { return false; }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetParamWithKey - CommonHelper: " + ex + "--Token =  " + Token);
                return false;
            }
        }

        public static string GetContentObject(string sContentEncode, string sKey)
        {
            try
            {
                // api.insidekp: Key quy uoc giua  2 ben | parramKey: tham so dong
                sContentEncode = sContentEncode.Replace(" ", "+");

                string data = Decode(sContentEncode, sKey); // Lay ra content 
                return data;
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetContentObject - CommonHelper: " + ex);
                // ErrorWriter.WriteLog(System.Web.HttpContext.Current.Server.MapPath("~"), "GiaiMa()", ex.ToString());
                return string.Empty;
            }

        }
        public static string Decode(string strString, string strKeyPhrase)
        {
            try
            {
                Byte[] byt = Convert.FromBase64String(strString);
                strString = System.Text.Encoding.UTF8.GetString(byt);
                strString = KeyED(strString, strKeyPhrase);
                return strString;
            }
            catch (Exception ex)
            {

                return strString;
            }
        }
        public static string Encode(string strString, string strKeyPhrase)
        {
            try
            {
                strString = KeyED(strString, strKeyPhrase);
                Byte[] byt = System.Text.Encoding.UTF8.GetBytes(strString);
                strString = Convert.ToBase64String(byt);
                return strString;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }

        }
        private static string KeyED(string strString, string strKeyphrase)
        {
            int strStringLength = strString.Length;
            int strKeyPhraseLength = strKeyphrase.Length;

            System.Text.StringBuilder builder = new System.Text.StringBuilder(strString);

            for (int i = 0; i < strStringLength; i++)
            {
                int pos = i % strKeyPhraseLength;
                int xorCurrPos = (int)(strString[i]) ^ (int)(strKeyphrase[pos]);
                builder[i] = Convert.ToChar(xorCurrPos);
            }

            return builder.ToString();
        }

       
        public static string RemoveUnicode(string text)
        {
            string[] arr1 = new string[] { "á", "à", "ả", "ã", "ạ", "â", "ấ", "ầ", "ẩ", "ẫ", "ậ", "ă", "ắ", "ằ", "ẳ", "ẵ", "ặ",
    "đ",
    "é","è","ẻ","ẽ","ẹ","ê","ế","ề","ể","ễ","ệ",
    "í","ì","ỉ","ĩ","ị",
    "ó","ò","ỏ","õ","ọ","ô","ố","ồ","ổ","ỗ","ộ","ơ","ớ","ờ","ở","ỡ","ợ",
    "ú","ù","ủ","ũ","ụ","ư","ứ","ừ","ử","ữ","ự",
    "ý","ỳ","ỷ","ỹ","ỵ",};
            string[] arr2 = new string[] { "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a",
    "d",
    "e","e","e","e","e","e","e","e","e","e","e",
    "i","i","i","i","i",
    "o","o","o","o","o","o","o","o","o","o","o","o","o","o","o","o","o",
    "u","u","u","u","u","u","u","u","u","u","u",
    "y","y","y","y","y",};
            for (int i = 0; i < arr1.Length; i++)
            {
                text = text.Replace(arr1[i], arr2[i]);
                text = text.Replace(arr1[i].ToUpper(), arr2[i].ToUpper());
            }
            return text;
        }
    
        public static string genLinkNews(string Title, string article_id)
        {
            Title = RemoveUnicode(CheckMaxLength(Title.Trim(), 100));
            Title = RemoveSpecialCharacters(CheckMaxLength(Title.Trim(), 100));
            Title = Title.Replace(" ", "-").ToLower();
            return ("/" + Title + "-" + article_id + ".html");
        }
        public static string genLinkNewsV2(string Title, string article_id)
        {
            Title = StringHelpers.ConvertNewsUrlToNoSymbol(CheckMaxLength(Title.Trim(), 100));
            return ("/" + Title + "-" + article_id);
        }

        // xử lý chuỗi quá dài
        //str: Chuoi truyen vao
        // So ky tu toi da cho phep
        // OUPUT: Tra ra chuoi sau khi xu ly
        public static string CheckMaxLength(string str, int MaxLength)
        {
            try
            {
                //str = RemoveSpecialCharacters(str);
                if (str.Length > MaxLength)
                {

                    str = str.Substring(0, MaxLength + 1); // cat chuoi
                    if (str != " ") //  ky tu sau truoc khi cat co chua ky tu ko
                    {
                        while (str.Last().ToString() != " ") // cat not cac cu tu chu cho den dau cach gan nhat
                        {
                            str = str.Substring(0, str.Length - 1); // dich trai
                        }
                    }
                    //str = str + "...";
                }
                return str;
            }
            catch (Exception ex)
            {
                // Utilities.Common.WriteLog(Models.Contants.FOLDER_LOG, "ERROR CheckMaxLength : " + ex.Message);
                return string.Empty;
            }
        }

        public static string RemoveSpecialCharacters(string input)
        {
            try
            {
                Regex r = new Regex("(?:[^a-z0-9 ]|(?<=['\"])s)", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);
                return r.Replace(input, String.Empty);
            }
            catch (Exception e)
            {
                return input ?? string.Empty;
            }
        }

       

        public static string convertImageToText(string link_static_image)
        {
            try
            {
                string result = string.Empty;
                string img_path = Environment.CurrentDirectory;
                string tessPath = Environment.CurrentDirectory + "\\tessdata";

                //LogHelper.InsertLogTelegram("convertImageToText = tessPath = " + tessPath);

                var last_image = imageConfig.analyticsImgCaptcha(link_static_image);
                if (last_image != null)
                {
                    Uri uri = new Uri(link_static_image);
                    string fileName = Path.GetFileNameWithoutExtension(uri.AbsolutePath).ToString() + ".jpg";
                    string full_link = img_path + "\\" + fileName;




                    last_image.Save(full_link, System.Drawing.Imaging.ImageFormat.Png);

                    using (var engine = new TesseractEngine(tessPath, "eng", EngineMode.Default))
                    {
                        //using (var page = engine.Process(last_image))
                        //{
                        //    result = page.GetText();
                        //}
                        using (var img = Pix.LoadFromFile(full_link)) // Load of the image file from the Pix object which is a wrapper for Leptonica PIX structure
                        {
                            using (var page = engine.Process(img)) //process the specified image
                            {
                                result = page.GetText(); //Gets the image's content as plain text.                            

                                //  Console.ReadKey();
                                LogHelper.InsertLogTelegram( "convertImageToText Success = result = " + result);
                            }
                        }
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("convertImageToText  =link_static_image=" + link_static_image + ex.ToString());
                return string.Empty;
            }
        }

        public static string ConvertAllNonCharacterToSpace(string text)
        {
            string rs = Regex.Replace(text, @"\s+", " ", RegexOptions.Singleline);
            return rs.Trim();
        }
        /// <summary>
        /// Attempts to match the supplied pattern to the input
        /// string. Only obtains a single match and returns the
        /// matching string if successful and an empty string if not.
        /// </summary>
        /// <param name="inputString">String to be searched</param>
        /// <param name="regExPattern">Pattern to be matched</param>
        /// <returns>String match or empty string if match not found</returns>
        public static string GetSingleRegExMatch(string inputString, string regExPattern)
        {
            string msg;
            string result = "";
            try
            {
                Match match = Regex.Match(inputString,
                    regExPattern,
                    RegexOptions.Singleline);
                if (match.Success)
                {
                    result = match.Value;
                }
            }
            catch (ArgumentException ex)
            {
                msg = regExPattern;
                //Debug.WriteLine(ex.InnerException +     " argument exception for pattern " + msg);
                LogHelper.InsertLogTelegram(ex.InnerException + " argument exception for pattern " + msg);
            }

            return result;
        }
        public static string RemoveAllNonCharacter(string text)
        {
            string rs = Regex.Replace(text, @"\s+", "", RegexOptions.Singleline);
            return rs.Trim();
        }
        public static string RemoveUnusedTags(this string source)
        {
            return Regex.Replace(source, @"<(\w+)\b(?:\s+[\w\-.:]+(?:\s*=\s*(?:""[^""]*""|'[^']*'|[\w\-.:]+))?)*\s*/?>\s*</\1\s*>", string.Empty, RegexOptions.Multiline);
        }
        public static T ConvertFromJsonString<T>(string jsonString)
        {
            try
            {
                T rs = JsonConvert.DeserializeObject<T>(jsonString);
                return rs;
            }
            catch
            {
                return default(T);
            }

        }
        public static string DecodeHTML(string html)
        {
            string result = "";
            try
            {
                result = HttpUtility.HtmlDecode(html);
            }
            catch
            {
                string msg = "Unable to decode HTML: " + html;
                throw new ArgumentException(msg);
            }

            return result;
        }
        public static string RemoveSpecialCharacterExceptVietnameseCharacter(string text)
        {
            string pattern = "/[^a-zA-Z0-9àáãảạăằắẳẵặâầấẩẫậèéẻẽẹêềếểễệđùúủũụưừứửữựòóỏõọôồốổỗộơờớởỡợìíỉĩịäëïîöüûñçýỳỹỵỷ ]/g";
            return Regex.Replace(text, pattern, "");
        }
        public static string RemoveAllSpecialCharacterinURL(string text)
        {
            string pattern = "/[^a-zA-Z0-9àáãảạăằắẳẵặâầấẩẫậèéẻẽẹêềếểễệđùúủũụưừứửữựòóỏõọôồốổỗộơờớởỡợìíỉĩịäëïîöüûñçýỳỹỵỷ/.-_: ]/g";
            return Regex.Replace(text, pattern, "");
        }
        public static string RemoveAllSpecialCharacterLogin(string text)
        {
            string pattern = "/[^a-zA-Z0-9.-_+/= ]/g";
            return Regex.Replace(text, pattern, "");
        }
        public static string genLinkDetailProduct(string label_name, string product_code, string product_name)
        {
            product_name = CommonHelper.RemoveSpecialCharacters(product_name);
            product_name = RemoveUnicode(CheckMaxLength(product_name.Trim(), 50));
            product_name = product_name.Replace(" ", "-").Replace("/", "");
            return ("/product/" + label_name + "/" + product_name + "-").ToLower() + product_code + ".html";
        }
        public static string genLinkDetailProductOtherLabel(string label_name, string path, bool is_extension = false)
        {
            path = path.Replace(".html?", "-variant-");
            path = path.Replace(".html", "");
            path = path.Replace("=", "__");
            string url = ("/product/" + label_name + "/" + path).ToLower() + ".html";
            if (is_extension)
            {
                url += "?product_source=3";
            }
            return url;
        }
        public static double convertToPound(double value, string unit)
        {
            try
            {
                double rs = 0;
                switch (unit)
                {
                    case "ounces":
                    case "oz":
                        rs = value * 0.0625;
                        break;
                    case "grams":
                    case "g":
                        rs = value * 0.0022046;
                        break;
                    case "kilograms":
                        rs = value * 2.2046;
                        break;
                    case "tonne":
                        rs = value * 2204.62262;
                        break;
                    case "kiloton":
                        rs = value * 2204622.6218;
                        break;
                    case "pounds":
                        rs = value;
                        break;
                    default:
                        rs = value;
                        break;
                }
                return rs;
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("convertToPound: value = " + value + " error:" + ex.ToString());
                return 1; // Nếu k có đơn vị nào thỏa mãn sẽ coi như là k có cân nặng và báo mail về cho cskh
            }

        }
        public static string GetCachePartFromURL(string url, int label_id = (int)LabelType.jomashop)
        {
            var cachepart = "";
            if (url == null || url.Trim() == "")
            {
                return cachepart;
            }
            else if (url.Contains("amazon.com"))
            {
                var asin = "";
                CommonHelper.CheckAsinByLink(url, out asin);
                return asin;
            }
            try
            {   // https://www.jomashop.com/golden-goose-stardan-low-top-sneakers-in-leather-gwf00128-f000567.html?product_id=774221
                var product_path = url.Split("/");
                var plant_text = product_path[product_path.Length - 1];
                cachepart = CommonHelper.Encode(plant_text, label_id.ToString());
            }
            catch { }
            return cachepart;
        }
        public static bool CheckAsinByLink(string Link, out string ASIN)
        {
            ASIN = Link;
            try
            {
                // regex lấy ra domain theo link
                Link = Link.Replace("http://", "https://").Replace("%", "");
                var uri = new Uri(Link);
                string sDomainLite = uri.Host;

                // regex lay ra link ID sản phẩm
                //M1: "https://www.amazon.com/gp/aw/d/B07GB4X6T7/ref=ox_sc_act_image_1?smid=AY8DYQ3EFA9NJ&psc=1"
                //M2: https://www.amazon.com/d/Eye-Creams/Hada-Labo-Tokyo-Correcting-Cream/B00OFTIP86/ref=sr_1_2_a_it?ie=UTF8&qid=1542617568&sr=8-2-spons&keywords=Hada+Labo+Tokyo&psc=1#customerReviews


                var match = Regex.Match(Link, "https://" + sDomainLite + "/([\\w-]+/)?(dp|gp/product|gp/aw/d)/(\\w+/)?(\\w{10})", RegexOptions.Singleline); //spelling error
                var url = match.Groups[0].Value;

                //Detect Truong hop 2
                if (url == string.Empty)
                {
                    match = Regex.Match(Link, "(?:[/dp/]|$)([A-Z0-9]{10})", RegexOptions.Singleline); //spelling error
                    url = match.Groups[0].Value;
                }

                // Lấy ra ASIN trên link
                ASIN = url == "" ? "" : url.Split('/').Last();


                return true;
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("CheckAsinAmz - CommonHelper: Link = " + Link + " error:" + ex.ToString());
                return false;
            }
        }

    }
}
