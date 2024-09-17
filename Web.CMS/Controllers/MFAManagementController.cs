using Entities.Models;
using Entities.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using OtpNet;
using Repositories.IRepositories;
using System;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Utilities;
using Utilities.Common;
using Utilities.Contants;
using WEB.CMS.Customize;

namespace WEB.CMS.Controllers
{
    [CustomAuthorize]
    public class MFAManagementController : Controller
    {
        private readonly IUserRepository _UserRepository;
        private readonly IMFARepository _mFARepository;
        private readonly IConfiguration _configuration;

        private string _UserName;
        private int _UserId;
        // MFAManagement
        public MFAManagementController(IConfiguration Configuration, IUserRepository userRepository, IMFARepository mFARepository)
        {
            _UserRepository = userRepository;
            _mFARepository = mFARepository;
            _configuration = Configuration;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                _UserName = HttpContext.User.FindFirst(ClaimTypes.Name).Value;
                _UserId = Convert.ToInt32(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                // Lấy thông tin 2FA
                var mfa_detail = await _mFARepository.get_MFA_DetailByUserID(_UserId);
                if (mfa_detail != null)
                {
                    ViewBag.QRCodeUri = await GenerateQRCodeAsync();
                    ViewBag.SecretKey = FormatKey(mfa_detail.SecretKey);
                    ViewBag.Username = mfa_detail.Username;
                    ViewBag.Status = mfa_detail.Status;
                    return View();
                }
                else
                {
                    ViewBag.QRCodeUri = "";
                    ViewBag.SecretKey = "";
                    ViewBag.Username = _UserName;
                    ViewBag.Status = 0;
                    return View();
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("Index - MFAManagementController: " + ex);
                return RedirectToAction("Login", "Account");
            }
        }

        [HttpPost]
        public async Task<ActionResult> OTPTest(MFAViewModel record) 
        {
            try
            {
                if (HttpContext.User.FindFirst(ClaimTypes.Name) != null)
                {
                    //Authenticator post data:
                    _UserId = Convert.ToInt32(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                    var mfa_record = await _mFARepository.get_MFA_DetailByUserID(_UserId);
                    switch (record.MFA_type)
                    {
                        case 0:
                            {
                                bool compare_value = false;
                                // Lấy thông tin MFA
                                // Convert sang byte
                                var bytes = Base32OTPEncoding.ToBytes(mfa_record.SecretKey.Trim());
                                // Xây mã OTP
                                var totp = new Totp(bytes);
                                var otp_code = totp.ComputeTotp();
                                //var remainingTime = totp.RemainingSeconds();
                                //Kiểm tra OTP tạo ra với OTP gửi lên và thời gian
                                if (record.MFA_Code == otp_code)
                                {
                                    compare_value = true;
                                }
                                else
                                {
                                    compare_value = false;
                                }
                                // var compare_value = await CompareOTP(_UserId, record.MFA_Code, record.MFA_timenow);
                                if (compare_value)
                                {
                                    return new JsonResult(new
                                    {
                                        status = ResponseType.SUCCESS.ToString(),
                                        msg = "Xác thực thành công đối với mã OTP đã nhập."
                                    });
                                }
                                else
                                {
                                    return new JsonResult(new
                                    {
                                        status = ResponseType.FAILED.ToString(),
                                        msg = "Xác thực không thành công, vui lòng kiểm tra lại cài đặt."
                                    });
                                }
                            }
                        case 1:
                            {
                                string backupcode_input = BackupCodeMD5FromInput(record.MFA_Code, mfa_record);
                                if (backupcode_input == mfa_record.BackupCode.Trim())
                                {
                                    return new JsonResult(new
                                    {
                                        status = ResponseType.SUCCESS.ToString(),
                                        msg = "Xác thực thành công đối với mã dự phòng đã nhập."
                                    });
                                }
                                else
                                {
                                    return new JsonResult(new
                                    {
                                        status = ResponseType.FAILED.ToString(),
                                        msg = "Xác thực không thành công, vui lòng kiểm tra lại mã."
                                    });
                                }
                            }
                        default:
                            {
                                return new JsonResult(new
                                {
                                    status = ResponseType.FAILED.ToString(),
                                    msg = "OTP nhập lên không thuộc phương thức hợp lệ, vui lòng thử lại"
                                });
                            }
                    }
                }
                else
                {
                    return new JsonResult(new
                    {
                        status = ResponseType.FAILED.ToString(),
                        msg = "Page cannot access by this way, please try again."
                    });
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("OTPTest - MFAManagementController: " + ex);
                return new JsonResult(new
                {
                    status = ResponseType.FAILED.ToString(),
                    msg = "Lỗi trong quá trình xử lý, vui lòng liên hệ với bộ phận kỹ thuật."
                });
            }
        }
        private async Task<bool> CompareOTP(int user_id, string otp_code_input, DateTime time_now)
        {
            try
            {
                // Lấy thông tin MFA
                var mfa_record = await _mFARepository.get_MFA_DetailByUserID(user_id);
                // Convert sang byte
                var bytes = Base32OTPEncoding.ToBytes(mfa_record.SecretKey.Trim());
                // Xây mã OTP
                var totp = new Totp(bytes);
                var otp_code = totp.ComputeTotp();
                //var remainingTime = totp.RemainingSeconds();
                //Kiểm tra OTP tạo ra với OTP gửi lên và thời gian
                if (otp_code_input == otp_code)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("CompareOTP - MFAManagementController: " + ex);
                return false;
            }

        }
        private string BackupCodeMD5FromInput(string input, Mfauser mfa_record)
        {
            try
            {
                MD5 md5_generator = MD5.Create();
                string hash_str = input.Trim() + "_" + mfa_record.UserId.ToString().Trim() + "_" + mfa_record.Username.ToString().Trim() + "_" + mfa_record.UserCreatedYear.ToString().Trim();
                byte[] hash_byte = System.Text.Encoding.ASCII.GetBytes(hash_str);
                string backupcode_input = Base32Encoding.ToString(md5_generator.ComputeHash(hash_byte));
                return backupcode_input.Trim();
            } catch(Exception)
            {
                return null;
            }
        }
        private async Task<string> GenerateQRCodeAsync() //int user_id
        {
            try
            {
                var result = await _mFARepository.get_MFA_DetailByUserID(_UserId);
                if (result != null)
                {
                    string label_name = "AdavigoCMS-" + result.Username.Trim();
                    string secret_key = result.SecretKey.Trim();
                    string issuer = "Adavigo CMS";
                    string otp_auth_url = @"" + "otpauth://totp/" + issuer + ":" + label_name + "?secret=" + secret_key + "&issuer=" + issuer + "";
                    return otp_auth_url;
                }
                return null;

            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GenerateQRCodeAsync - MFAManagementController: " + ex);
                return null;
            }
        }
        private async Task<string> GenerateSecretKeyAsync(int user_id)
        {
            try
            {
                SHA256 sHA256 = SHA256.Create();
                var client_detail = await _UserRepository.GetDetailUser(user_id);
                /*Secret Key Generate*/
                string SecretKey = "";
                string random_int_begin = new Random().Next(0, 99999999).ToString(new string('0', 8));
                string random_int_last = new Random().Next(0, 99999999).ToString(new string('0', 8));
                // 12345678_55_minh.nq_11111111_minhnguyen@Adavigo.vn
                string base_text = random_int_begin.Trim()+"_"+ client_detail.Entity.Id + "_" + client_detail.Entity.UserName.Trim()+"_" + random_int_last.Trim()+ "_" + client_detail.Entity.Email.Trim();
                byte[] base_text_in_bytes = System.Text.Encoding.ASCII.GetBytes(base_text);
                byte[] hash_text_sha256 = sHA256.ComputeHash(base_text_in_bytes);
                SecretKey = Base32OTPEncoding.ToString(hash_text_sha256);
                //Get 32 first char from base32 string, as google authenticator secret key length
                SecretKey = SecretKey.Substring(0, 32).Trim();
                return SecretKey.Trim();
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GenerateSecretKeyAsync - MFAManagementController: " + ex);
                return null;
            }
        }
        
        private string FormatKey(string unformattedKey)
        {
            try
            {
                return Regex.Replace(unformattedKey.Trim(), ".{4}", "$0 ");
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
