using Entities.ViewModels.AccountAccessAPI;
using Entities.ViewModels.AccountAccessApiPermission;
using Microsoft.AspNetCore.Mvc;
using Repositories.IRepositories;
using Utilities;
using WEB.CMS.Customize;

namespace WEB.CMS.Controllers
{
    [CustomAuthorize]
    public class AccountAccessAPIPermissionController : Controller
    {
        IAccountAccessApiPermissionRepository _accountAccessApiPermissionRepository;
        public AccountAccessAPIPermissionController(IAccountAccessApiPermissionRepository accountAccessApiPermissionRepository)
        {
            _accountAccessApiPermissionRepository = accountAccessApiPermissionRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> InsertAccountAccessAPIPermission(AAAPSubmitModel model)
        {
            try
            {
                var rs = await _accountAccessApiPermissionRepository.InsertAccountAccessApiPermission(model);
                if (rs > 0)
                {
                    return new JsonResult(new
                    {
                        isSuccess = true,
                        message = "Thêm mới thành công"
                    });
                }
                else
                {
                    return new JsonResult(new
                    {
                        isSuccess = false,
                        message = "Thêm mới thất bại"
                    });
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("Insert - AccountAccessAPIPermissionController: " + ex);
                return new JsonResult(new
                {
                    isSuccess = false,
                    message = "Thêm mới thất bại"
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateAccountAccessAPIPermission(AAAPSubmitModel model)
        {
            try
            {
                var rs = await _accountAccessApiPermissionRepository.UpdateAccountAccessApiPermission(model);
                if (rs > 0)
                {
                    return new JsonResult(new
                    {
                        isSuccess = true,
                        message = "Cập nhật thành công"
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
                LogHelper.InsertLogTelegram("Insert - AccountAccessAPIPermissionController: " + ex);
                return new JsonResult(new
                {
                    isSuccess = false,
                    message = "Cập nhật thất bại"
                });
            }
        }
    }


}
