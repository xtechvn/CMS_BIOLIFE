using Caching.RedisWorker;
using Entities.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities;
using Utilities.Contants;

namespace WEB.Adavigo.CMS.Service
{
    public class ProvinceRedisService
    {
        private readonly IConfiguration _configuration;
        private readonly RedisConn _redisService;

        public ProvinceRedisService(IConfiguration configuration)
        {
            _configuration = configuration;
            _redisService = new RedisConn(_configuration);
            _redisService.Connect();
        }
        public async Task<List<Province>> SearchProvince(string txt_search)
        {

            try
            {
                int db_index = Convert.ToInt32(_configuration["Redis:Database:db_common"]);
                string cache_name = CacheName.PROVINCE;
                var strDataCache = _redisService.Get(cache_name, db_index);
                if (txt_search == null) txt_search = "";
                if (!string.IsNullOrEmpty(strDataCache))
                {
                    var obj_lst = JsonConvert.DeserializeObject<List<Province>>(strDataCache);
                    if(txt_search==null || txt_search.Trim() == "")
                    {
                        return obj_lst;
                    }
                    if(obj_lst!=null && obj_lst.Count > 0)
                    {
                        List<Province> result = obj_lst.Where(x => (x.Status == 0 || x.Status == null) && (CommonHelper.RemoveUnicode(x.Name.ToLower().Replace(" ","")).Contains(CommonHelper.RemoveUnicode(txt_search.Replace(" ", "")).ToLower()) || (x.NameNonUnicode!=null && x.NameNonUnicode.ToLower().Replace(" ", "").Contains(CommonHelper.RemoveUnicode(txt_search.Replace(" ", "")).ToLower())))).ToList();
                        return result;

                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.LogHelper.InsertLogTelegram("SearchProvince - ProvinceRedisService: " + ex);
            }
            return new List<Province>();
        }
        public async Task<bool> SetProvinces(List<Province> list)
        {

            try
            {
                int db_index = Convert.ToInt32(_configuration["Redis:Database:db_common"]);
                string cache_name = CacheName.PROVINCE;
                _redisService.Set(cache_name,JsonConvert.SerializeObject(list), db_index);
                return true;
            }
            catch 
            {
            }
            return false;
        }
        public async Task<Province> GetProvicedById(int id)
        {

            try
            {
                int db_index = Convert.ToInt32(_configuration["Redis:Database:db_common"]);
                string cache_name = CacheName.PROVINCE;
                var strDataCache = _redisService.Get(cache_name, db_index);
                if (!string.IsNullOrEmpty(strDataCache))
                {
                    var obj_lst = JsonConvert.DeserializeObject<List<Province>>(strDataCache);
                    var result = obj_lst.Where(x => (x.Status == 0 || x.Status == null) && x.Id == id).FirstOrDefault();
                    return result;
                }
            }
            catch (Exception ex)
            {
                Utilities.LogHelper.InsertLogTelegram("GetProvicedById - ProvinceRedisService: " + ex);
            }
            return null;
        }
    }
}
