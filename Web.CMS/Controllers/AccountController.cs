using Entities.ConfigModels;
using Entities.Models;
using Entities.ViewModels;
using Entities.ViewModels.Login;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using OtpNet;
using Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Utilities;
using Utilities.Common;
using Utilities.Contants;
using WEB.CMS.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using WEB.Adavigo.CMS.Service;
using Microsoft.AspNetCore.Hosting;
using Caching.RedisWorker;

namespace WEB.CMS.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserRepository _UserRepository;
        private readonly IMFARepository _mFARepository;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        private RedisConn _redisConn;

        public AccountController(IUserRepository userRepository, IMFARepository mFARepository, IConfiguration configuration, IWebHostEnvironment hostEnvironment)
        {
            _UserRepository = userRepository;
            _mFARepository = mFARepository;
            _configuration = configuration;
            _WebHostEnvironment = hostEnvironment;
            _redisConn = new RedisConn(configuration);
            _redisConn.Connect();
        }
        /// <summary>
        /// Function Đăng xuất
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            try
            {
                int _UserId = 0;
                if (HttpContext.User.FindFirst(ClaimTypes.NameIdentifier) != null)
                {
                    _UserId = Convert.ToInt32(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                }
                _redisConn.clear(CacheName.USER_ROLE + _UserId, Convert.ToInt32(_configuration["Redis:Database:db_common"]));
            }
            catch { }
            return Redirect(ReadFile.LoadConfig().LoginURL);

        }
        public IActionResult RedirectLogin()
        {
            try
            {
                HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            }
            catch { }
            if (_configuration["Config:On_QC_Environment"]!=null && _configuration["Config:On_QC_Environment"].Trim()=="1")
            {
                return RedirectToAction("Login", "Account");
            }
            return Redirect(ReadFile.LoadConfig().LoginURL) ;
        }
        /// <summary>
        /// Login with token
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        [Route("login/token/{token_user}")]
        public async Task<IActionResult> LoginWithToken(string token_user)
        {
            try
            {
                JArray objParr = null;
                #region Test:

                //var input_test = new
                //{
                //    user_id = 2139,
                //    user_name = "minh.nq",
                //    email = "minhnguyen@usexpress.vn",
                //    time = DateTime.Now
                //};
                //var data_product = JsonConvert.SerializeObject(input_test);
                //token = CommonHelper.Encode(data_product, _configuration["DataBaseConfig:key_api:api_manual"]);
                //token = token.Replace("//", "_").Replace("+", "-");


                #endregion
                if (CommonHelper.GetParamWithKey(token_user.Replace("_", @"/").Replace("-", "+"), out objParr, _configuration["DataBaseConfig:key_api:api_manual"]))
                {
                    //LogHelper.InsertLogTelegram("CMS Login : login/token/" + token_user + "\n Data:\n " + JsonConvert.SerializeObject(objParr));
                    int _UserId = 0;
                    if (HttpContext.User.FindFirst(ClaimTypes.NameIdentifier) != null)
                    {
                        _UserId = Convert.ToInt32(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                    }
                    int user_id = Convert.ToInt32(objParr[0]["user_id"].ToString());
                    DateTime? token_time  = Convert.ToDateTime(objParr[0]["time"].ToString());
                    string user_name = objParr[0]["user_name"].ToString();
                    string email = objParr[0]["email"].ToString();
                    int token_exprire = ReadFile.LoadConfig().LoginTokenExprire;

                    if (user_id <= 0 || token_time == null || ((DateTime)token_time).AddMinutes(token_exprire) < DateTime.Now)
                    {

                    }
                    else
                    {
                        var user_exists = await _UserRepository.GetById(user_id);
                        var model = await _UserRepository.GetDetailUser(user_id);
                        if (model.Entity == null) model.Entity = new User();
                        model.Entity.Id = user_id;
                        model.Entity.UserName = user_name;
                        model.Entity.Email = email;
                        await CreateCookieAuthenticate(model);
                        return RedirectToAction("Index", "Home");
                       
                    }
                }

            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("CMS Login : login/token/" + token_user + "\nError: " + ex);
            }
            if (_configuration["Config:On_QC_Environment"] != null && _configuration["Config:On_QC_Environment"].Trim() == "1")
            {
                return RedirectToAction("Login", "Account");
            }
            return Redirect(ReadFile.LoadConfig().LoginURL);

        }
        /// <summary>
        /// Login with token
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("login/check-login")]
        public async Task<IActionResult> CheckIfUserLogged([FromForm]string token)
        {
            try
            {
                JArray objParr = null;
                #region Test:

                //var input_test = new
                //{
                //    user_id = 18,

                //};
                //var data_product = JsonConvert.SerializeObject(input_test);
                //token = CommonHelper.Encode(data_product, _configuration["DataBaseConfig:key_api:api_manual"]);
                //token = token.Replace("//", "_").Replace("+", "-");


                #endregion
                token = token.Replace("_", "//").Replace("-", "+");
                if (CommonHelper.GetParamWithKey(token, out objParr, _configuration["DataBaseConfig:key_api:api_manual"]))
                {
                    int user_id = user_id = Convert.ToInt32(objParr[0]["user_id"].ToString());
                    int _UserId = 0;

                    if (HttpContext.User.FindFirst(ClaimTypes.NameIdentifier) != null)
                    {
                        _UserId = Convert.ToInt32(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                    }
                    if (_UserId>0 && _UserId == user_id)
                    {
                        return Ok(new
                        {
                            status = (int)ResponseType.SUCCESS,
                            Logged = 1
                        });
                    }
                    else
                    {
                        return Ok(new
                        {
                            status = (int)ResponseType.FAILED,
                            Logged = 0
                        });
                    }
                }
               
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("CheckIfUserLogged - AccountController" + ex);

            }
            return Ok(new
            {
                status = (int)ResponseType.FAILED,
                Logged = -1
            });
        }
        private async Task CreateCookieAuthenticate(UserDetailViewModel model)
        {
            try
            {
                var claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.NameIdentifier, model.Entity.Id.ToString()));
                claims.Add(new Claim(ClaimTypes.Name, model.Entity.UserName));
                claims.Add(new Claim("DepartmentId", (model.Entity.DepartmentId ?? 0).ToString()));
                claims.Add(new Claim(ClaimTypes.Email, model.Entity.Email));
                claims.Add(new Claim(ClaimTypes.Role, string.Join(",", model.RoleIdList)));

                //--Get and Cache Permission:
                UserRoleCacheModel user_role_cache = new UserRoleCacheModel();
                var role_permission = await _UserRepository.GetUserPermissionById(model.Entity.Id);
                if (role_permission != null && role_permission.Any())
                {
                    user_role_cache.Permission = role_permission.Select(s => new PermissionData
                    {
                        MenuId = s.MenuId,
                        RoleId = s.RoleId,
                        PermissionId = s.PermissionId
                    });
                }
                else
                {
                    user_role_cache.Permission = Enumerable.Empty<PermissionData>();
                }
                user_role_cache.UserUnderList = _UserRepository.GetListUserByUserId(model.Entity.Id);
                var data_encode = JsonConvert.SerializeObject(user_role_cache);
                string token = CommonHelper.Encode(data_encode, _configuration["DataBaseConfig:key_api:api_manual"]);
                //string token = data_encode;
                _redisConn.Set(CacheName.USER_ROLE + model.Entity.Id + "_" + _configuration["CompanyType"], token, Convert.ToInt32(_configuration["Redis:Database:db_common"]));


                //-- Login:
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    AllowRefresh = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddDays(1),
                    IsPersistent = true
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
            }
            catch
            {
                throw;
            }
        }

        public IActionResult Login(string url = "/")
        {
            ViewBag.ReturnURL = url;
            return View();
        }

        /// <summary>
        /// Function thực hiện xử lý đăng nhập bước 1.
        /// </summary>
        /// <param name="entity"> Thông tin đăng nhập</param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmLogin(AccountModel model)
        {
            try
            {
                //-- Validate Input
                if (model == null || model.UserName == null || model.UserName.Trim() == "" || model.Password == null || model.Password.Trim() == "")
                {
                    return Ok(new
                    {
                        status = (int)ResponseType.FAILED,
                        msg = "Tài khoản / Mật khẩu không được để trống, vui lòng thử lại"
                    });
                }
                //-- Bỏ ký tự đặc biệt
                model.ReturnUrl = CommonHelper.RemoveAllSpecialCharacterinURL(model.ReturnUrl);
                model.UserName = CommonHelper.RemoveAllSpecialCharacterLogin(model.UserName);
                model.UserName = model.UserName.Replace("+", "").Replace("//", "").Replace("=", "");
                model.Password = CommonHelper.RemoveAllSpecialCharacterLogin(model.Password);
                //-- Kiểm tra user/pass
                var user = await _UserRepository.CheckExistAccount(model);
                if (user == null || user.Entity == null || user.Entity.Id <= 0)
                {
                    return Ok(new
                    {
                        status = (int)ResponseType.FAILED,
                        msg = "Tài khoản / Mật khẩu không chính xác, vui lòng thử lại"
                    });
                }
                //-- Nếu tài khoản bị khóa
                if (user.Entity.Status != 0)
                {
                    return Ok(new
                    {
                        status = (int)ResponseType.FAILED,
                        msg = "Tài khoản của bạn đã bị khóa, vui lòng liên hệ IT"
                    });
                }
                //-- Nếu môi trường QC
                if (_configuration["Config:On_QC_Environment"] == "1")
                {
                    await CreateCookieAuthenticate(user);
                    return Ok(new
                    {
                        status = (int)ResponseType.SUCCESS,
                        msg = "Đăng nhập thành công",
                        direct = model.ReturnUrl ?? "/"
                    });
                }

                var remoteIpAddress = HttpContext.Request.HttpContext.Connection.RemoteIpAddress;

                //-- Tạo token
                UserLoginModel login_model = new UserLoginModel()
                {
                    id = user.Entity.Id,
                    date = DateTime.Now.AddHours(4),
                    url = model.ReturnUrl ?? "/",
                    user = model.UserName,
                    pass = model.Password,
                    ip = remoteIpAddress.ToString()
                };
                var key = MFAService.Get_AESKey(MFAService.ConvertBase64StringToByte(ReadFile.LoadConfig().AES_KEY));
                var iv = MFAService.Get_AESIV(MFAService.ConvertBase64StringToByte(ReadFile.LoadConfig().AES_IV));
                var encrypt = MFAService.AES_EncryptToByte(JsonConvert.SerializeObject(login_model), key, iv);
                var token = MFAService.ConvertByteToBase64String(encrypt);
                //Set Session:
                HttpContext.Session.SetString("token", token);

                //-- Kiểm tra bảo mật 2 lớp :
                Mfauser mfa_detail = await _mFARepository.get_MFA_DetailByUserID(user.Entity.Id);
                //-- Nếu chưa tạo hoặc chưa quét QR
                if (mfa_detail == null || mfa_detail.Status == 0)
                {
                    bool create_2fa_status = false;
                    //-- Tạo MFA
                    if (mfa_detail == null)
                    {

                        var new_2fa_model = await MFAService.Get2FAModel(user);
                        var mfa_id = await _mFARepository.CreateAsync(new_2fa_model);
                        if (mfa_id > 0) create_2fa_status = true;
                    }
                    //-- Có MFA sẵn
                    else
                    {
                        create_2fa_status = true;
                    }
                    //-- Direct To Setup
                    if (create_2fa_status)
                    {
                        return Ok(new
                        {
                            status = (int)ResponseType.SUCCESS,
                            msg = "Tài khoản chưa được thiết lập 2FA, chuyển đến trang cài đặt 2FA ",
                            direct = "/Account/2FA/",
                        });
                    }

                }
                //Nếu đã thiết lập 2FA:
                else
                {
                    return Ok(new
                    {
                        status = (int)ResponseType.SUCCESS,
                        msg = "Nhập mã OTP",
                        direct = "/Account/OTP",
                    });
                }

            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("ConfirmLogin - AccountController" + ex);
            }
            return Ok(new
            {
                status = (int)ResponseType.FAILED,
                msg = "Có lỗi xảy ra trong quá trình đăng nhập, vui lòng liên hệ IT"
            });
        }
        public async Task<IActionResult> Setup2FA()
        {
            try
            {
                string token = HttpContext.Session.GetString("token");
                if (token != null && token.Trim() != "")
                {
                    var remoteIpAddress = HttpContext.Request.HttpContext.Connection.RemoteIpAddress;

                    UserLoginModel login_model = MFAService.DecryptLoginModelFromToken(token);
                    if (login_model != null && login_model.id > 0 && DateTime.Now < login_model.date && login_model.ip == remoteIpAddress.ToString())
                    {
                        var mfa_record = await _mFARepository.get_MFA_DetailByUserID(Convert.ToInt32(login_model.id));
                        var user = await _UserRepository.GetById(login_model.id);
                        string enviroment = _configuration["Config:OTP_Enviroment"];
                        if (enviroment == null) enviroment = "";
                        ViewBag.QRCodeUri = MFAService.GenerateQRCode(mfa_record, enviroment);
                        ViewBag.SecretKey = MFAService.FormatKey(mfa_record.SecretKey);
                        string label_name = "AdavigoCMS_" + enviroment + "-" + user.UserName;
                        ViewBag.Issurer = label_name;
                        ViewBag.Status = mfa_record.Status;
                        return View();
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("Setup2FA - AccountController" + ex);
            }
            return RedirectToAction("Login", "Account");
        }
        [HttpPost]
        public async Task<ActionResult> OTPTest(string otp)
        {
            try
            {
                //Set Session:
                string token = HttpContext.Session.GetString("token");
                if (token != null && token.Trim() != "")
                {
                    UserLoginModel login_model = MFAService.DecryptLoginModelFromToken(token);
                    var remoteIpAddress = HttpContext.Request.HttpContext.Connection.RemoteIpAddress;

                    if (login_model != null && login_model.id > 0 && DateTime.Now < login_model.date && login_model.ip == remoteIpAddress.ToString())
                    {
                        //-- Build mã OTP:
                        var mfa_record = await _mFARepository.get_MFA_DetailByUserID(Convert.ToInt32(login_model.id));
                        var correct_otp = MFAService.CompareOTP(mfa_record, otp);
                        if (correct_otp)
                        {
                            return Ok(new
                            {
                                status = (int)ResponseType.SUCCESS,
                                msg = "Xác thực thành công đối với mã OTP đã nhập."
                            });
                        }
                        else
                        {
                            return Ok(new
                            {
                                status = (int)ResponseType.FAILED,
                                msg = "Xác thực OTP không thành công, vui lòng kiểm tra lại cài đặt."
                            });
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("OTPTest - AccountController" + ex);

            }
            return Ok(new
            {
                status = (int)ResponseType.FAILED,
                msg = "Lỗi trong quá trình xử lý, vui lòng liên hệ với IT."
            });
        }
        [HttpPost]
        public async Task<ActionResult> Confirm2FA()
        {
            try
            {
                string token = HttpContext.Session.GetString("token");
                if (token != null && token.Trim() != "")
                {
                    UserLoginModel login_model = MFAService.DecryptLoginModelFromToken(token);
                    var remoteIpAddress = HttpContext.Request.HttpContext.Connection.RemoteIpAddress;

                    if (login_model != null && login_model.id > 0 && DateTime.Now < login_model.date && login_model.ip == remoteIpAddress.ToString())
                    {
                        //-- Build mã OTP:
                        var mfa_record = await _mFARepository.get_MFA_DetailByUserID(Convert.ToInt32(login_model.id));
                        mfa_record.Status = 1;
                        var result = await _mFARepository.UpdateAsync(mfa_record);
                        if (result)
                        {
                            HttpContext.Session.Remove("token");
                            return Ok(new
                            {
                                status = (int)ResponseType.SUCCESS,
                                msg = "Kích hoạt bảo mật 2 lớp thành công."
                            });
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("Confirm2FA - AccountController" + ex);

            }
            return Ok(new
            {
                status = (int)ResponseType.FAILED,
                msg = "Có lỗi trong quá trình xử lý, vui lòng liên hệ IT."
            });
        }
        /// <summary>
        /// Trả view OTP
        /// </summary>
        /// <returns></returns>
        public IActionResult OTP()
        {
            try
            {
                string token = HttpContext.Session.GetString("token");
                if (token != null && token.Trim() != "")
                {
                    UserLoginModel login_model = MFAService.DecryptLoginModelFromToken(token);
                    var remoteIpAddress = HttpContext.Request.HttpContext.Connection.RemoteIpAddress;

                    if (login_model != null && login_model.id > 0 && DateTime.Now < login_model.date && login_model.ip == remoteIpAddress.ToString())
                    {
                        ViewBag.Token = MFAService.Base64StringToURLParam(token);
                        return View();
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("OTP - AccountController" + ex);

            }
            return RedirectToAction("Login", "Account");
        }
        /// <summary>
        /// Login với OTP
        /// </summary>
        /// <param name = "record" ></ param >
        /// < returns ></ returns >
        [HttpPost]
        public async Task<IActionResult> SendOTP(string otp)
        {
            try
            {
                string token = HttpContext.Session.GetString("token");
                if (token != null && token.Trim() != "")
                {
                    UserLoginModel login_model = MFAService.DecryptLoginModelFromToken(token);
                    var remoteIpAddress = HttpContext.Request.HttpContext.Connection.RemoteIpAddress;

                    if (login_model != null && login_model.id > 0 && DateTime.Now < login_model.date && login_model.ip == remoteIpAddress.ToString())
                    {
                        //-- Build mã OTP:
                        var mfa_record = await _mFARepository.get_MFA_DetailByUserID(Convert.ToInt32(login_model.id));
                        var correct_otp = MFAService.CompareOTP(mfa_record, otp);
                        if (correct_otp)
                        {
                            AccountModel account = new AccountModel()
                            {
                                UserName = login_model.user,
                                Password = login_model.pass
                            };
                            var model = await _UserRepository.CheckExistAccount(account);
                            await CreateCookieAuthenticate(model);
                            HttpContext.Session.Remove("token");
                            return Ok(new
                            {
                                status = (int)ResponseType.SUCCESS,
                                msg = "Xác thực OTP thành công.",
                                direct = login_model.url ?? "/"
                            });
                        }
                        else
                        {
                            return Ok(new
                            {
                                status = (int)ResponseType.FAILED,
                                msg = "Xác thực OTP không thành công, vui lòng kiểm tra lại mã OTP."
                            });
                        }
                    }
                }



            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("SendOTP - AccountController" + ex);
            }
            return Ok(new
            {
                status = (int)ResponseType.FAILED,
                msg = "Lỗi trong quá trình xác thực OTP, vui lòng liên hệ IT."
            });
        }
    }
}
