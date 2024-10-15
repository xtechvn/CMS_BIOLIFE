using Caching.RedisWorker;
using Entities;
using Entities.ViewModels;
using ENTITIES.ViewModels.Notify;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Utilities;
using Utilities.Contants;
using WEB.Adavigo.CMS.Service;
using WEB.CMS.Customize;

namespace WEB.Adavigo.CMS.Controllers.Configs
{
    [CustomAuthorize]

    public class MenuController : Controller
    {
        private readonly IMenuRepository _MenuRepository;
        private readonly IConfiguration _configuration;
        private readonly RedisConn _redisService;
        public MenuController(IConfiguration configuration, IUserRepository userRepository, IMenuRepository menuRepository, RedisConn redisService)
        {
            _configuration = configuration;
            _MenuRepository = menuRepository;
            _redisService = redisService;
            _redisService.Connect();

        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Search(string name, string link)
        {
            var menus = await _MenuRepository.GetAll(name, link);
            return View(menus);
        }
        [HttpPost]
        [HttpGet]
        public async Task<IActionResult> AddOrUpdate(int id, int parent_id)
        {
            var model = new MenuUpsertViewModel
            {
                ParentId = parent_id,
                Status = 0
            };

            if (id > 0)
            {
                var menu = await _MenuRepository.GetById(id);
                model = new MenuUpsertViewModel
                {
                    Id = id,
                    ParentId = menu.ParentId,
                    MenuCode = menu.MenuCode,
                    Name = menu.Name,
                    Link = menu.Link,
                    Title = menu.Title,
                    Icon = menu.Icon,
                    Status = menu.Status
                };
            }
            return View(model);
        }


        public async Task<IActionResult> Permission(int id)
        {
            var model = new MenuPermissionModel
            {
                menu_id = id,
                permission_ids = new List<int>()
            };

            var permission_list = await _MenuRepository.GetSelectedPermissionList(id);
            if (permission_list != null && permission_list.Any())
            {
                model.permission_ids = permission_list;
            }

            ViewBag.Permissions = await _MenuRepository.GetListPermission();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> SavePermission(MenuPermissionModel model)
        {
            try
            {
                long result = await _MenuRepository.SavePermission(model);

                if (result > 0)
                {
                    return new JsonResult(new
                    {
                        isSuccess = true,
                        message = $"Cập nhật quyền cho menu thành công"
                    });
                }
                else
                {
                    return new JsonResult(new
                    {
                        isSuccess = false,
                        message = $"Cập nhật quyền cho menu thất bại"
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
        public async Task<IActionResult> Summit(MenuUpsertViewModel request)
        {
            try
            {
                long result = 0;
                string ActionName = string.Empty;
                if (request.Id > 0)
                {
                    ActionName = "Cập nhật";
                    result = await _MenuRepository.Update(request);
                }
                else
                {
                    ActionName = "Thêm mới";
                    result = await _MenuRepository.Create(request);
                }

                if (result > 0)
                {
                    return new JsonResult(new
                    {
                        isSuccess = true,
                        message = $"{ActionName} menu thành công"
                    });
                }
                else
                {
                    return new JsonResult(new
                    {
                        isSuccess = false,
                        message = $"{ActionName} menu thất bại"
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

        public async Task<IActionResult> ChangeStatus(int Id, int Status)
        {
            try
            {
                var result = await _MenuRepository.ChangeStatus(Id, Status);

                if (result > 0)
                {
                    return new JsonResult(new
                    {
                        isSuccess = true,
                        message = "Ẩn Menu thành công"
                    });
                }
                else
                {
                    return new JsonResult(new
                    {
                        isSuccess = false,
                        message = "Ẩn Menu thất bại"
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
        public async Task<IActionResult> Notify()
        {
            try
            {
                long _UserId = 0;

                if (HttpContext.User.FindFirst(ClaimTypes.NameIdentifier) != null)
                {
                    _UserId = Convert.ToInt64(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                }
                int db_index = Convert.ToInt32(_configuration["Redis:Database:db_common"]);
              
                var lst_Notify = new NotifySummeryViewModel();
                              
                    //lấy từ api
                    lst_Notify = new NotifySummeryViewModel();
                    return Ok(new
                    {
                        status = (int)ResponseType.SUCCESS,
                        data = lst_Notify != null ? lst_Notify : null
                    });
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("Notify - MenuController: " + ex);

            }
            return Ok(new
            {
                status = (int)ResponseType.ERROR,
                data = new List<NotifySummeryViewModel>()
            });
        }
        [HttpPost]
        public async Task<IActionResult> updateNotify(string id, string seen_status)
        {
            try
            {
                long _UserId = 0;

                if (HttpContext.User.FindFirst(ClaimTypes.NameIdentifier) != null)
                {
                    _UserId = Convert.ToInt64(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                }

                var UpdateNotify = 0;
                if (UpdateNotify == 0)
                    return Ok(new
                    {
                        status = (int)ResponseType.SUCCESS,

                    });

            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("updateNotify - MenuController: " + ex);

            }
            return Ok(new
            {
                status = (int)ResponseType.ERROR,

            });
        }
    }
}
