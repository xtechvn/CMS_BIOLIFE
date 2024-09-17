using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Repositories.IRepositories;
using System.Security.Claims;
using Utilities;
using Utilities.Contants;

namespace WEB.CMS.Controllers.PaymentAccount
{
    public class PaymentAccountController : Controller
    {
        private readonly IPaymentAccountRepository _paymentAccountRepository;
   
        public PaymentAccountController(IPaymentAccountRepository paymentAccountRepository)
        {

            _paymentAccountRepository = paymentAccountRepository;
           
        }
        [HttpPost]
        public IActionResult Setup(BankingAccount DataModel)
        {
            int stt_code = (int)ResponseType.FAILED;
            string msg = "Error On Excution";
            try
            {
                int _UserId = 0;
                if (HttpContext.User.FindFirst(ClaimTypes.NameIdentifier) != null)
                {
                    _UserId = Convert.ToInt32(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                }
                DataModel.CreatedBy = _UserId;
                DataModel.UpdatedBy = _UserId;
                var Result = _paymentAccountRepository.UpsertBankingAccount(DataModel);
                if (Result > 0)
                {
                    stt_code = (int)ResponseType.SUCCESS;
                    msg = "Thêm mới/Cập nhật tài khoản thanh toán thành công";
                }
                else
                {
                    stt_code = (int)ResponseType.FAILED;
                    msg = "Thêm mới/Cập nhật tài khoản thanh toán không thành công";
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("Setup - PaymentAccountController: " + ex);
                stt_code = (int)ResponseType.ERROR;
                msg = "Lỗi kỹ thuật vui lòng liên hệ bộ phận IT";
            }
            return Ok(new
            {
                stt_code = stt_code,
                msg = msg,
            });
        }
        public async Task<IActionResult> Detail(int id, int user_Id)
        {

            try
            {
                ViewBag.ClientId = user_Id;
                if (id != 0)
                {
                    var model = _paymentAccountRepository.GetBankingAccountById(id);
                    return PartialView(model);
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("Detail - PaymentAccountController: " + ex);
            }
            return PartialView();
        }
        [HttpPost]
        public IActionResult deleteById(int id)
        {
            try
            {
                var a = _paymentAccountRepository.Delete(id);
                if (a == 1)
                {
                    return Ok(new
                    {
                        stt_code = (int)ResponseType.SUCCESS,
                        msg = "Xóa thành công",

                    });
                }
                else
                {

                    return Ok(new
                    {
                        stt_code = (int)ResponseType.FAILED,
                        msg = "Xóa không thành công",

                    });
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("deleteById - PaymentAccountController: " + ex);
            }
            return PartialView();
        }
        [HttpPost]
        public IActionResult DeleteBankingAccount(int id)
        {
            try
            {
                var result = _paymentAccountRepository.DeleteBankingAccountById(id);

                if (result > 0)
                {
                    return new JsonResult(new
                    {
                        isSuccess = true,
                        message = "Xóa thông tin thành công"
                    });
                }
                else
                {
                    return new JsonResult(new
                    {
                        isSuccess = false,
                        message = "Xóa thông tin thất bại"
                    });
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("PaymentDelete - SupplierController: " + ex.Message);
                return new JsonResult(new
                {
                    isSuccess = false,
                    message = ex.Message
                });
            }
        }
    }
}
