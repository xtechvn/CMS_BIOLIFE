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
    public class SupplierRedisService
    {
        private readonly IConfiguration _configuration;
        private readonly RedisConn _redisService;

        public SupplierRedisService(IConfiguration configuration)
        {
            _configuration = configuration;
            _redisService = new RedisConn(_configuration);
            _redisService.Connect();
        }
        public async Task<bool> SetSuplier(List<Supplier> data)
        {

            try
            {
                int db_index = Convert.ToInt32(_configuration["Redis:Database:db_common"]);
                string cache_name = CacheName.SUPPLIER;
                if (data != null && data.Count > 0)
                {
                    var strDataCache = JsonConvert.SerializeObject(data);
                    _redisService.Set(cache_name,strDataCache, db_index);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Utilities.LogHelper.InsertLogTelegram("SetSuplier - SupplierRedisService: " + ex);
            }
            return false;        
        }
        public async Task<List<Supplier>> SearchSuplier(string txt_search)
        {

            try
            {
                int db_index = Convert.ToInt32(_configuration["Redis:Database:db_common"]);
                string cache_name = CacheName.SUPPLIER;
                var strDataCache = _redisService.Get(cache_name, db_index);
                if (txt_search == null) txt_search = "";
                if (!string.IsNullOrEmpty(strDataCache))
                {
                    var obj_lst = JsonConvert.DeserializeObject<List<Supplier>>(strDataCache);
                    if(txt_search==null || txt_search.Trim() == "")
                    {
                        return obj_lst;
                    }
                    if(obj_lst!=null && obj_lst.Count > 0)
                    {
                       var data = obj_lst.Where(s =>
                                            s.FullName.Trim().ToLower().Contains(txt_search.Trim().ToLower())
                                            || (!string.IsNullOrEmpty(s.ShortName) && s.ShortName.Trim().ToLower().Contains(txt_search.Trim().ToLower()))
                                            || (!string.IsNullOrEmpty(s.Email) && s.Email.Trim().ToLower().Contains(txt_search.Trim().ToLower()))
                                            || (!string.IsNullOrEmpty(s.Phone) && s.Phone.Trim().ToLower().Contains(txt_search.Trim().ToLower()))
                                            ).ToList();
                        return data;

                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.LogHelper.InsertLogTelegram("SearchSuplier - SupplierRedisService: " + ex);
            }
            return new List<Supplier>();
        }
    }
}
