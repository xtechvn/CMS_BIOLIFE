using Entities.ViewModels.Mongo;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using Utilities;

namespace WEB.Adavigo.CMS.Service
{
    public class LogActionMongoService
    {
        private readonly IConfiguration configuration;
        public LogActionMongoService(IConfiguration _configuration)
        {
            configuration = _configuration;

        }
        public async Task<long> InsertLog(LogActionModel model)
        {
            try
            {
                string url = "mongodb://" + configuration["DataBaseConfig:MongoServer:user"] + ":" + configuration["DataBaseConfig:MongoServer:pwd"] + "@" + configuration["DataBaseConfig:MongoServer:Host"] + ":" + configuration["DataBaseConfig:MongoServer:Port"] + "/" + configuration["DataBaseConfig:MongoServer:catalog_log"];
                var client = new MongoClient(url);

                IMongoDatabase db = client.GetDatabase(configuration["DataBaseConfig:MongoServer:catalog_log"]);
                LogActionModel log = new LogActionModel()
                {
                    _id= ObjectId.GenerateNewId().ToString(),
                    LogId = model.LogId,
                    Type = model.Type,
                    Log = model.Log,
                    Note = model.Note,
                    CreatedUserName=model.CreatedUserName,
                    CreatedTime = DateTime.Now

                };
                IMongoCollection<LogActionModel> affCollection = db.GetCollection<LogActionModel>(configuration["DataBaseConfig:MongoServer:LogAction_collection"]);

               
                    await affCollection.InsertOneAsync(log);
                

                return model.LogId;
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("PushLog - LogActionMongoService: " + ex.Message);
            }
            return 0;
        }
        public List<LogActionModel> GetListLogActions(LogActionModel searchModel)
        {
            var listLog = new List<LogActionModel>();
            try
            {
                var db = MongodbService.GetDatabase();

     
                var collection = db.GetCollection<LogActionModel>(configuration["DataBaseConfig:MongoServer:LogAction_collection"]);
                var filter = Builders<LogActionModel>.Filter.Empty;


                filter &= Builders<LogActionModel>.Filter.Eq(n => n.Type, searchModel.Type);

                filter &= Builders<LogActionModel>.Filter.Where(s => s.LogId == searchModel.LogId);



                var S = Builders<LogActionModel>.Sort.Descending("_id");


                listLog = collection.Find(filter).Sort(S).ToList();

            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetListLogActions - LogActionMongoService. " + JsonConvert.SerializeObject(ex));
            }
            return listLog;
        }

    }
}
