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
  public  class StringVideoHelpers
    {
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

        public static string ConvertVideoToBase64(String url)
        {
            try
            {
                StringBuilder _sb = new StringBuilder();
                _sb.Append("data:video/mp4;base64,");

                Byte[] _byte = GetImage(url);

                _sb.Append(Convert.ToBase64String(_byte, 0, _byte.Length));
                return _sb.ToString();
            }
            catch
            {
                return null;
            }
        }
        public static async Task<string> ReplaceEditorVideo(string html_string, string Video_domain)
        {
            try
            {
                
               
                var VideoApiLink = await UploadVideoHelper.UploadVideo64Src(html_string, Video_domain);
                string regexVideoSrc =  Video_domain + VideoApiLink;
                return regexVideoSrc;
            }
            catch
            {

            }
            return html_string;
        }
        public static VideoBase64 GetVideoSrcBase64Object(string imgSrc)
        {
            try
            {
                if (!string.IsNullOrEmpty(imgSrc) && imgSrc.StartsWith("data:video/mp4"))
                {
                    var VideoBase64 = new VideoBase64();
                    var base64Data = imgSrc.Split(',')[0];
                    VideoBase64.VideoData = imgSrc.Split(',')[1];
                    VideoBase64.VideoExtension = base64Data.Split(';')[0].Split('/')[1];
                    return VideoBase64;
                }
            }
            catch (FormatException)
            {

            }
            return null;
        }
    }
}
