using Caching.Elasticsearch;
using Entities.Models;
using Entities.ViewModels;
using Entities.ViewModels.CustomerManager;
using Entities.ViewModels.ElasticSearch;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Utilities;
using Utilities.Contants;
using WEB.Adavigo.CMS.Service;
using WEB.CMS.Customize;
using WEB.CMS.Models;
using WEB.CMS.Service;

namespace WEB.Adavigo.CMS.Controllers
{
    [CustomAuthorize]
    public class CustomerManagerController : Controller
    {
        private readonly ICustomerManagerRepository _customerManagerRepositories;

        private readonly IConfiguration _configuration;
        private readonly IAllCodeRepository _allCodeRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IUserRepository _userRepository;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        private readonly IBankingAccountRepository _bankingAccountRepository;
        private ManagementUser _ManagementUser;
        private IAccountClientRepository _accountClientRepository;
        private LogCacheFilterMongoService _logCacheFilterMongoService;
        private IUserAgentRepository _userAgentRepository;
        private UserESRepository _userESRepository;
        private IIdentifierServiceRepository _identifierServiceRepository;
        private readonly IOrderRepository _orderRepository;
        public CustomerManagerController(IConfiguration configuration, ICustomerManagerRepository customerManagerRepositories, ManagementUser ManagementUser, IWebHostEnvironment WebHostEnvironment, IAccountClientRepository accountClientRepository,
         IAllCodeRepository allCodeRepository, IClientRepository clientRepository, IUserRepository userRepository, IBankingAccountRepository bankingAccountRepository, IUserAgentRepository userAgentRepository,
         IIdentifierServiceRepository identifierServiceRepository, IOrderRepository orderRepository)
        {
            _customerManagerRepositories = customerManagerRepositories;
            _configuration = configuration;
            _allCodeRepository = allCodeRepository;
            _clientRepository = clientRepository;
            _userRepository = userRepository;
            _ManagementUser = ManagementUser;
            _WebHostEnvironment = WebHostEnvironment;
            _bankingAccountRepository = bankingAccountRepository;
            _accountClientRepository = accountClientRepository;
            _logCacheFilterMongoService = new LogCacheFilterMongoService(configuration);
            _userAgentRepository = userAgentRepository;
            _userESRepository = new UserESRepository(configuration["DataBaseConfig:Elastic:Host"], configuration);
            _identifierServiceRepository = identifierServiceRepository;
            _orderRepository = orderRepository;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                var key_token_api = _configuration["DataBaseConfig:key_api:api_manual"];
                //string ApiPrefix = ReadFile.LoadConfig().API_URL + ReadFile.LoadConfig().API_ALLCODE;
                var AgencyType = _allCodeRepository.GetListByType(AllCodeType.AGENCY_TYPE);
                var PermisionType = _allCodeRepository.GetListByType(AllCodeType.PERMISION_TYPE);
                var ClientType = _allCodeRepository.GetListByType(AllCodeType.CLIENT_TYPE);
                ViewBag.AgencyType = AgencyType;
                ViewBag.PermisionType = PermisionType;
                ViewBag.ClientType = ClientType;

                var current_user = _ManagementUser.GetCurrentUser();
                ViewBag.buttomThem = 0;
                if (current_user != null)
                {
                    var i = 0;
                    if (current_user != null && !string.IsNullOrEmpty(current_user.Role))
                    {
                        var list = Array.ConvertAll(current_user.Role.Split(','), int.Parse);
                        foreach (var item in list)
                        {
                            //kiểm tra chức năng có đc phép sử dụng
                            var listPermissions = await _userRepository.CheckRolePermissionByUserAndRole(current_user.Id, item, (int)SortOrder.THEM, (int)MenuId.QL_KHACH_HANG);
                            if (listPermissions == true)
                            {
                                ViewBag.buttomThem = 1;
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("Index - CustomerManagerController: " + ex);
            }

            return View();
        }

        public async Task<IActionResult> CustomerManagerDetail(int id)
        {
            try
            {

                var AgencyType = _allCodeRepository.GetListByType("AGENCY_TYPE");
                var PermisionType = _allCodeRepository.GetListByType("PERMISION_TYPE");
                var ClientType = _allCodeRepository.GetListByType("CLIENT_TYPE");
                ViewBag.AgencyType = AgencyType;
                ViewBag.PermisionType = PermisionType;
                ViewBag.ClientType = ClientType;

                if (id != 0)
                {
                    var model = await _clientRepository.GetClientDetailByClientId(id);
                    ViewBag.dataModel = model;
                    return PartialView();
                }
                return PartialView();
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("CustomerManagerDetail - CustomerManagerController: " + ex);
                return PartialView();
            }

        }
        [HttpPost]
        public async Task<IActionResult> ListClient(CustomerManagerViewSearchModel searchModel, int currentPage = 1, int pageSize = 20)
        {
            var model = new GenericViewModel<CustomerManagerViewModel>();

            try
            {
                var current_user = _ManagementUser.GetCurrentUser();
                if (current_user != null)
                {
                    var i = 0;
                    if (searchModel.CacheName != null)
                    {
                        var data = _logCacheFilterMongoService.GetListLogCache(null, searchModel.CacheName);
                        if (data != null)
                        {
                            searchModel.MaKH = searchModel.MaKH == -1 ? data[0].MaKH : searchModel.MaKH;
                            searchModel.CreatedBy = searchModel.CreatedBy == -1 ? data[0].CreatedBy : searchModel.CreatedBy;
                            searchModel.UserId = searchModel.UserId == -1 ? data[0].UserId : searchModel.UserId;
                            searchModel.TenKH = searchModel.TenKH == null ? data[0].TenKH : searchModel.TenKH;
                            searchModel.Email = searchModel.Email == null ? data[0].Email : searchModel.Email;
                            searchModel.Phone = searchModel.Phone == null ? data[0].Phone : searchModel.Phone;
                            searchModel.AgencyType = searchModel.AgencyType == -1 ? data[0].AgencyType : searchModel.AgencyType;
                            searchModel.ClientType = searchModel.ClientType == -1 ? data[0].ClientType : searchModel.ClientType;
                            searchModel.PermissionType = searchModel.PermissionType == -1 ? data[0].PermissionType : searchModel.PermissionType;
                            searchModel.CreateDate = searchModel.CreateDate == null ? data[0].CreateDate : searchModel.CreateDate;
                            searchModel.EndDate = searchModel.EndDate == null ? data[0].EndDate : searchModel.EndDate;
                            searchModel.MinAmount = searchModel.MinAmount == -1 ? data[0].MinAmount : searchModel.MinAmount;
                            searchModel.MaxAmount = searchModel.MaxAmount == -1 ? data[0].MaxAmount : searchModel.MaxAmount;

                        }

                    }
                    if (current_user != null && !string.IsNullOrEmpty(current_user.Role))
                    {
                        var list = Array.ConvertAll(current_user.Role.Split(','), int.Parse);
                        foreach (var item in list)
                        {
                            //kiểm tra chức năng có đc phép sử dụng
                            var listPermissions = await _userRepository.CheckRolePermissionByUserAndRole(current_user.Id, item, (int)SortOrder.TRUY_CAP, (int)MenuId.QL_KHACH_HANG);
                            var listPermissions6 = await _userRepository.CheckRolePermissionByUserAndRole(current_user.Id, item, (int)SortOrder.VIEW_ALL, (int)MenuId.QL_KHACH_HANG);
                            if (listPermissions == true)
                            {
                                searchModel.SalerPermission = current_user.Id.ToString(); i++;
                            }
                            if (listPermissions6 == true)
                            {
                                searchModel.SalerPermission = current_user.UserUnderList;
                                i++;
                            }
                            if (item == (int)RoleType.Admin)
                            {
                                searchModel.SalerPermission = null;
                                i++;
                            }
                        }
                    }
                    if (i != 0)
                    {
                        model = await _customerManagerRepositories.GetPagingList(searchModel, currentPage, pageSize);
                    }

                }

            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("ListClient - CustomerManagerController: " + ex);
            }

            return PartialView(model);
        }
        public async Task<IActionResult> Detail(long id)
        {
            try
            {
                var model = await _clientRepository.GetClientDetailByClientId(id);

                if (model != null && model.ClientType != ClientType.KL)
                {
                    ViewBag.btnStatus = 1;
                }
                ViewBag.id = id;
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("Detail - CustomerManagerController: " + ex);
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Setup(string data)
        {
            int stt_code = (int)ResponseType.FAILED;
            string msg = "Error On Excution";
            try
            {
                var DataModel = JsonConvert.DeserializeObject<CustomerManagerView>(data);
                DataModel.UserId = Convert.ToInt32(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

                Regex regexemail = new Regex(@"^(\s*)([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)(\s*)|((\.(\w){2,})+)(\s*)$");
                if (!regexemail.IsMatch(DataModel.email))
                {
                    return Ok(new
                    {
                        stt_code = (int)ResponseType.FAILED,
                        msg = "Email nhập không đúng định dạng",

                    });
                }

                var email = _clientRepository.GetClientByEmail(DataModel.email);

                if (email == null && DataModel.Id == 0)
                {

                    DataModel.ClientCode = await _identifierServiceRepository.buildClientNo(Convert.ToInt32(DataModel.id_ClientType));
                    var Result = _customerManagerRepositories.SetUpClient(DataModel);
                    if (Result != 0)
                    {

                        if (Result == 2)
                        {
                            stt_code = (int)ResponseType.FAILED;
                            msg = "Mã Code đã tồn tại";
                        }
                        else
                        {

                            //var SendMail = await apiService.SendMailResetPassword(DataModel.email);
                            stt_code = (int)ResponseType.SUCCESS;
                            msg = "Thêm mới thông tin thành công";
                        }
                    }
                    else
                    {
                        stt_code = (int)ResponseType.FAILED;
                        msg = "Thêm mới thông tin không thành công";

                    }
                }
                if (email != null && DataModel.Id != 0 || email == null && DataModel.Id != 0)
                {

                    var Result = _customerManagerRepositories.SetUpClient(DataModel);
                    if (Result == 1)
                    {
                        stt_code = (int)ResponseType.SUCCESS;
                        msg = "Cập nhật thông tin thành công";
                    }
                    if (Result == 2)
                    {
                        stt_code = (int)ResponseType.FAILED;
                        msg = "Email đã tồn tại";
                    }

                    if (Result == 0)
                    {
                        stt_code = (int)ResponseType.FAILED;
                        msg = "Cập nhật thông tin không thành công";
                    }
                }
                if (email != null && DataModel.Id == 0)
                {

                    stt_code = (int)ResponseType.FAILED;
                    msg = "Email đã tồn tại";
                }


            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("Setup - CustomerManagerController: " + ex);
                stt_code = (int)ResponseType.ERROR;
                msg = "Lỗi kỹ thuật vui lòng liên hệ bộ phận IT";
            }

            return Ok(new
            {
                stt_code = stt_code,
                msg = msg,

            });
        }
        [HttpPost]
        public IActionResult ListPaymentAccount(int id)
        {
            var model = new List<BankingAccount>();
            try
            {
                model = _bankingAccountRepository.GetBankAccountByClientId(id);

            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("ListPaymentAccount - CustomerManagerController: " + ex);
            }

            return PartialView(model);
        }
        [HttpPost]
        public async Task<IActionResult> DetailCustomerManager(long id)
        {

            try
            {
                var Amount = await _customerManagerRepositories.GetAmountRemainOfContractByClientId(id);
                var data = await _customerManagerRepositories.GetDetailClient(id);
                var model = _userAgentRepository.UserAgentByClient((int)id, 0);
                ViewBag.userAgent = model;
                if (Amount != null) { ViewBag.Amount = Amount.AmountRemain; }
                else
                {
                    ViewBag.Amount = 0;
                }



                return PartialView(data);
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("DetailCustomerManager - CustomerManagerController: " + ex);
                return PartialView();
            }
        }
        public async Task<string> GetSuggestionUser(string name)
        {
            try
            {

                var listData = _userRepository.GetAll();
                var SuggestOrder = listData.Where(x => x.FullName.ToLower().Contains(name)).Select(s => new
                {
                    id = s.Id,
                    name = s.FullName
                });
                return JsonConvert.SerializeObject(SuggestOrder);
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetSuggestionUser - CustomerManagerController: " + ex);
                return null;
            }
        }
        public async Task<string> GetSuggestionClient(string name)
        {
            try
            {
                var listData = _clientRepository.GetAllClient();
                var SuggestOrder = listData.Where(x => x.ClientName.ToLower().Contains(name)).Select(s => new
                {
                    id = s.Id,
                    name = s.ClientName,

                });
                if (SuggestOrder.ToList().Count == 0)
                {
                    SuggestOrder = listData.Where(x => x.Id.ToString().ToLower().Contains(name)).Select(s => new
                    {
                        id = s.Id,
                        name = s.Id.ToString(),

                    });
                };
                if (SuggestOrder.ToList().Count == 0)
                {
                    SuggestOrder = listData.Where(x => x.Email.ToLower().Contains(name)).Select(s => new
                    {
                        id = s.Id,
                        name = s.Email,

                    });
                };
                if (SuggestOrder.ToList().Count == 0)
                {
                    SuggestOrder = listData.Where(x => x.Phone.ToLower().Contains(name)).Select(s => new
                    {
                        id = s.Id,
                        name = s.Phone,

                    });
                };
                return JsonConvert.SerializeObject(SuggestOrder);
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetSuggestionClient - CustomerManagerController: " + ex.ToString());
                return null;
            }
        }
        [HttpPost]
        public async Task<IActionResult> ListClientType(int id)
        {

            try
            {

                var key_token_api = _configuration["DataBaseConfig:key_api:api_manual"];
                //string ApiPrefix = ReadFile.LoadConfig().API_URL + ReadFile.LoadConfig().API_ALLCODE;

                var ClientType = _allCodeRepository.GetListByType("CLIENT_TYPE");
                var PermisionType = _allCodeRepository.GetListByType("PERMISION_TYPE");
                if (id != 1)
                {
                    return Ok(new
                    {
                        status = (int)ResponseType.SUCCESS,
                        data = ClientType
                    });
                }
                else
                {
                    return Ok(new
                    {
                        status = (int)ResponseType.SUCCESS,
                        data = PermisionType
                    });
                }

            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("ListClientType - OrderManualController: " + ex.ToString());
                return Ok(new
                {
                    status = (int)ResponseType.SUCCESS,
                    data = new List<AllCode>()
                });
            }

        }
        [HttpPost]
        public async Task<IActionResult> ResetStatusAc(long clientId, long Status)
        {
            int status = 0;
            string msg = "Thay đổi không thành công";
            try
            {

                var ClientType = _customerManagerRepositories.ResetStatusAc(clientId, Status, 0);
                if (ClientType != 0)
                {
                    status = (int)ResponseType.SUCCESS;
                    msg = "Thay đổi thành công";
                }
                else
                {
                    status = (int)ResponseType.FAILED;
                    msg = "Thay đổi không thành công";
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("ResetStatusAc - OrderManualController: " + ex.ToString());
                return Ok(new
                {
                    status = (int)ResponseType.FAILED,
                    msg = "Không thành công vui lòng liên hệ bộ phận IT"
                });
            }
            return Ok(new
            {
                status = status,
                msg = msg
            });

        }

        [HttpPost]
        public async Task<IActionResult> ExportExcel(CustomerManagerViewSearchModel searchModel, field field)
        {
            try
            {
                int _UserId = 0;
                if (HttpContext.User.FindFirst(ClaimTypes.NameIdentifier) != null)
                {
                    _UserId = Convert.ToInt32(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                }
                string _FileName = StringHelpers.GenFileName("Danh sách khách hàng", _UserId, "xlsx");
                string _UploadFolder = @"Template\Export";
                string _UploadDirectory = Path.Combine(_WebHostEnvironment.WebRootPath, _UploadFolder);

                if (!Directory.Exists(_UploadDirectory))
                {
                    Directory.CreateDirectory(_UploadDirectory);
                }
                //delete all file in folder before export
                try
                {
                    System.IO.DirectoryInfo di = new DirectoryInfo(_UploadDirectory);
                    foreach (FileInfo file in di.GetFiles())
                    {
                        file.Delete();
                    }
                }
                catch
                {
                }
                string FilePath = Path.Combine(_UploadDirectory, _FileName);

                var rsPath = await _customerManagerRepositories.ExportDeposit(searchModel, FilePath, field);

                if (!string.IsNullOrEmpty(rsPath))
                {
                    return new JsonResult(new
                    {
                        isSuccess = true,
                        message = "Xuất dữ liệu thành công",
                        path = "/" + _UploadFolder + "/" + _FileName
                    });
                }
                else
                {
                    return new JsonResult(new
                    {
                        isSuccess = false,
                        message = "Xuất dữ liệu thất bại"
                    });
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("ExportExcel - FundingController: " + ex);
                return new JsonResult(new
                {
                    isSuccess = false,
                    message = ex.Message.ToString()
                });
            }
        }
        [HttpPost]
        public async Task<IActionResult> SendMailResetPasswordb2b(long id)
        {
            int status = (int)ResponseType.SUCCESS;
            string msg = "Gửi email thành công";
            try
            {

                var model = await _clientRepository.GetClientDetailByClientId(id);
                var Ac = _accountClientRepository.AccountClientByClientId(id);

                //APIService apiService = new APIService(_configuration, _userRepository);

                //if (Ac != null && model != null && model.ClientType != ClientType.KL)
                //{
                //    apiService.SendMailResetPassword(Ac.UserName);
                //}
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("SendMailResetPasswordb2b - CustomerManagerController: " + ex);
                status = (int)ResponseType.FAILED;
                msg = "Gửi email không thành công";
            }
            return Ok(new
            {
                status = status,
                msg = msg
            });
        }

        public async Task<IActionResult> InsertLogCache(CustomerManagerViewSearchModel searchModel)
        {

            int status = (int)ResponseType.FAILED;
            string msg = "Error On Excution";
            try
            {

                var Insert = await _logCacheFilterMongoService.InsertLogCache(searchModel);
                if (Insert > 0)
                {
                    status = (int)ResponseType.SUCCESS;
                    msg = "Lưu thành công thành công";
                }
                else
                {
                    status = (int)ResponseType.SUCCESS;
                    msg = "Lưu thành không công thành công";
                }


            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("InsertLogCache - CustomerManagerController: " + ex);
                status = (int)ResponseType.ERROR;
                msg = "Lỗi kỹ thuật vui lòng liên hệ bộ phận IT";
            }
            return Ok(new
            {
                status = status,
                msg = msg,

            });
        }
        public async Task<string> GetSuggestionUserCache(string txt_search)
        {
            try
            {

                var data = _logCacheFilterMongoService.GetListLogCache(txt_search, null);
                return JsonConvert.SerializeObject(data);
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetSuggestionUserCache - CustomerManagerController: " + ex);
                return null;
            }
        }
        public async Task<IActionResult> DetailUserAgent(int user_Id, long client)
        {

            try
            {
                ViewBag.client = client;
                if (user_Id != 0)
                {
                    var model = _userAgentRepository.UserAgentByClient(0, user_Id);
                    if (model != null && model.Count > 0)
                        return PartialView(model[0]);
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("DetailUserAgent - PaymentAccountController: " + ex);
            }
            return PartialView();
        }
        [HttpPost]
        public IActionResult UpdatalUserAgent(int id, int userId, long clientId)
        {

            try
            {

                var create_id = Convert.ToInt32(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                var model = _userAgentRepository.UpdataUserAgent(id, userId, create_id, clientId);
                if (model > 0)
                {
                    return Ok(new
                    {
                        stt_code = (int)ResponseType.SUCCESS,
                        msg = "Đổi nhân viên thành công",

                    });
                }
                else
                {

                    return Ok(new
                    {
                        stt_code = (int)ResponseType.FAILED,
                        msg = "Đổi nhân viên không thành công",

                    });
                }

            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("UpdatalUserAgent - PaymentAccountController: " + ex);
            }
            return Ok(new
            {
                stt_code = (int)ResponseType.FAILED,
                msg = "Đổi nhân viên không thành công",

            });
        }
        public async Task<IActionResult> UserSuggestion(string txt_search, int service_type = 0)
        {

            try
            {
                long _UserId = 0;
                if (HttpContext.User.FindFirst(ClaimTypes.NameIdentifier) != null)
                {
                    _UserId = Convert.ToInt64(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                }
                if (txt_search == null) txt_search = "";
                var data = await _userESRepository.GetUserSuggesstion(txt_search);
                return Ok(new
                {
                    status = (int)ResponseType.SUCCESS,
                    data = data,
                });

            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("UserSuggestion - CustomerManagerController: " + ex.ToString());
                return Ok(new
                {
                    status = (int)ResponseType.SUCCESS,
                    data = new List<CustomerESViewModel>()
                });
            }

        }
        public async Task<IActionResult> ClientSuggestion(string txt_search)
        {

            try
            {

                if (string.IsNullOrEmpty(txt_search))
                {
                    return Ok(new
                    {
                        status = (int)ResponseType.EMPTY
                    });
                }
                else
                {
                    bool isUnicode = Encoding.ASCII.GetByteCount(txt_search) != Encoding.UTF8.GetByteCount(txt_search);


                    byte[] utfBytes = Encoding.UTF8.GetBytes(txt_search.Trim());
                    txt_search = Encoding.UTF8.GetString(utfBytes);
                }

                var es_service = new esService(_configuration);
                var data_hotel = await es_service.search(txt_search, "searchClient.json");
                if (data_hotel != "{}")
                {
                    //var es_result =// ((RestSharp.RestResponseBase)find_hotel).Content;                       

                    JObject jsonObject = JObject.Parse(data_hotel);
                    var hits = (JArray)jsonObject["hits"]["hits"];
                    var hotel_result = new List<earchClientESViewModel>();
                    foreach (var hit in hits)
                    {
                        var source = JsonConvert.DeserializeObject<earchClientESViewModel>(hit["_source"].ToString());
                        hotel_result.Add(source);
                    }

                    return Ok(new
                    {
                        status = (int)ResponseType.SUCCESS,
                        data = hotel_result,
                    });
                }
                else
                {
                    return Ok(new
                    {
                        status = (int)ResponseType.EMPTY,
                        msg = "Không có dữ liệu nào thỏa mãn từ khóa " + txt_search
                    });
                }


            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("ClientSuggestion - ContractController: " + ex);
                return Ok(new
                {
                    status = (int)ResponseType.SUCCESS,
                    data = new List<CustomerESViewModel>()
                });
            }

        }
        [HttpPost]
        public async Task<IActionResult> ListOrderbyClientid(OrderViewSearchModel searchModel, long currentPage, long pageSize)
        {

            try
            {
                searchModel.pageSize = (int)pageSize;
                searchModel.PageIndex = (int)currentPage;
                var model = new GenericViewModel<OrderViewModel>();
                model = await _orderRepository.GetList(searchModel);
           
                return PartialView(model);
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("ListOrderbyClientid - CustomerManagerController: " + ex.ToString());

            }
            return PartialView();
        }
    }
}
