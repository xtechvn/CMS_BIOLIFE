using Entities.ViewModels;
using Entities.ViewModels.API;
using Entities.ViewModels.ApiSever;
using ENTITIES.ViewModels.Notify;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Utilities;
using Utilities.Contants;
using WEB.CMS.Models;

namespace WEB.Adavigo.CMS.Service
{
    public class APIService
    {
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository;
        private HttpClient _HttpClient;
        private const string CONST_TOKEN_PARAM = "token";
        private readonly string _ApiSecretKey;
        private string USER_NAME = "test";
        private string PASSWORD = "password";
        private string API_GET_TOKEN = "/api/auth/login";
        private string TOKEN = "";
        public APIService(IConfiguration configuration, IUserRepository userRepository)
        {
            _configuration = configuration;
            _userRepository = userRepository;
            _HttpClient = new HttpClient(new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, certificate2, arg3, arg4) => true
            })
            {
                BaseAddress = new Uri(configuration["API:Domain"])
            };
            _ApiSecretKey = configuration["API:SecretKey"];
            API_GET_TOKEN = configuration["API:GetToken"];
            USER_NAME = configuration["API:username"];
            PASSWORD = configuration["API:password"];

        }
        public async Task<string> POST(string endpoint, object request)
        {
            try
            {
                if (TOKEN == null || TOKEN.Trim() == "") TOKEN = await GetToken();
                string token = EncodeHelpers.Encode(JsonConvert.SerializeObject(request), _ApiSecretKey);
                var request_message = new HttpRequestMessage(HttpMethod.Post, endpoint);
                request_message.Headers.Add("Authorization", "Bearer " + TOKEN);
                var content = new StringContent("{\"token\":\"" + token + "\"}", Encoding.UTF8, "application/json");
                request_message.Content = content;
                var response = await _HttpClient.SendAsync(request_message);
                return await response.Content.ReadAsStringAsync();
            }
            catch
            {
                return null;
            }
        }
        public async Task<string> GetToken()
        {
            try
            {
                var request = new UserLoginAPIModel()
                {
                    Username = USER_NAME,
                    Password = PASSWORD
                };
                var request_message = new HttpRequestMessage(HttpMethod.Post, API_GET_TOKEN);
                var content = new StringContent(JsonConvert.SerializeObject(request), null, "application/json");
                request_message.Content = content;
                var response = await _HttpClient.SendAsync(request_message);
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                {
                    var json = JObject.Parse(await response.Content.ReadAsStringAsync());
                    var status = int.Parse(json["status"].ToString());
                    if (status != (int)ResponseType.SUCCESS)
                    {
                        LogHelper.InsertLogTelegram("GetToken - APIService:" + json["msg"].ToString());
                    }
                    else
                    {
                        return json["token"].ToString();
                    }

                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetToken - APIService:" + ex.ToString());

            }
            return null;

        }
        public async Task<int> SendMailResetPassword(string email)
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                var j_param = new Dictionary<string, string>()
                {
                {"template_type","2" },
                {"email", email},
                };
                var data = JsonConvert.SerializeObject(j_param);
                var a = _configuration["DataBaseConfig:key_api:api_manual"];
                var token = EncodeHelpers.Encode(data, _configuration["DataBaseConfig:key_api:api_manual"]);
                var request = new FormUrlEncodedContent(new[]
                    {
                    new KeyValuePair<string, string>("token",token)
                });
                var url = ReadFile.LoadConfig().API_URL + ReadFile.LoadConfig().API_Send_Email_Reset_Password;
                var response = await httpClient.PostAsync(url, request);


                if (response.IsSuccessStatusCode)
                {

                    return 0;
                }

                return 1;
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("apisever:" + ex.ToString());
                return 1;
            }
        }
        public async Task<string> buildContractNo()
        {
            try
            {
                HttpClient httpClient = new HttpClient();

                string private_token_key = _configuration["DataBaseConfig:key_api:api_manual"];
                // Mã hợp dồng
                JObject jsonObject = new JObject(
                   new JProperty("code_type", "3")
               );
                var j_param = new Dictionary<string, object>
                {
                    { "key",jsonObject}
                };
                var data_product = JsonConvert.SerializeObject(j_param);
                var token = CommonHelper.Encode(data_product, private_token_key);
                var request = new FormUrlEncodedContent(new[]
                    {
                    new KeyValuePair<string, string>("token",token)
                });
                var url = ReadFile.LoadConfig().API_URL + ReadFile.LoadConfig().Get_Order_no;
                var response = await httpClient.PostAsync(url, request);


                if (response.IsSuccessStatusCode)
                {
                    var text = response.Content.ReadAsStringAsync().Result;
                    var result = JsonConvert.DeserializeObject<buildContractNoViewModel>(text);
                    if (result.status == 0)
                    {
                        return result.code;
                    }
                    else {
                        LogHelper.InsertLogTelegram("apisever:" + result.msg);
                        return null;
                    }

                }

                return null;
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("apisever:" + ex.ToString());
                return null;
            }
        }
        public async Task<string> buildClientCode(string client_type)
        {
            try
            {
                HttpClient httpClient = new HttpClient();

                string private_token_key = _configuration["DataBaseConfig:key_api:api_manual"];
                // Mã hợp dồng
                JObject jsonObject = new JObject(
                     new JProperty("code_type", "8"),
                     new JProperty("client_type", client_type)
                 );
                var j_param = new Dictionary<string, object>
                {
                    { "key",jsonObject}
                };
                var data_product = JsonConvert.SerializeObject(j_param);
                var token = CommonHelper.Encode(data_product, private_token_key);
                var request = new FormUrlEncodedContent(new[]
                    {
                    new KeyValuePair<string, string>("token",token)
                });
                var url = ReadFile.LoadConfig().API_URL + ReadFile.LoadConfig().Get_Order_no;
                var response = await httpClient.PostAsync(url, request);


                if (response.IsSuccessStatusCode)
                {
                    var text = response.Content.ReadAsStringAsync().Result;
                    var result = JsonConvert.DeserializeObject<buildContractNoViewModel>(text);
                    if (result.status == 0)
                    {
                        return result.code;
                    }
                    else
                    {
                        LogHelper.InsertLogTelegram("apisever:" + result.msg);
                        return null;
                    }

                }

                return null;
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("apisever:" + ex.ToString());
                return null;
            }
        }
        public async Task<ObjectResponse<ProductCategoryViewModel>> GetProductCategory()
        {
            try
            {


                var url = ReadFile.LoadConfig().API_ADAVIGO_URL + ReadFile.LoadConfig().Get_Product_Category_By_Parent_Id;


                HttpClient httpClient = new HttpClient();

                var content = new FormUrlEncodedContent(new[] { new KeyValuePair<string, string>("token", "OmYiIDZWKRUTOm4QBxV/dHY8") });
                var response = await httpClient.PostAsync(url, content);
                var contents = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ObjectResponse<ProductCategoryViewModel>>(contents);

            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("apisever:" + ex.ToString());
                return null;
            }

        }
        public async Task<int> SendMessage(string user_id_send, string module_type, string action_type, string Code, string link_redirect, string role_type, string service_code = "")
        {
            try
            {

                var user = await _userRepository.GetDetailUser(Convert.ToInt32(user_id_send));
                HttpClient httpClient = new HttpClient();
                var j_param = new Dictionary<string, object>
                {
                   {"user_name_send", user.Entity.FullName.ToString()}, //tên người gửi
                    {"user_id_send", user_id_send}, //id người gửi
                    {"code", Code}, // mã đối tượng gửi
                    {"link_redirect", link_redirect}, // Link mà khi người dùng click vào detail item notify sẽ chuyển sang đó
                    {"module_type", module_type}, // loại module thực thi luồng notify. Ví dụ: Đơn hàng, khách hàng.......
                    {"action_type", action_type}, // action thực hiện. Ví dụ: Duyệt, tạo mới, từ chối....
                    {"role_type", role_type}, // quyền mà sẽ gửi tới
                    {"service_code", service_code}// mã dịch vụ
                };
                var data_product = JsonConvert.SerializeObject(j_param);

                var token = CommonHelper.Encode(data_product, _configuration["DataBaseConfig:key_api:b2c"]);
                var request = new FormUrlEncodedContent(new[]
                    {
                    new KeyValuePair<string, string>("token",token)
                });
                var url = ReadFile.LoadConfig().API_ADAVIGO_URL + ReadFile.LoadConfig().send_Message;
                var response = await httpClient.PostAsync(url, request);
                if (response.IsSuccessStatusCode)
                {
                    return 0;
                }

                return 1;
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("SendMessage-apisever:" + ex.ToString());
                return 1;
            }
        }

        public async Task<NotifySummeryViewModel> GetListNotify(string user_id)
        {
            try
            {

                HttpClient httpClient = new HttpClient();
                NotifySummeryViewModel result = null;
                var j_param = new Dictionary<string, object>
                {
                       {"user_id", user_id}
                };
                var data_product = JsonConvert.SerializeObject(j_param);

                var token = CommonHelper.Encode(data_product, _configuration["DataBaseConfig:key_api:b2c"]);
                var request = new FormUrlEncodedContent(new[]
                    {
                    new KeyValuePair<string, string>("token",token)
                });
                var url = ReadFile.LoadConfig().API_ADAVIGO_URL + ReadFile.LoadConfig().Notify_Get_List;
                var response = await httpClient.PostAsync(url, request);
                var stringResult = "";

                if (response.IsSuccessStatusCode)
                {
                    stringResult = response.Content.ReadAsStringAsync().Result;

                    var data = JsonConvert.DeserializeObject<NotifyRedisViewModel>(stringResult);
                    result = data.data;
                    return result;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetListNotify:" + ex.ToString());
                return null;
            }
        }
        public async Task<int> UpdateNotify(string notify_id, string user_seen_id, string seen_status)
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                var j_param = new Dictionary<string, object>
                {
                  /*  {"notify_id", "A1,A32"}, // 
                    {"user_seen_id", "222"},*/
                     {"notify_id", notify_id}, // 
                    {"user_seen_id", user_seen_id},
                    {"seen_status", seen_status}, // SEEN_ALL = 1: click vao chuông |    SEEN_DETAIL = 2  click vao item notify

                };
                var data = JsonConvert.SerializeObject(j_param);
                var a = _configuration["DataBaseConfig:key_api:b2c"];
                var token = EncodeHelpers.Encode(data, _configuration["DataBaseConfig:key_api:b2c"]);
                var request = new FormUrlEncodedContent(new[]
                    {
                    new KeyValuePair<string, string>("token",token)
                });
                var url = ReadFile.LoadConfig().API_URL + ReadFile.LoadConfig().Notify_update_status;
                var response = await httpClient.PostAsync(url, request);


                if (response.IsSuccessStatusCode)
                {

                    return 0;
                }

                return 1;
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("apisever:" + ex.ToString());
                return 1;
            }
        }
        public async Task<int> UpdateUser(UserViewModel model)
        {
            try
            {
                string md5_hash = "";
                if (model.Password != null && model.Password.Trim() != "")
                {
                    md5_hash = EncodeHelpers.MD5Hash(model.Password);
                }
                var old_company = model.OldCompanyType != null && model.OldCompanyType.Trim() != "" ? model.OldCompanyType.Split(",") : null;
                var deactive_company = new List<string>();
                if (old_company != null && old_company.Length > 0)
                {
                    foreach (var old in old_company)
                    {
                        if (!model.CompanyType.ToLower().Contains(old.ToLower()))
                        {
                            deactive_company.Add(old);
                        }
                    }
                }
                HttpClient httpClient = new HttpClient();
                var j_param = new UserAPIModel()
                {
                    Id = model.Id,//  Nếu là tạo mới truyền - 1,Nếu cập nhật thì sẽ truyền id của user cập nhật
                    UserName = model.UserName,
                    FullName = model.FullName,
                    Password = md5_hash ?? model.Password,
                    ResetPassword = md5_hash ?? model.Password,
                    Phone = model.Phone ?? "",
                    BirthDay = model.BirthDay,
                    Gender = model.Gender,
                    Email = model.Email ?? "",
                    Avata = model.Avata ?? "",
                    Address = model.Address ?? "",
                    Status = model.Status, // 0: BÌnh thường
                    Note = model.Note ?? "",
                    CreatedBy = model.CreatedBy, // id của user nào tạo
                    ModifiedBy = model.ModifiedBy, // id của user nào update,
                    CompanyType = model.CompanyType, // loại công ty. 0: Travel | 1: Phú Quốc | 2: Đại Việt
                    CompanyDeactiveType = deactive_company.Count > 0 ? string.Join(",", deactive_company) : ""
                };


                var data_product = JsonConvert.SerializeObject(j_param);
                var token = CommonHelper.Encode(data_product, _configuration["DataBaseConfig:key_api:api_manual"]);


                var request = new FormUrlEncodedContent(new[]
                    {
                    new KeyValuePair<string, string>("token",token)
                });
                var url = ReadFile.LoadConfig().API_ADAVIGO_URL_LOGIN + ReadFile.LoadConfig().API_UpdateUser;
                var response = await httpClient.PostAsync(url, request);


                if (response.IsSuccessStatusCode)
                {

                    var stringResult = response.Content.ReadAsStringAsync().Result;

                    var result = JsonConvert.DeserializeObject<UserAPIResponse>(stringResult);
                    model.Id = result.user_id;
                }

                return 1;
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("apisever - UpdateUser:" + ex.ToString());
                return -1;
            }
        }
        public async Task<UserAPIUserDetail> GetByUserDetail(int user_id, string user_name, string email)
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                var j_param = new
                {
                    user_id = user_id,//  Nếu là tạo mới truyền - 1,Nếu cập nhật thì sẽ truyền id của user cập nhật
                    username = user_name,
                    email = email,
                };


                var data_product = JsonConvert.SerializeObject(j_param);
                var token = CommonHelper.Encode(data_product, _configuration["DataBaseConfig:key_api:api_manual"]);


                var request = new FormUrlEncodedContent(new[]
                    {
                    new KeyValuePair<string, string>("token",token)
                });
                var url = ReadFile.LoadConfig().API_ADAVIGO_URL_LOGIN + ReadFile.LoadConfig().API_GetUserDetail;
                var response = await httpClient.PostAsync(url, request);


                if (response.IsSuccessStatusCode)
                {

                    var stringResult = response.Content.ReadAsStringAsync().Result;

                    var result = JsonConvert.DeserializeObject<UserAPIDetail>(stringResult);
                    return result.data[0];
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("apisever - GetByUserDetail:" + ex.ToString());
            }
            return null;

        }
        public async Task<int> ChangePassword(string user_name, string password)
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                var j_param = new
                {
                    username = user_name,
                    password = password,
                };


                var data_product = JsonConvert.SerializeObject(j_param);
                var token = CommonHelper.Encode(data_product, _configuration["DataBaseConfig:key_api:api_manual"]);


                var request = new FormUrlEncodedContent(new[]
                    {
                    new KeyValuePair<string, string>("token",token)
                });
                var url = ReadFile.LoadConfig().API_ADAVIGO_URL_LOGIN + ReadFile.LoadConfig().API_ChangePassword;
                var response = await httpClient.PostAsync(url, request);


                if (response.IsSuccessStatusCode)
                {

                    var stringResult = response.Content.ReadAsStringAsync().Result;
                    var json = JObject.Parse(stringResult);
                    return Convert.ToInt32(json["data"].ToString());
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("apisever - ChangePassword:" + ex.ToString());
            }
            return -1;

        }

        public async Task<BaseObjectQr> GenQrCode(string username)
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                BaseObjectQr result = null;
                var j_param = new Dictionary<string, string>()
                {
                {"username",username },
                };
                var data = JsonConvert.SerializeObject(j_param);
                var a = _configuration["DataBaseConfig:key_api:api_manual"];
                var token = EncodeHelpers.Encode(data, _configuration["DataBaseConfig:key_api:api_manual"]);
                var request = new FormUrlEncodedContent(new[]
                    {
                    new KeyValuePair<string, string>("token",token)
                });
                var url = ReadFile.LoadConfig().API_ADAVIGO_URL_LOGIN + ReadFile.LoadConfig().API_Gen_Qr;
                var response = await httpClient.PostAsync(url, request);
                var stringResult = "";

                if (response.IsSuccessStatusCode)
                {
                    stringResult = response.Content.ReadAsStringAsync().Result;

                    result = JsonConvert.DeserializeObject<BaseObjectQr>(stringResult);
                    return result;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("apisever:" + ex.ToString());
                return null;
            }

        }
        public async Task<string> GetVietQRCode(string account_number, string bank_code, string order_no, double amount)
        {
            try
            {
                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Post, "https://api.vietqr.io/v2/generate");
                request.Headers.Add("x-client-id", "ba09d2bf-7f59-442f-8c26-49a8d48e78d7");
                request.Headers.Add("x-api-key", "a479a45c-47d5-41c1-9f83-990d65cd832a");
                var body = "{ \"accountNo\": \""
                    + account_number
                    + "\", \"accountName\": \"CTCP TM VA DV QUOC TE DAI VIET\", \"acqId\": \""
                    + (bank_code.Length > 6 ? bank_code.Substring(0, 6) : bank_code)
                    + "\", \"addInfo\": \""
                    + order_no
                    + " THANH TOAN\", \"amount\": \"" + Math.Round(amount, 0)
                    + "\", \"template\": \"compact\" }";
                var content = new StringContent(body, null, "application/json");
                request.Content = content;
                var response = await client.SendAsync(request);

                return await response.Content.ReadAsStringAsync();

            }
            catch
            {

            }
            return null;
        }
        public  async Task<string> UploadImageQRBase64(string order_no, string amount, string ImageData,string type)
        {
            string key_token_api = "wVALy5t0tXEgId5yMDNg06OwqpElC9I0sxTtri4JAlXluGipo6kKhv2LoeGQnfnyQlC07veTxb7zVqDVKwLXzS7Ngjh1V3SxWz69";
            string ImagePath = string.Empty;
            string tokenData = string.Empty;
         
            try
            {
                var objimage = StringHelpers.GetImageSrcBase64Object(ImageData);
                var j_param = new Dictionary<string, string> {
                    { "order_no", order_no },
                    { "amount", amount },
                    { "type", type },
                    { "data_file", objimage.ImageData },  
                    { "extend", objimage.ImageExtension }
                };

                using (HttpClient httpClient = new HttpClient())
                {
                    tokenData = CommonHelper.Encode(JsonConvert.SerializeObject(j_param), key_token_api);
                    var contentObj = new { token = tokenData };
                    var content = new StringContent(JsonConvert.SerializeObject(contentObj), Encoding.UTF8, "application/json");
                    var url = ReadFile.LoadConfig().IMAGE_DOMAIN + ReadFile.LoadConfig().upload_QR_payment_order;
                    var result = await httpClient.PostAsync(url, content);
                    dynamic resultContent = Newtonsoft.Json.Linq.JObject.Parse(result.Content.ReadAsStringAsync().Result);
                    if (resultContent.status == 0)
                    {
                        return resultContent.url_path;
                    }
                    else
                    {
                        LogHelper.InsertLogTelegram("UploadImageQRBase64. Result: " + resultContent.status + ". Message: " + resultContent.msg);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("UploadImageQRBase64 - " + ex.Message.ToString() + " Token:" + tokenData);
            }
            return ImagePath;
        }

        public class UserLoginAPIModel
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }
    }
}
