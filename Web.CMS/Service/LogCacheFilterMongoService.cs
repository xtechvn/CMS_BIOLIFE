using Entities.ViewModels.CustomerManager;
using Entities.ViewModels.Mongo;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using Utilities;
using WEB.Adavigo.CMS.Service;

namespace WEB.CMS.Service
{
    public class LogCacheFilterMongoService
    {
        private readonly IConfiguration configuration;
        public LogCacheFilterMongoService(IConfiguration _configuration)
        {
            configuration = _configuration;

        }
        public async Task<long> InsertLogCache(CustomerManagerViewSearchModel model)
        {
            try
            {
                string url = "mongodb://" + configuration["DataBaseConfig:MongoServer:user"] + ":" + configuration["DataBaseConfig:MongoServer:pwd"] + "@" + configuration["DataBaseConfig:MongoServer:Host"] + ":" + configuration["DataBaseConfig:MongoServer:Port"] + "/" + configuration["DataBaseConfig:MongoServer:catalog_log"];
                var client = new MongoClient(url);

                IMongoDatabase db = client.GetDatabase(configuration["DataBaseConfig:MongoServer:catalog_log"]);
                CustomerManagerViewSearchModel log = new CustomerManagerViewSearchModel()
                {
                    _id = ObjectId.GenerateNewId().ToString(),
                    MaKH = model.MaKH,
                    CreatedBy = model.CreatedBy,
                    UserId = model.UserId,
                    TenKH = model.TenKH,
                    Email = model.Email,
                    Phone = model.Phone,
                    AgencyType = model.AgencyType,
                    ClientType = model.ClientType,
                    PermissionType = model.PermissionType,
                    PageIndex = model.PageIndex,
                    PageSize = model.PageSize,
                    CreateDate = model.CreateDate,
                    EndDate = model.EndDate,
                    MinAmount = model.MinAmount,
                    MaxAmount = model.MaxAmount,
                    SalerPermission = model.SalerPermission,
                    CacheName = model.CacheName,

                };
                IMongoCollection<CustomerManagerViewSearchModel> affCollection = db.GetCollection<CustomerManagerViewSearchModel>(configuration["DataBaseConfig:MongoServer:Cache_Filter_KH"]);


                await affCollection.InsertOneAsync(log);


                return 1;
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("InsertLogCache - LogCacheFilterMongoService: " + ex.Message);
            }
            return 0;
        }
        public List<CustomerManagerViewSearchModel> GetListLogCache(string name,string _id)
        {
            var listLog = new List<CustomerManagerViewSearchModel>();
            try
            {
                var db = MongodbService.GetDatabase();


                var collection = db.GetCollection<CustomerManagerViewSearchModel>(configuration["DataBaseConfig:MongoServer:Cache_Filter_KH"]);
                var filter = Builders<CustomerManagerViewSearchModel>.Filter.Empty;

                if (name != null)
                {
                    filter &= Builders<CustomerManagerViewSearchModel>.Filter.Where(s => s.CacheName.ToUpper().Contains(name.Trim().ToUpper()));
                }
                if (_id != null)
                {
                    filter &= Builders<CustomerManagerViewSearchModel>.Filter.Where(s => s._id.ToUpper().Contains(_id.Trim().ToUpper()));
                }

                var S = Builders<CustomerManagerViewSearchModel>.Sort.Descending("_id");


                listLog = collection.Find(filter).Sort(S).ToList();

            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetListLogCache - LogCacheFilterMongoService. " + JsonConvert.SerializeObject(ex));
            }
            return listLog;
        }
    }
}
