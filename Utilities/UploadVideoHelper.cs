using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Utilities.ViewModels.Article;

namespace Utilities
{
    public class UploadVideoHelper
    {
        static string apiUploadVideo = "https://static-image.adavigo.com/Video/upload-video";
        static string key_token_api = "wVALy5t0tXEgId5yMDNg06OwqpElC9I0sxTtri4JAlXluGipo6kKhv2LoeGQnfnyQlC07veTxb7zVqDVKwLXzS7Ngjh1V3SxWz69";

        //UploadVideoBase64
        public static async Task<string> UploadVideoBase64(VideoBase64 modelVideo)
        {
            string ImagePath = string.Empty;
            string tokenData = string.Empty;
            try
            {

                var j_param = new Dictionary<string, string> {
                    { "data_file", modelVideo.VideoData },
                    { "extend", modelVideo.VideoExtension }};

                using (HttpClient httpClient = new HttpClient())
                {
                    tokenData = CommonHelper.Encode(JsonConvert.SerializeObject(j_param), key_token_api);
                    var contentObj = new { token = tokenData };
                    var content = new StringContent(JsonConvert.SerializeObject(contentObj), Encoding.UTF8, "application/json");
                    var result = await httpClient.PostAsync(apiUploadVideo, content);
                    dynamic resultContent = Newtonsoft.Json.Linq.JObject.Parse(result.Content.ReadAsStringAsync().Result);
                    if (resultContent.status == "success")
                    {
                        return resultContent.url_path;
                    }
                    else
                    {
                        LogHelper.InsertLogTelegram("UploadImageBase64. Result: " + resultContent.status + ". Message: " + resultContent.msg);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("UploadImageBase64 - " + ex.Message.ToString() + " Token:" + tokenData);
            }
            return ImagePath;
        }
        public static async Task<string> UploadVideo64Src(string ImageSrc, string StaticDomain)
        {
            try
            {
                var objvideo = StringVideoHelpers.GetVideoSrcBase64Object(ImageSrc);
                if (objvideo != null)
                {
                    objvideo.VideoData = ResizeBase64video(objvideo.VideoData, out string FileType);
                    if (!string.IsNullOrEmpty(FileType)) objvideo.VideoExtension = FileType;

                    return await UploadVideoBase64(objvideo);
                }
                else
                {
                    if (ImageSrc.StartsWith(StaticDomain))
                        return ImageSrc.Replace(StaticDomain, string.Empty);
                    else
                        return ImageSrc;
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("UploadVideo64Src - " + ex.Message.ToString());
            }
            return string.Empty;
        }

        public static string ResizeBase64video(string ImageBase64, out string FileType)
        {
            FileType = null;
            try
            {
                var IsValid = StringHelpers.TryGetFromBase64String(ImageBase64, out byte[] ImageByte);
                if (IsValid)
                {
                    using (var memoryStream = new MemoryStream(ImageByte))
                    {
                        FileType = "mp4";
                        return Convert.ToBase64String(ImageByte);

                    }
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }
    }
}
