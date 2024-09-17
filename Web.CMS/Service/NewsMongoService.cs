using Entities.ViewModels.News;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WEB.CMS.Service.News
{
    public class NewsMongoService
    {
        private readonly IConfiguration configuration;
        private IMongoCollection<NewsViewCount> newsmongoCollection;
        public NewsMongoService(IConfiguration _Configuration)
        {
            configuration = _Configuration;

            var client = new MongoClient("mongodb://" + configuration["DataBaseConfig:MongoServer:Host"] + "");
            IMongoDatabase db = client.GetDatabase(configuration["DataBaseConfig:MongoServer:catalog"]);
            this.newsmongoCollection = db.GetCollection<NewsViewCount>("ArticlePageView");

        }
        public async Task<string> AddNewOrReplace(NewsViewCount model)
        {
            try
            {
                var filter = Builders<NewsViewCount>.Filter;
                var filterDefinition = filter.Empty;
                filterDefinition &= Builders<NewsViewCount>.Filter.Eq(x => x.articleID,model.articleID); 
                var exists_model = await newsmongoCollection.Find(filterDefinition).FirstOrDefaultAsync();
                if(exists_model!=null && exists_model.articleID == model.articleID)
                {
                    exists_model.pageview = model.pageview;
                    await newsmongoCollection.FindOneAndReplaceAsync(filterDefinition, exists_model);
                    return exists_model._id;
                }
                else
                {
                    model.GenID();
                    await newsmongoCollection.InsertOneAsync(model);
                    return model._id;
                }
            }
            catch (Exception ex)
            {
                Utilities.LogHelper.InsertLogTelegram("addNew - AnswerSurvery: " + ex);
                return null;
            }
        }
        public async Task<List<NewsViewCount>> GetMostViewedArticle()
        {
            try
            {
                var filter = Builders<NewsViewCount>.Filter;
                var filterDefinition = filter.Empty;
                var list = await newsmongoCollection.Find(filterDefinition).SortByDescending(x => x.pageview).ToListAsync();
                if(list!=null && list.Count > 0)
                {
                    if (list.Count < 10) return list;
                    else return list.Skip(0).Take(10).ToList();
                }

            }
            catch (Exception ex)
            {
                Utilities.LogHelper.InsertLogTelegram("GetMostViewedArticle - AnswerSurvery: " + ex);
            }
            return null;
        }

        public async Task<List<NewsViewCount>> GetListViewedArticle(List<long> article_id)
        {
            try
            {
                var filter = Builders<NewsViewCount>.Filter;
                var filterDefinition = filter.Empty;
                filterDefinition &= Builders<NewsViewCount>.Filter.In(x => x.articleID, article_id);
                var list = await newsmongoCollection.Find(filterDefinition).ToListAsync();
                return list;
            }
            catch (Exception ex)
            {
                Utilities.LogHelper.InsertLogTelegram("GetListViewedArticle - NewsMongoService: " + ex);
                return null;
            }
        }
    }
}
