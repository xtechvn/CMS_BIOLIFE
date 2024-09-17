using Entities.ViewModels.Log;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Utilities;
using Utilities.Contants;
using WEB.CMS.Models;
using WEB.CMS.Service.Log;

namespace WEB.CMS.Common
{
    public static class LoggingActivity
    {
        public static async Task AddLog(IConfiguration configuration, int user_id,string user_name, int log_type, string action_log, string j_data_log, string additional_keyword="")
        {
            try
            {
                if (user_id<0||user_name==null||user_name.Trim()==""||log_type<0)
                {
                }
                else
                {
                    var data = new LogUsersActivityModel()
                    {
                        user_type = 0,
                        user_id = user_id, 
                        user_name = user_name,
                        j_data_log = j_data_log, 
                        log_type = log_type, 
                        key_word_search = additional_keyword,
                        log_date=DateTime.Now,
                        action_log= action_log,
                        
                    };
                    await UsersLoggingService.InsertLog(configuration, data, "ActivityLogCMS");
                }
            } catch(Exception ex)
            {
                LogHelper.InsertLogTelegram("AddLog - LogActivity " + ex.ToString());
            }
        }
    }
}
