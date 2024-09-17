using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.Common
{
    public static class TimeCorrection
    {
        public static async Task<DateTime> GetCurrentDateTime()
        {
            DateTime result = DateTime.Now.ToUniversalTime();
            try
            {
                HttpClient httpClient = new HttpClient();
                var apiPrefix = "http://worldtimeapi.org/api/timezone/Asia/Bangkok";
                var rs = await httpClient.GetAsync(apiPrefix);
                
                var rs_content = JsonConvert.DeserializeObject<Dictionary<string, string>>(rs.Content.ReadAsStringAsync().Result);
                result = (Convert.ToDateTime(rs_content["datetime"]) != null) ? Convert.ToDateTime(rs_content["datetime"]).ToUniversalTime() : Convert.ToDateTime(rs_content["utc_datetime"]);
            }
            catch (Exception)
            {
            }
            return result;
        }
    }
}
