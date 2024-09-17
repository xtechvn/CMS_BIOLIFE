using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Utilities;
using Utilities.Contants;
using WEB.CMS.Models;

namespace WEB.Adavigo.CMS.Service
{
    public class IndentiferService
    {
        private readonly IConfiguration _configuration;
        public IndentiferService (IConfiguration configuration)
        {
            _configuration = configuration;
          
        }
        
        public async Task<string> GetServiceCodeByType(int service_type)
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                /*
                var j_param_2 = new Dictionary<string, string>()
                {
                    {"code_type","2" },
                    {"service_type",service_type.ToString() }
                };
                var j_param = new Dictionary<string, string>()
                {
                   {"key", JsonConvert.SerializeObject(j_param_2)}

                };
                var data = JsonConvert.SerializeObject(j_param);
                */

                string data ="{\"key\":{\"code_type\": 2, \"service_type\":" + service_type + "  }}";
                var a = _configuration["DataBaseConfig:key_api:api_manual"];
                var token = EncodeHelpers.Encode(data, _configuration["DataBaseConfig:key_api:api_manual"]);
                var request = new FormUrlEncodedContent(new[]
                    {
                    new KeyValuePair<string, string>("token",token)
                });
                var url = ReadFile.LoadConfig().API_URL + ReadFile.LoadConfig().Get_Order_no;
                var response = await httpClient.PostAsync(url, request);
                dynamic resultContent_2 = Newtonsoft.Json.Linq.JObject.Parse(response.Content.ReadAsStringAsync().Result);
                var status = (int)resultContent_2.status;
                if (status == (int)ResponseType.SUCCESS)
                {
                    if (resultContent_2.code != null)
                    {
                        return (string)resultContent_2.code;
                    }
                }

            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetServiceCodeByType - IndentiferService: " + ex.ToString());
            }
            return null;

        }
    }
    public static class OrderIndentiferService
    {
        
        public static bool IsOrderManual(string orderNo)
        {
            try
            {
                if (   orderNo.StartsWith("O")
                    || orderNo.StartsWith("A")
                    || orderNo.StartsWith("P")
                    || orderNo.StartsWith("D")
                    )
                {
                    return true;
                }

            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("IsOrderManual - OrderIndentiferService: " + ex.ToString());
            }
            return false;

        }
    }
}
