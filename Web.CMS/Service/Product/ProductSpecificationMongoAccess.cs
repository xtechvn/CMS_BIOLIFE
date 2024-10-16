using Azure.Core;
using Entities.ViewModels.Products;
using MongoDB.Driver;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace WEB.CMS.Models.Product
{
    public class ProductSpecificationMongoAccess
    {
        private readonly IConfiguration _configuration;
        private IMongoCollection<ProductSpecificationMongoDbModel> _product_specification_collection;

        public ProductSpecificationMongoAccess(IConfiguration configuration)
        {
            _configuration = configuration;
            //mongodb://adavigolog_writer:adavigolog_2022@103.163.216.42:27017/?authSource=HoanBds
            string url = "mongodb://" + configuration["DataBaseConfig:MongoServer:user"] +
                ":" + configuration["DataBaseConfig:MongoServer:pwd"] +
                "@" + configuration["DataBaseConfig:MongoServer:Host"] +
                ":" + configuration["DataBaseConfig:MongoServer:Port"] +
                "/?authSource=" + configuration["DataBaseConfig:MongoServer:catalog"] + "";

            var client = new MongoClient(url);
            IMongoDatabase db = client.GetDatabase(configuration["DataBaseConfig:MongoServer:catalog"]);

            _product_specification_collection = db.GetCollection<ProductSpecificationMongoDbModel>("ProductSpecification");
        }
        public async Task<string> AddNewAsync(ProductSpecificationMongoDbModel model)
        {
            try
            {
                model.GenID();
                await _product_specification_collection.InsertOneAsync(model);
                return model._id;
            }
            catch (Exception ex)
            {
                Utilities.LogHelper.InsertLogTelegram("ProductSpecificationMongoAccess - AddNewAsync: \nData: aff_model: " + JsonConvert.SerializeObject(model) + ".\n Error: " + ex);
                return null;
            }
        }
        public async Task<string> UpdateAsync(ProductSpecificationMongoDbModel model)
        {
            try
            {
                var filter = Builders<ProductSpecificationMongoDbModel>.Filter;
                var filterDefinition = filter.And(
                    filter.Eq("_id", model._id));
                await _product_specification_collection.FindOneAndReplaceAsync(filterDefinition, model);
                return model._id;
            }
            catch (Exception ex)
            {
                Utilities.LogHelper.InsertLogTelegram("ProductSpecificationMongoAccess - UpdateAsync: \nData: aff_model: " + JsonConvert.SerializeObject(model) + ".\n Error: " + ex);
                return null;
            }
        }
       
      
        public async Task<ProductSpecificationMongoDbModel> GetByID(string id)
        {
            try
            {
                var filter = Builders<ProductSpecificationMongoDbModel>.Filter;
                var filterDefinition = filter.Empty;
                filterDefinition &= Builders<ProductSpecificationMongoDbModel>.Filter.Eq(x => x._id, id); ;
                var model = await _product_specification_collection.Find(filterDefinition).FirstOrDefaultAsync();
                return model;
            }
            catch (Exception ex)
            {
                Utilities.LogHelper.InsertLogTelegram("ProductSpecificationMongoAccess - GetByID Error: " + ex);
                return null;
            }
        }
        public async Task<ProductSpecificationMongoDbModel> GetByNameAndType(int type,string name)
        {
            try
            {
                var filter = Builders<ProductSpecificationMongoDbModel>.Filter;
                var filterDefinition = filter.Empty;
                filterDefinition &= Builders<ProductSpecificationMongoDbModel>.Filter.Eq(x => x.attribute_name, name); ;
                filterDefinition &= Builders<ProductSpecificationMongoDbModel>.Filter.Eq(x => x.attribute_type, type); ;
                var model = await _product_specification_collection.Find(filterDefinition).FirstOrDefaultAsync();
                return model;
            }
            catch (Exception ex)
            {
                Utilities.LogHelper.InsertLogTelegram("ProductSpecificationMongoAccess - GetByID Error: " + ex);
                return null;
            }
        }

        public async Task<List<ProductSpecificationMongoDbModel>> Listing(int type, string name)
        {
            try
            {
                var filter = Builders<ProductSpecificationMongoDbModel>.Filter;
                var filterDefinition = filter.Empty;
                filterDefinition &= Builders<ProductSpecificationMongoDbModel>.Filter.Eq(x => x.attribute_type, type);
                if(name!=null && name.Trim() != "")
                {
                    filterDefinition &= Builders<ProductSpecificationMongoDbModel>.Filter.Regex(x => x.attribute_name, new Regex(name, RegexOptions.IgnoreCase));

                }
                var model =  _product_specification_collection.Find(filterDefinition);
                var result = await model.ToListAsync();
                return result;
            }
            catch (Exception ex)
            {
                Utilities.LogHelper.InsertLogTelegram("ProductSpecificationMongoAccess - Listing Error: " + ex);
                return null;
            }
        }
        


    }
}
