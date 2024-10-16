using Aspose.Cells;
using Caching.RedisWorker;
using Entities.ConfigModels;
using Entities.Models;
using Entities.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Nest;
using Newtonsoft.Json;
using Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities;
using Utilities.Contants;
using WEB.CMS.Customize;
using WEB.CMS.RabitMQ;

namespace WEB.CMS.Controllers
{
    [CustomAuthorize]
    public class GroupProductController : Controller
    {
        private readonly IGroupProductRepository _GroupProductRepository;
        private readonly IPositionRepository _PositionRepository;
        private readonly IAllCodeRepository _AllCodeRepository;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        private readonly string _UrlStaticImage;
        private readonly IConfiguration _configuration;
        private readonly RedisConn _redisService;
        private readonly WorkQueueClient work_queue;

        public GroupProductController(IGroupProductRepository groupProductRepository,
               IWebHostEnvironment hostEnvironment, IPositionRepository positionRepository,
               RedisConn redisService, IAllCodeRepository allCodeRepository, IOptions<DomainConfig> domainConfig, IConfiguration configuration)
        {
            _GroupProductRepository = groupProductRepository;
            _WebHostEnvironment = hostEnvironment;
            _PositionRepository = positionRepository;
            work_queue = new WorkQueueClient(configuration);

            _AllCodeRepository = allCodeRepository;
            _UrlStaticImage = domainConfig.Value.ImageStatic;
            _configuration = configuration;
            _redisService = redisService;
            _redisService.Connect();
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<string> Search(string Name, int Status = -1)
        {
            return await _GroupProductRepository.GetListTreeView(Name, Status);
        }

        /// <summary>
        /// Add Or Update GroupProduct
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type">
        /// 0: Add child
        /// 1: Edit itseft
        /// </param>
        /// <returns></returns>
        public async Task<IActionResult> AddOrUpdate(int id, int type)
        {
            var model = new GroupProductDetailModel();
            try
            {
                if (type == 0)
                {
                    model = new GroupProductDetailModel()
                    {
                        Id = 0,
                        Status = 0,
                        OrderNo = 0,
                        ParentId = id
                    };
                }
                else
                {
                    var entity = await _GroupProductRepository.GetById(id);
                    model = new GroupProductDetailModel()
                    {
                        Id = entity.Id,
                        Name = entity.Name,
                        ImagePath = !string.IsNullOrEmpty(entity.ImagePath) ? _UrlStaticImage + entity.ImagePath : entity.ImagePath,
                        OrderNo = entity.OrderNo,
                        ParentId = entity.ParentId,
                        Status = entity.Status,

                        PositionId = entity.PositionId,
                        Description = entity.Description,
                        IsShowFooter = entity.IsShowFooter,
                        IsShowHeader = entity.IsShowHeader,
                        Code = entity.Code
                    };
                }
                _redisService.clear(CacheName.ARTICLE_B2C_CATEGORY_MENU, Convert.ToInt32(_configuration["Redis:Database:db_common"]));
                // Tạo message để push vào queue
                var j_param = new Dictionary<string, object>
                            {
                                { "store_name", "sp_getGroupProduct" },
                                { "index_es", "es_biolife_sp_get_groupproduct" },
                                {"project_type", Convert.ToInt16(ProjectType.BIOLIFE) },
                                  {"id" , id }
                            };
                var _data_push = JsonConvert.SerializeObject(j_param);
                // Push message vào queue
                var response_queue = work_queue.InsertQueueSimple(_data_push, QueueName.queue_app_push);
            }
            catch
            {

            }

            ViewBag.PositionList = await _PositionRepository.GetAll();
            return View(model);
        }

        /// <summary>
        /// public async Task<IActionResult> UpSert(IFormFile imageFile, string imageSize, GroupProductUpsertModel model)
        /// </summary>
        /// <param name="imageFile"></param>
        /// <param name="imageSize"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<IActionResult> UpSert(GroupProductUpsertModel model)
        {
            try
            {
                var upsertModel = new GroupProduct()
                {
                    Id = model.Id,
                    Name = model.Name,
                    OrderNo = model.OrderNo,
                    ParentId = model.ParentId,
                    Description = model.Description,
                    PositionId = model.PositionId,
                    Status = model.Status,
                    ImagePath = await UpLoadHelper.UploadBase64Src(model.ImageBase64, _UrlStaticImage),
                    IsShowHeader = model.IsShowHeader,
                    IsShowFooter = model.IsShowFooter,
                    ModifiedOn = DateTime.Now,
                    Code = model.Code

                };
                var rs = await _GroupProductRepository.UpSert(upsertModel);
                if (rs > 0)
                {
                    _redisService.clear(CacheName.ARTICLE_B2C_CATEGORY_MENU + rs, Convert.ToInt32(_configuration["Redis:Database:db_common"]));
                    _redisService.clear(CacheName.ARTICLE_B2C_CATEGORY_MENU + upsertModel.ParentId, Convert.ToInt32(_configuration["Redis:Database:db_common"]));

                    // Tạo message để push vào queue
                    var j_param = new Dictionary<string, object>
                            {
                                { "store_name", "sp_getGroupProduct" },
                                { "index_es", "es_biolife_sp_get_groupproduct" },
                                {"project_type", Convert.ToInt16(ProjectType.BIOLIFE) },
                                  {"id" ,  model.Id }
                            };
                    var _data_push = JsonConvert.SerializeObject(j_param);
                    // Push message vào queue
                    var response_queue = work_queue.InsertQueueSimple(_data_push, QueueName.queue_app_push);

                    return new JsonResult(new
                    {
                        isSuccess = true,
                        message = "Cập nhật thành công",
                        modelId = rs,
                    });
                }
                else if (rs == -1)
                {
                    return new JsonResult(new
                    {
                        isSuccess = false,
                        message = "Tồn tại nhóm hàng cùng cấp"
                    });
                }
                else
                {
                    return new JsonResult(new
                    {
                        isSuccess = false,
                        message = "Cập nhật thất bại"
                    });
                }
            }
            catch (Exception ex)
            {
                return new JsonResult(new
                {
                    isSuccess = false,
                    message = ex.Message.ToString()
                });
            }
        }



        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var rootParentId = await _GroupProductRepository.GetRootParentId(id);
                var rs = await _GroupProductRepository.Delete(id);

                if (rs > 0)
                {
                    _redisService.clear(CacheName.ARTICLE_B2C_CATEGORY_MENU + id, Convert.ToInt32(_configuration["Redis:Database:db_common"]));

                    //// Tạo message để push vào queue
                    //var j_param = new Dictionary<string, object>
                    //        {
                    //            { "store_name", "sp_getGroupProduct" },
                    //            { "index_es", "es_biolife_sp_get_groupproduct" },
                    //            {"project_type", Convert.ToInt16(ProjectType.BIOLIFE) },
                    //              {"id" ,  id }
                    //        };
                    //var _data_push = JsonConvert.SerializeObject(j_param);
                    //// Push message vào queue
                    //var response_queue = work_queue.InsertQueueSimple(_data_push, QueueName.queue_app_push);


                    return new JsonResult(new
                    {
                        isSuccess = true,
                        message = "Xóa thành công."
                    });
                }
                else if (rs == -1)
                {
                    return new JsonResult(new
                    {
                        isSuccess = false,
                        message = "Nhóm hàng đang được sử dụng. Bạn không thể xóa."
                    });
                }
                else if (rs == -2)
                {
                    return new JsonResult(new
                    {
                        isSuccess = false,
                        message = "Nhóm hàng đang có cấp con. Bạn không thể xóa."
                    });
                }
                else
                {
                    return new JsonResult(new
                    {
                        isSuccess = false,
                        message = "Xóa thất bại."
                    });
                }
            }
            catch (Exception ex)
            {
                return new JsonResult(new
                {
                    isSuccess = false,
                    message = ex.Message
                });
            }
        }

        [HttpPost]
        [Obsolete]
        public IActionResult UploadExcel(IFormFile fileCrawl)
        {
            try
            {
                var listLink = new List<string>();
                if (fileCrawl == null)
                {
                    return new JsonResult(new
                    {
                        Code = 2,
                        Data = new List<String>(),
                        Message = "Vui lòng chọn file."
                    });
                }
                if (!fileCrawl.FileName.Contains("xlsx") && !fileCrawl.FileName.Contains("xsl"))
                {
                    return new JsonResult(new
                    {
                        Code = 2,
                        Data = new List<String>(),
                        Message = "File không đúng định dạng. Vui lòng nhập định dạng là file excel."
                    });
                }
                if (fileCrawl.Length > 10000000)
                {
                    return new JsonResult(new
                    {
                        Code = 2,
                        Data = new List<String>(),
                        Message = "File bạn tải lên quá 10MB. Vui lòng nhập file có kích thước nhỏ hơn 10MB."
                    });
                }
                Workbook workbook = new Workbook(fileCrawl.OpenReadStream());
                var worksheet = workbook.Worksheets[0];
                var listLinkWrong = new List<string>();//list link khong hop le
                if (worksheet.Cells.Count > 0)
                {
                    //truong hop link trong file khong dung dinh dang
                    var list = worksheet.Cells;
                    for (int i = 1; i < list.Count; i++)
                    {
                        if (list[i].Value == null || string.IsNullOrEmpty(list[i].Value.ToString()))
                        {
                            continue;
                        }
                        listLinkWrong.Add(list[i].Value.ToString());

                    }
                }
                else
                {
                    return new JsonResult(new
                    {
                        Code = 2,
                        Data = listLink,
                        DataLinkWrong = listLinkWrong,
                        Message = "Bạn chưa nhập link vào file excel"
                    });
                }
                return new JsonResult(new
                {
                    Code = 1,
                    Data = listLink,
                    DataLinkWrong = listLinkWrong,
                    Message = "Thành công"
                });
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("UploadExcelAsync: " + ex);
                return new JsonResult(new
                {
                    Code = 0,
                    Data = new List<String>(),
                    DataLinkWrong = new List<String>(),
                    Message = "Lỗi khi gửi file lên server."
                });
            }
        }

        public IActionResult AddCampaign()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> GetAllGroup()
        {
            try
            {
                var listGroup = await _GroupProductRepository.GetAll();
                return new JsonResult(new
                {
                    Data = listGroup
                });
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetAllGroup: " + ex);
                return new JsonResult(new
                {
                    Data = new List<GroupProduct>()
                }); ;
            }
        }

        [HttpPost]
        public IActionResult GetAllPosition()
        {
            try
            {
                var listPosition = _PositionRepository.GetAll();
                return new JsonResult(new
                {
                    Code = 1,
                    Data = listPosition,
                });
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetAllPosition - GroupProductController: " + ex);
                return new JsonResult(new
                {
                    Code = 0,
                    Data = new List<Position>(),
                });
            }
        }

        public IActionResult AddOrUpdatePositionAsync(int id)
        {
            ViewBag.listPosition = _PositionRepository.GetListAll();
            return View();
        }

        [HttpPost]
        public IActionResult AddPositionJson(Position position)
        {
            try
            {
                var postionExists = _PositionRepository.GetByPositionName(position.PositionName);
                if (postionExists != null && postionExists.Result != null)
                {
                    return new JsonResult(new
                    {
                        Code = -2,
                        Message = "Tên kích thước đã tồn tại. Vui lòng nhập tên khác."
                    });
                }
                var rs = _PositionRepository.Create(position);
                if (rs.Result > -1)
                {
                    return new JsonResult(new
                    {
                        Code = 1,
                        Message = "Thêm mới kích thước thành công"
                    });
                }
                else
                {
                    return new JsonResult(new
                    {
                        Code = -1,
                        Message = "Thêm mới kích thước thất bại"
                    });
                }

            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("AddPositionJson - GroupProductController: " + ex);
                return new JsonResult(new
                {
                    Code = 0,
                    Data = new List<String>(),
                    Message = "Lỗi thêm mới kích thước."
                });
            }
        }

        public IActionResult UpdatePosition(int id)
        {
            try
            {
                var model = _PositionRepository.GetById(id);
                return View(model);
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("UpdatePosition - GrouProduct: " + ex);
                return View();
            }
        }


        public async Task<IActionResult> Clearcache(int id, string name)
        {
            try
            {

                _redisService.clear(CacheName.ARTICLE_B2C_CATEGORY_MENU + id, Convert.ToInt32(_configuration["Redis:Database:db_common"]));

                return new JsonResult(new
                {
                    isSuccess = true,
                    message = "Clear cache thành công cho chuyên mục " + name + " có id là " + id + "."
                });

            }
            catch (Exception ex)
            {
                return new JsonResult(new
                {
                    isSuccess = false,
                    message = ex.Message
                });
            }
        }
    }
}