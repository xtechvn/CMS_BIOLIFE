using Microsoft.AspNetCore.Mvc;
using Web.CMS.Controllers.Files.Bussiness;

namespace WEB.CMS.Controllers.Files
{
    public class FilesController : Controller
    {
        private StaticAPIService _staticAPIService;
        private readonly IConfiguration _configuration;

        public FilesController(IConfiguration configuration)
        {
            _staticAPIService = new StaticAPIService(configuration);
            _configuration=configuration;
        }
        public async Task<IActionResult> SummitImages(string data_image)
        {
            try
            {
                if (
                    data_image == null || data_image.Trim() == ""
                    )
                {
                    return Ok(new
                    {
                        is_success = false,

                    });
                }
                var data_img = _staticAPIService.GetImageSrcBase64Object(data_image);
                if (data_img != null)
                {
                    var url = await _staticAPIService.UploadImageBase64(data_img);
                    return Ok(new
                    {
                        is_success = true,
                        data = url
                    });
                }

            }
            catch (Exception ex)
            {

            }
            return Ok(new
            {
                is_success = false,
            });
        }
        [HttpPost]
        public async Task<IActionResult> SummitVideo(IFormFile request)
        {
            try
            {
                string url = await _staticAPIService.UploadVideo(request);
                return Ok(new
                {
                    is_success = (url!=null && url.Trim()!=""),
                    data = url
                });
            }
            catch (Exception ex)
            {

            }
            return Ok(new
            {
                is_success = false,
                data=""
            });
        }
    }
}
