using Entities.Models;
using Newtonsoft.Json;
using Repositories.IRepositories;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;
using System.Text;
using Utilities;
using Utilities.ViewModels.Article;
using WEB.CMS.Models;

namespace WEB.Adavigo.CMS.Service
{
    public class StaticAPIService
    {
        private string API_IMAGE = "https://static-image.adavigo.com/images/upload";
        private string API_VIDEO = "https://static-image.adavigo.com/Video/upload-video";
        private string STATIC_URL = "https://static-image.adavigo.com";
        private string KEY = "wVALy5t0tXEgId5yMDNg06OwqpElC9I0sxTtri4JAlXluGipo6kKhv2LoeGQnfnyQlC07veTxb7zVqDVKwLXzS7Ngjh1V3SxWz69";
        public StaticAPIService(IConfiguration configuration)
        {
            KEY = configuration["API:UploadImageKey"];
            API_IMAGE = configuration["API:UploadImage"];
            API_VIDEO = configuration["API:UploadVideo"];
            STATIC_URL = configuration["API:StaticURL"];
        }
        public async Task<string> UploadImageBase64(ImageBase64 modelImage)
        {
            try
            {
                var j_param = new Dictionary<string, string> {
                    { "data_file", modelImage.ImageData },
                    { "extend", modelImage.ImageExtension }};
                string tokenData = CommonHelper.Encode(JsonConvert.SerializeObject(j_param), KEY);
                using (HttpClient httpClient = new HttpClient())
                {
                    var contentObj = new { token = tokenData };
                    var content = new StringContent(JsonConvert.SerializeObject(contentObj), Encoding.UTF8, "application/json");
                    var result = await httpClient.PostAsync(API_IMAGE, content);
                    dynamic resultContent = Newtonsoft.Json.Linq.JObject.Parse(result.Content.ReadAsStringAsync().Result);
                    if (resultContent.status == 0)
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
                LogHelper.InsertLogTelegram("UploadImageBase64 - " + ex.Message.ToString());
            }
            return null;
        }
        public async Task<string> UploadVideoBase64(ImageBase64 modelVideo)
        {
            try
            {
                var j_param = new Dictionary<string, string> {
                    { "data_file", modelVideo.ImageData },
                    { "extend", modelVideo.ImageExtension }};
                string tokenData = CommonHelper.Encode(JsonConvert.SerializeObject(j_param), KEY);
                using (HttpClient httpClient = new HttpClient())
                {
                    var contentObj = new { token = tokenData };
                    var content = new StringContent(JsonConvert.SerializeObject(contentObj), Encoding.UTF8, "application/json");
                    var result = await httpClient.PostAsync(API_VIDEO, content);
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
                LogHelper.InsertLogTelegram("UploadImageBase64 - " + ex.Message.ToString());
            }
            return null;
        }
        public ImageBase64 GetImageSrcBase64Object(string imgSrc)
        {
            try
            {
                if (!string.IsNullOrEmpty(imgSrc) && imgSrc.StartsWith("data:image"))
                {
                    string img_edited = imgSrc;
                   
                    var jpegEncoder = new JpegEncoder { Quality = 100 };
                    try
                    {
                        using (var image = SixLabors.ImageSharp.Image.Load(imgSrc))
                        {
                            var proportion = (double)imgSrc.Length / 1024;
                            var maxWidth = (int)(image.Width / proportion);
                            var maxHeight = (int)(image.Height / proportion);
                            image.Mutate(x => x
                                .Resize(new ResizeOptions
                                {
                                    Mode = ResizeMode.Max,
                                    Size = new Size(maxWidth, maxHeight)
                                }));
                            // Save the Image
                            img_edited = image.ToBase64String(image.Metadata.DecodedImageFormat);
                        }
                    }
                    catch
                    {

                    }
                    var ImageBase64 = new ImageBase64();
                    var base64Data = img_edited.Split(',')[0];
                    ImageBase64.ImageData = img_edited.Split(',')[1];
                    ImageBase64.ImageExtension = base64Data.Split(';')[0].Split('/')[1];
                    return ImageBase64;
                }
            }
            catch (FormatException)
            {

            }
            return null;
        }
        public ImageBase64 GetVideoSrcBase64Object(string imgSrc)
        {
            try
            {
                if (!string.IsNullOrEmpty(imgSrc) && imgSrc.StartsWith("data:video"))
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
    }
}
