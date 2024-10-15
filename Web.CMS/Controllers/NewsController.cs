using Entities.ViewModels;
using Entities.ViewModels.News;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Utilities;
using Utilities.Common;
using Utilities.Contants;
using WEB.CMS.Customize;
using WEB.CMS.Models;
using WEB.CMS.RabitMQ;
using WEB.CMS.Service.News;

namespace WEB.CMS.Controllers
{
    [CustomAuthorize]
    public class NewsController : Controller
    {
        private const int NEWS_CATEGORY_ID = 39;
        private const int VIDEO_NEWS_CATEGORY_ID = 1;
        private readonly IGroupProductRepository _GroupProductRepository;
        private readonly IArticleRepository _ArticleRepository;
        private readonly IUserRepository _UserRepository;
        private readonly ICommonRepository _CommonRepository;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        private readonly IConfiguration _configuration;
        private readonly WorkQueueClient work_queue;

        public NewsController(IConfiguration configuration, IArticleRepository articleRepository, IUserRepository userRepository, ICommonRepository commonRepository, IWebHostEnvironment hostEnvironment,
            IGroupProductRepository groupProductRepository)
        {
            _ArticleRepository = articleRepository;
            _CommonRepository = commonRepository;
            _UserRepository = userRepository;
            _WebHostEnvironment = hostEnvironment;
            _configuration = configuration;
            _GroupProductRepository = groupProductRepository;
            work_queue = new WorkQueueClient(configuration);


        }

        public async Task<IActionResult> Index()
        {
            var NEWS_CATEGORY_ID = Convert.ToInt32(_configuration["Config:default_news_root_group"]);
            ViewBag.ListArticleStatus = await _CommonRepository.GetAllCodeByType(AllCodeType.ARTICLE_STATUS);
            ViewBag.StringTreeViewCate = await _GroupProductRepository.GetListTreeViewCheckBox(NEWS_CATEGORY_ID, -1);
            ViewBag.ListAuthor = await _UserRepository.GetUserSuggestionList(string.Empty);
            return View();
        }

        /// <summary>
        /// Search News
        /// </summary>
        /// <param name="searchModel"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Search(ArticleSearchModel searchModel, int currentPage = 1, int pageSize = 20)
        {
            var model = new GenericViewModel<ArticleViewModel>();
            try
            {
                model = _ArticleRepository.GetPagingList(searchModel, currentPage, pageSize);
                ViewBag.ListID = (model != null && model.ListData != null && model.ListData.Select(x => x.Id).ToList() != null && model.ListData.Select(x => x.Id).ToList().Count > 0) ? JsonConvert.SerializeObject(model.ListData.Select(x => x.Id).ToList()) : "";
            }
            catch
            {

            }
            return PartialView(model);
        }

        public async Task<IActionResult> Detail(long Id)
        {
            var model = new ArticleModel();
            var size_img = ReadFile.LoadConfig().SIZE_IMG;
            ViewBag.size_img = size_img;
            if (Id > 0)
            {
                model = await _ArticleRepository.GetArticleDetail(Id);
            }
            else
            {
                model.Status = ArticleStatus.SAVE;
            }
            var NEWS_CATEGORY_ID = Convert.ToInt32(_configuration["Config:default_news_root_group"]);
            ViewBag.StringTreeViewCate = await _GroupProductRepository.GetListTreeViewCheckBox(NEWS_CATEGORY_ID, -1, model.Categories);
            ViewBag.StringTreeViewMainCate = await _GroupProductRepository. GetListTreeViewSelect(NEWS_CATEGORY_ID, -1, model.MainCategoryId);
            return View(model);
        }

        public async Task<string> GetSuggestionTag(string name)
        {
            try
            {
                var tagList = await _ArticleRepository.GetSuggestionTag(name);
                return JsonConvert.SerializeObject(tagList);
            }
            catch
            {
                return null;
            }
        }

        public async Task<IActionResult> RelationArticle(long Id)
        {
            var NEWS_CATEGORY_ID = Convert.ToInt32(_configuration["Config:default_news_root_group"]);
            ViewBag.StringTreeViewCate = await _GroupProductRepository.GetListTreeViewCheckBox(NEWS_CATEGORY_ID, -1);
            ViewBag.ListAuthor = await _UserRepository.GetUserSuggestionList(string.Empty);
            return PartialView();
        }

        [HttpPost]
        public IActionResult RelationSearch(ArticleSearchModel searchModel, int currentPage = 1, int pageSize = 10)
        {
            var model = new GenericViewModel<ArticleViewModel>();
            try
            {
                model = _ArticleRepository.GetPagingList(searchModel, currentPage, pageSize);
            }
            catch
            {

            }
            return PartialView(model);
        }

        [HttpPost]
        public async Task<IActionResult> UpSert([FromBody] object data)
        {
            try
            {

                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };

                var model = JsonConvert.DeserializeObject<ArticleModel>(data.ToString(), settings);

                var NEWS_CATEGORY_ID = Convert.ToInt32(_configuration["Config:default_news_root_group"]);
                //if (await _GroupProductRepository.IsGroupHeader(model.Categories)) model.Categories.Add(NEWS_CATEGORY_ID);

                if (model != null && HttpContext.User.FindFirst(ClaimTypes.NameIdentifier) != null)
                {
                    model.AuthorId = int.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                }

                model.Body = ArticleHelper.HighLightLinkTag(model.Body);
                if (model.Body == null || model.Body.Trim() == "" || model.Title == null || model.Title.Trim() == "" || model.Lead == null || model.Lead.Trim() == "")
                {
                    return new JsonResult(new
                    {
                        isSuccess = false,
                        message = "Phần Tiêu đề, Mô tả và Nội dung bài viết không được để trống"
                    });
                }
                if (model.Lead.Length >= 400)
                {
                    return new JsonResult(new
                    {
                        isSuccess = false,
                        message = "Phần Tiêu đề không được vượt quá 400 ký tự"
                    });
                }
                var articleId = await _ArticleRepository.SaveArticle(model);

                if (articleId > 0)
                {
                    // clear cache article
                    var strCategories = string.Empty;
                    if (model.Categories != null && model.Categories.Count > 0)
                        strCategories = string.Join(",", model.Categories);

                    ClearCacheArticle(articleId, strCategories);

                    // Tạo message để push vào queue
                    var j_param = new Dictionary<string, object>
                            {
                                { "store_name", "SP_GetAllArticle" },
                                { "index_es", "es_biolife_sp_get_article" },
                                {"project_type", Convert.ToInt16(ProjectType.BIOLIFE) },
                                  {"id" , articleId }
                            };
                    var _data_push = JsonConvert.SerializeObject(j_param);
                    // Push message vào queue
                    var response_queue = work_queue.InsertQueueSimple(_data_push, QueueName.queue_app_push);

                    return new JsonResult(new
                    {
                        isSuccess = true,
                        message = "Cập nhật thành công",
                        dataId = articleId,
                        
                    });
                }
                else
                {
                    return new JsonResult(new
                    {
                        isSuccess = false,
                        message = "Cập nhật thất bại"
                    });
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("UpSert - NewsController: " + ex);
                return new JsonResult(new
                {
                    isSuccess = false,
                    message = ex.Message.ToString()
                });
            }
        }

        public async Task<IActionResult> ChangeArticleStatus(long Id, int articleStatus)
        {
            try
            {
                var _ActionName = string.Empty;

                switch (articleStatus)
                {
                    case ArticleStatus.PUBLISH:
                        _ActionName = "Đăng bài viết";
                        break;

                    case ArticleStatus.REMOVE:
                        _ActionName = "Hạ bài viết";
                        break;
                }

                var rs = await _ArticleRepository.ChangeArticleStatus(Id, articleStatus);

                if (rs > 0)
                {
                    //  clear cache article
                    var Categories = await _ArticleRepository.GetArticleCategoryIdList(Id);
                    ClearCacheArticle(Id, string.Join(",", Categories));

                    // Tạo message để push vào queue
                    var j_param = new Dictionary<string, object>
                            {
                                { "store_name", "SP_GetAllArticle" },
                                { "index_es", "es_biolife_sp_get_article" },
                                {"project_type", Convert.ToInt16(ProjectType.BIOLIFE) },
                                  {"id" , Id }
                            };
                    var _data_push = JsonConvert.SerializeObject(j_param);
                    // Push message vào queue
                    var response_queue = work_queue.InsertQueueSimple(_data_push, QueueName.queue_app_push);

                    return new JsonResult(new
                    {
                        isSuccess = true,
                        message = _ActionName + " thành công",
                        dataId = Id,
                        //queueResponse = response_queue
                    });
                }
                else
                {
                    return new JsonResult(new
                    {
                        isSuccess = false,
                        message = _ActionName + " thất bại"
                    });
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("ChangeArticleStatus - NewsController: " + ex);
                return new JsonResult(new
                {
                    isSuccess = false,
                    message = ex.Message.ToString()
                });
            }
        }

        public async Task<IActionResult> DeleteArticle(long Id)
        {
            try
            {
                var Categories = await _ArticleRepository.GetArticleCategoryIdList(Id);
                var rs = await _ArticleRepository.DeleteArticle(Id);

                if (rs > 0)
                {
                    //  clear cache article
                    ClearCacheArticle(Id, string.Join(",", Categories));
                    //// Tạo message để push vào queue
                    //var j_param = new Dictionary<string, object>
                    //        {
                    //            { "store_name", "SP_GetAllArticle" },
                    //            { "index_es", "es_biolife_sp_get_article" },
                    //            {"project_type", Convert.ToInt16(ProjectType.BIOLIFE) },
                    //              {"id" , "-1" }
                    //        };
                    //var _data_push = JsonConvert.SerializeObject(j_param);
                    //// Push message vào queue
                    //var response_queue = work_queue.InsertQueueSimple(_data_push, QueueName.queue_app_push);

                    return new JsonResult(new
                    {
                        isSuccess = true,
                        message = "Xóa bài viết thành công",
                        dataId = Id
                    });
                }
                else
                {
                    return new JsonResult(new
                    {
                        isSuccess = false,
                        message = "Xóa bài viết thất bại"
                    });
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("DeleteArticle - NewsController: " + ex);
                return new JsonResult(new
                {
                    isSuccess = false,
                    message = ex.Message.ToString()
                });
            }
        }

        public async Task ClearCacheArticle(long articleId, string ArrCategoryId)
        {
            string token = string.Empty;
            try
            {
                var api = new APIService2(_configuration);
                var apiPrefix = ReadFile.LoadConfig().API_URL + ReadFile.LoadConfig().API_SYNC_ARTICLE;
                var key_token_api = ReadFile.LoadConfig().KEY_TOKEN_API;
                HttpClient httpClient = new HttpClient();
                var j_param = new Dictionary<string, string> {
                    { "article_id", articleId.ToString() },
                    { "category_id",ArrCategoryId }
                };
                api.POST(ReadFile.LoadConfig().API_SYNC_ARTICLE, j_param);
                var category_list_id = ArrCategoryId.Split(",");
                foreach (var item in category_list_id)
                {
                    var j_param2 = new Dictionary<string, string> {
                        { "category_id", item },
                        { "skip","1" },
                        { "take","10" }
                    };
                    api.POST(_configuration["API:Api_get_list_by_categoryid_order"], j_param2);
                    api.POST(_configuration["API:Api_get_list_by_categoryid"], j_param2);
                }


            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("ClearCacheArticle - " + ex.ToString() + " Token:" + token);
            }
        }
        [HttpPost]
        public async Task<List<NewsViewCount>> GetPageViewByList(List<long> article_id)
        {
            try
            {
                NewsMongoService news_services = new NewsMongoService(_configuration);
                return await news_services.GetListViewedArticle(article_id);
            }
            catch
            {

            }
            return null;
        }

    }
}
