using Azure.Core;
using Entities.ViewModels.Products;
using MongoDB.Driver;
using Nest;
using Newtonsoft.Json;
using System.Collections.Concurrent;
using System.Reflection;
using System.Text.RegularExpressions;
using Utilities;
using Utilities.Contants;
using Utilities.Contants.ProductV2;

namespace WEB.CMS.Models.Product
{
    public class ProductDetailMongoAccess
    {
        private readonly IConfiguration _configuration;
        private IMongoCollection<ProductMongoDbModel> _productDetailCollection;

        public ProductDetailMongoAccess(IConfiguration configuration)
        {
            _configuration = configuration;
            //mongodb://adavigolog_writer:adavigolog_2022@103.163.216.42:27017/?authSource=HoanBds
            string url = "mongodb://"+ configuration["DataBaseConfig:MongoServer:user"] + 
                ":" + configuration["DataBaseConfig:MongoServer:pwd"] +
                "@" + configuration["DataBaseConfig:MongoServer:Host"] +
                ":" + configuration["DataBaseConfig:MongoServer:Port"] + 
                "/?authSource=" + configuration["DataBaseConfig:MongoServer:catalog"] + "";

            var client = new MongoClient(url);
            IMongoDatabase db = client.GetDatabase(configuration["DataBaseConfig:MongoServer:catalog"]);
            _productDetailCollection = db.GetCollection<ProductMongoDbModel>("ProductDetail");
        }
        public async Task<string> AddNewAsync(ProductMongoDbModel model)
        {
            try
            {
                model.GenID();
                await _productDetailCollection.InsertOneAsync(model);
                return model._id;
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("AddNewAsync - ProductDetailMongoAccess: " + ex.ToString());
                return null;
            }
        }
        public async Task<string> UpdateAsync(ProductMongoDbModel model)
        {
            try
            {
                var filter = Builders<ProductMongoDbModel>.Filter;
                var filterDefinition = filter.And(
                    filter.Eq("_id", model._id));
                await _productDetailCollection.FindOneAndReplaceAsync(filterDefinition, model);
                return model._id;
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("UpdateAsync - ProductDetailMongoAccess: " + ex.ToString());
                return null;
            }
        }
       
      
        public async Task<ProductMongoDbModel> GetByID(string id)
        {
            try
            {
                var filter = Builders<ProductMongoDbModel>.Filter;
                var filterDefinition = filter.Empty;
                filterDefinition &= Builders<ProductMongoDbModel>.Filter.Eq(x => x._id, id); ;
                var model = await _productDetailCollection.Find(filterDefinition).FirstOrDefaultAsync();
                return model;
            }
            catch (Exception ex)
            {
                Utilities.LogHelper.InsertLogTelegram("ProductDetailMongoAccess - GetByID Error: " + ex);
                return null;
            }
        }
     
        public async Task<List<ProductMongoDbModel>> Listing(string keyword = "", int group_id = -1, int page_index = 1, int page_size = 10)
        {
            try
            {
                var filter = Builders<ProductMongoDbModel>.Filter;
                var filterDefinition = filter.Empty;
                filterDefinition &= Builders<ProductMongoDbModel>.Filter.Regex(x => x.name, new Regex(keyword.ToLower().Trim(), RegexOptions.IgnoreCase));
                filterDefinition &= Builders<ProductMongoDbModel>.Filter.Eq(x => x.parent_product_id, "");
                if (group_id > 0)
                {
                    filterDefinition &= Builders<ProductMongoDbModel>.Filter.Regex(x => x.group_product_id, group_id.ToString());
                }
                var sort_filter = Builders<ProductMongoDbModel>.Sort;
                var sort_filter_definition = sort_filter.Descending(x=>x.updated_last);
                var model =  _productDetailCollection.Find(filterDefinition);
                model.Options.Skip = page_index < 1 ? 0 : (page_index - 1) * page_size;
                model.Options.Limit = page_size;
                var result = await model.ToListAsync();
                return result;
            }
            catch (Exception ex)
            {
                Utilities.LogHelper.InsertLogTelegram("ProductDetailMongoAccess - Listing Error: " + ex);
                return null;
            }
        }
        public async Task<List<ProductMongoDbModel>> SubListing(string parent_id)
        {
            try
            {
                var filter = Builders<ProductMongoDbModel>.Filter;
                var filterDefinition = filter.Empty;
                filterDefinition &= Builders<ProductMongoDbModel>.Filter.Eq(x => x.parent_product_id, parent_id);
                filterDefinition &= Builders<ProductMongoDbModel>.Filter.Eq(x => x.status, (int)ProductStatus.ACTIVE); ;

                var model = _productDetailCollection.Find(filterDefinition);
                var result = await model.ToListAsync();
                return result;
            }
            catch (Exception ex)
            {
                Utilities.LogHelper.InsertLogTelegram("ProductDetailMongoAccess - SubListing Error: " + ex);
                return null;
            }
        }
        public async Task<List<ProductMongoDbModel>> SubListing(IEnumerable<string> parent_id)
        {
            try
            {
                var filter = Builders<ProductMongoDbModel>.Filter;
                var filterDefinition = filter.Empty;
                filterDefinition &= Builders<ProductMongoDbModel>.Filter.Eq(x => x.status, (int)ProductStatus.ACTIVE); ;
                filterDefinition &= Builders<ProductMongoDbModel>.Filter.In(x=>x.parent_product_id, parent_id);

                var model = _productDetailCollection.Find(filterDefinition);
                var result = await model.ToListAsync();
                return result;
            }
            catch (Exception ex)
            {
                Utilities.LogHelper.InsertLogTelegram("ProductDetailMongoAccess - SubListing Error: " + ex);
                return null;
            }
        }

        public async Task<string> DeactiveByParentId(string id)
        {
            try
            {
                var filter = Builders<ProductMongoDbModel>.Filter;
                var filterDefinition = filter.Empty;
                filterDefinition &= Builders<ProductMongoDbModel>.Filter.Eq(x => x.parent_product_id, id);
                var update = Builders<ProductMongoDbModel>.Update.Set(x => x.status, (int)ProductStatus.DEACTIVE);

                var updated_item = await _productDetailCollection.UpdateManyAsync(filterDefinition, update);
                return id;
            }
            catch (Exception ex)
            {
                Utilities.LogHelper.InsertLogTelegram("ProductDetailMongoAccess - DeactiveByParentId Error: " + ex);
            }
            return null;

        }
        public async Task<List<ProductMongoDbModel>> GetListByIds(string ids)
        {
            try
            {
                var filter = Builders<ProductMongoDbModel>.Filter;
                var filterDefinition = filter.Empty;
                filterDefinition &= Builders<ProductMongoDbModel>.Filter.Where(x => ids.Contains(x._id));

                var model = _productDetailCollection.Find(filterDefinition);
                var result = await model.ToListAsync();
                return result;
            }
            catch (Exception ex)
            {
                Utilities.LogHelper.InsertLogTelegram("ProductDetailMongoAccess - GetListByIds Error: " + ex);
            }
            return null;

        }

        public async Task<string> Delete(string id)
        {
            try
            {
                var filter = Builders<ProductMongoDbModel>.Filter;
                var filterDefinition = filter.Empty;
                filterDefinition &= Builders<ProductMongoDbModel>.Filter.Eq(x => x._id, id);

                var updated_item = await _productDetailCollection.DeleteOneAsync(filterDefinition);
                return id;
            }
            catch (Exception ex)
            {
                Utilities.LogHelper.InsertLogTelegram("ProductDetailMongoAccess - DeactiveByParentId Error: " + ex);
            }
            return null;

        }
        public async Task<string> DeleteByParentId(string id)
        {
            try
            {
                var filter = Builders<ProductMongoDbModel>.Filter;
                var filterDefinition = filter.Empty;
                filterDefinition &= Builders<ProductMongoDbModel>.Filter.Eq(x => x.parent_product_id, id);

                var updated_item = await _productDetailCollection.DeleteManyAsync(filterDefinition);
                return id;
            }
            catch (Exception ex)
            {
                Utilities.LogHelper.InsertLogTelegram("ProductDetailMongoAccess - DeactiveByParentId Error: " + ex);
            }
            return null;

        }
        public async Task<string> DeleteInactiveByParentId(string id)
        {
            try
            {
                var filter = Builders<ProductMongoDbModel>.Filter;
                var filterDefinition = filter.Empty;
                filterDefinition &= Builders<ProductMongoDbModel>.Filter.Eq(x => x.parent_product_id, id);
                filterDefinition &= Builders<ProductMongoDbModel>.Filter.Eq(x => x.status, (int)ProductStatus.DEACTIVE);

                var updated_item = await _productDetailCollection.DeleteManyAsync(filterDefinition);
                return id;
            }
            catch (Exception ex)
            {
                Utilities.LogHelper.InsertLogTelegram("ProductDetailMongoAccess - DeactiveByParentId Error: " + ex);
            }
            return null;

        }
    }
}
