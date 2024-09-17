using Entities.ViewModels.Attachment;
using Microsoft.AspNetCore.Mvc;
using Repositories.IRepositories;
using System.Security.Claims;
using Utilities.Contants;
using Utilities;
using WEB.CMS.Models;

namespace WEB.CMS.Controllers
{
    public class AttachFileController : Controller
    {
        private readonly IAttachFileRepository _AttachFileRepository;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        private readonly Models.AppSettings config;
        public AttachFileController(IAttachFileRepository attachFileRepository, IWebHostEnvironment hostEnvironment)
        {
            _AttachFileRepository = attachFileRepository;
            _WebHostEnvironment = hostEnvironment;
            config = ReadFile.LoadConfig();
        }
        public async Task<IActionResult> Widget(string id, long DataId, int Type, AttachmentsOption option)
        {
            var _UserLogin = 0;
            if (HttpContext.User.FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                _UserLogin = int.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            }
            var models = await _AttachFileRepository.GetListByType(DataId, Type);
            ViewBag.DataId = DataId;
            ViewBag.Type = Type;
            ViewBag.UserId = _UserLogin;
            ViewBag.Data = models;
            ViewBag.ImageExtension = new List<string>() { "png", "jpg", "gif", "jpeg", "PNG", "JPG", "GIF", "JPEG" };
            ViewBag.VideoExtension = new List<string>() { "mp4", "vod", "mkv", "avi", "MP4", "VOD", "MKV", "AVI" };
            ViewBag.Option = option;
            ViewBag.ID = id;
            return PartialView();
        }
        public async Task<IActionResult> UploadFile(IFormFile[] files)
        {
            try
            {
                var _UserLogin = 0;
                if (HttpContext.User.FindFirst(ClaimTypes.NameIdentifier) != null)
                {
                    _UserLogin = int.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                }
                List<string> urls = new List<string>();
                if (files != null && files.Length > 0)
                {
                    foreach (var file in files)
                    {
                        string _FileName = file.FileName;
                        string _UploadFolder = @"uploads/images/" + _UserLogin;
                        string _UploadDirectory = Path.Combine(_WebHostEnvironment.WebRootPath, _UploadFolder);

                        if (!Directory.Exists(_UploadDirectory))
                        {
                            Directory.CreateDirectory(_UploadDirectory);
                        }
                        string filePath = Path.Combine(_UploadDirectory, _FileName);
                        if (!System.IO.File.Exists(filePath))
                        {
                            using (var fileStream = new FileStream(filePath, FileMode.Create))
                            {
                                await file.CopyToAsync(fileStream);
                            }
                        }
                        urls.Add("/" + _UploadFolder + "/" + _FileName);
                    }
                }

                if (urls != null && urls.Count > 0)
                {
                    return new JsonResult(new
                    {
                        status = (int)ResponseType.SUCCESS,
                        msg = "Thành công",
                        data = urls
                    });
                }
                else
                {
                    return new JsonResult(new
                    {
                        status = (int)ResponseType.FAILED,
                        msg = "Tải tệp đính kèm thất bại",
                        data = urls
                    });
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("UploadFile - AttachFileViewComponent" + ex.ToString());
            }
            return new JsonResult(new
            {
                status = (int)ResponseType.FAILED,
                msg = "Lỗi trong quá trình tải lên tệp đính kèm, vui lòng liên hệ IT.",
            });
        }
        public async Task<IActionResult> ConfirmFileUpload(List<AttachfileViewModel> files, long data_id, int service_type)
        {
            try
            {
                var key = MFAService.Get_AESKey(MFAService.ConvertBase64StringToByte(ReadFile.LoadConfig().AES_KEY));
                var iv = MFAService.Get_AESIV(MFAService.ConvertBase64StringToByte(ReadFile.LoadConfig().AES_IV));


                var _UserLogin = 0;
                if (HttpContext.User.FindFirst(ClaimTypes.NameIdentifier) != null)
                {
                    _UserLogin = int.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                }
                if (files != null && files.Count > 0)
                {
                    string url_post = config.IMAGE_DOMAIN + config.API_STATIC_UPLOADFILE;
                    var _httpClient = new HttpClient();
                    foreach (var file in files)
                    {
                        string full_path = Directory.GetCurrentDirectory() + "\\wwwroot\\" + file.path.Replace("/", "\\");
                        try
                        {
                            var path = file.path.Split("/");
                            var file_name = path[path.Length - 1];
                            var encrypt = MFAService.AES_EncryptToByte(DateTime.Now.ToString(), key, iv);
                            var token = MFAService.ConvertByteToBase64String(encrypt);
                            //Bind your file location
                            var readFileData = System.IO.File.ReadAllBytes(full_path);
                            //Create Multipart Request
                            var formContent = new MultipartFormDataContent();

                            ByteArrayContent bytes = new ByteArrayContent(readFileData);
                            MultipartFormDataContent multiContent = new MultipartFormDataContent();
                            formContent.Add(bytes, "data", path[path.Length - 1]);

                            formContent.Add(new StringContent(path[path.Length - 1]), "name");
                            formContent.Add(new StringContent(data_id.ToString()), "data_id");
                            formContent.Add(new StringContent(service_type.ToString()), "type");
                            formContent.Add(new StringContent(token), "token");
                            var response = await _httpClient.PostAsync(url_post, formContent);
                            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                var content = Newtonsoft.Json.Linq.JObject.Parse(response.Content.ReadAsStringAsync().Result);
                                if (content["status"] != null && content["status"].ToString() != null && content["status"].ToString().Trim() != ""
                                    && content["url"] != null && content["url"].ToString() != null && content["url"].ToString().Trim() != "")
                                {
                                    file.path = config.IMAGE_DOMAIN + content["url"].ToString();
                                    try
                                    {
                                        System.IO.File.Delete(full_path);
                                    }
                                    catch { }
                                }

                            }

                        }
                        catch
                        {

                        }
                        var ext_split = file.path.Split(".");
                        file.ext = ext_split[^1];
                    }
                    await _AttachFileRepository.SaveAttachFileURL(files, data_id, _UserLogin, service_type);
                    await _AttachFileRepository.DeleteNonExistsAttachFile(files.Select(x => x.id).ToList(), data_id, service_type);
                    return Ok(new
                    {
                        status = (int)ResponseType.SUCCESS,
                        msg = "Thành công"
                    });
                }
                else
                {
                    await _AttachFileRepository.DeleteNonExistsAttachFile(new List<long>(), data_id, service_type);
                    return Ok(new
                    {
                        status = (int)ResponseType.SUCCESS,
                        msg = "Thành công"
                    });
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("ConfirmFileUpload - AttachFileController" + ex.ToString());
                return Ok(new
                {
                    status = (int)ResponseType.FAILED,
                    msg = "Lỗi trong quá trình xử lý vui lòng liên hệ IT"
                });
            }
        }
    }
}
