using Entities.ViewModels.Log;
using MongoDB.Driver;
using Utilities;
using Utilities.Contants;

namespace WEB.CMS.Service.Log
{
    public static class UsersLoggingService
    {
        public static async Task<string> InsertLog(IConfiguration configuration, LogUsersActivityModel log, string document_name)
        {
            try
            {
                var client = new MongoClient("mongodb://" + configuration["DataBaseConfig:MongoServer:Host"] + "");
                IMongoDatabase db = client.GetDatabase(configuration["DataBaseConfig:MongoServer:catalog"]);
                IMongoCollection<LogUsersActivityModel> affCollection = db.GetCollection<LogUsersActivityModel>(document_name);
                var filter = Builders<LogUsersActivityModel>.Filter.Where(x => x.id == log.id);
                var result_document = affCollection.Find(filter).ToList();
                if (result_document != null && result_document.Count > 0)
                {
                    await affCollection.ReplaceOneAsync(filter, log);
                }
                else
                {
                    await affCollection.InsertOneAsync(log);
                }
                return "";
            } catch(Exception ex)
            {
                return ex.ToString();
            }
        }
        public static async Task<string> InsertLogFromAPI(IConfiguration configuration,string j_data_log)
        {
            try
            {
                var client = new MongoClient("mongodb://" + configuration["DataBaseConfig:MongoServer:Host"] + "");
                IMongoDatabase db = client.GetDatabase(configuration["DataBaseConfig:MongoServer:catalog"]);
                LogUsersActivityModel log = new LogUsersActivityModel()
                {
                    user_type = 0,
                    user_id = -1,
                    user_name = "Kerry",
                    log_type = (int)LogActivityType.CHANGE_ORDER_BY_KERRRY,
                    log_date = DateTime.Now,
                    j_data_log = j_data_log,
                };
                IMongoCollection<LogUsersActivityModel> affCollection = db.GetCollection<LogUsersActivityModel>(LogActivityBSONDocuments.API);
                var filter = Builders<LogUsersActivityModel>.Filter.Where(x => x.id == log.id);
                var result_document = affCollection.Find(filter).ToList();
                if (result_document != null && result_document.Count > 0)
                {
                    await affCollection.ReplaceOneAsync(filter, log);
                }
                else
                {
                    await affCollection.InsertOneAsync(log);
                }
                return "";
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram(ex.Message);
                return ex.ToString();
            }
        }
    }
}
