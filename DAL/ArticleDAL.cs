using DAL.Generic;
using DAL.StoreProcedure;
using Entities.Models;
using Entities.ViewModels;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;
using Utilities.Contants;

namespace DAL
{
    public class ArticleDAL : GenericService<Article>
    {
        private static DbWorker _DbWorker;
        public ArticleDAL(string connection) : base(connection)
        {
            _DbWorker = new DbWorker(connection);
        }

        public DataTable GetPagingList(ArticleSearchModel searchModel, int currentPage, int pageSize)
        {
            try
            {
                DateTime _FromDate = DateTime.MinValue;
                DateTime _ToDate = DateTime.MinValue;
                string _ArrCategoryId = string.Empty;

                if (!string.IsNullOrEmpty(searchModel.FromDate))
                {
                    _FromDate = DateTime.ParseExact(searchModel.FromDate, "d/M/yyyy", null);
                }

                if (!string.IsNullOrEmpty(searchModel.ToDate))
                {
                    _ToDate = DateTime.ParseExact(searchModel.ToDate, "d/M/yyyy", null);
                }

                if (searchModel.ArrCategoryId != null && searchModel.ArrCategoryId.Length > 0)
                {
                    _ArrCategoryId = string.Join(",", searchModel.ArrCategoryId);
                }

                SqlParameter[] objParam = new SqlParameter[10];
                objParam[0] = new SqlParameter("@Title", searchModel.Title ?? string.Empty);
                objParam[1] = new SqlParameter("@ArticleId", searchModel.ArticleId);

                if (_FromDate != DateTime.MinValue)
                    objParam[2] = new SqlParameter("@FromDate", _FromDate);
                else
                    objParam[2] = new SqlParameter("@FromDate", DBNull.Value);

                if (_ToDate != DateTime.MinValue)
                    objParam[3] = new SqlParameter("@ToDate", _ToDate);
                else
                    objParam[3] = new SqlParameter("@ToDate", DBNull.Value);

                objParam[4] = new SqlParameter("@AuthorId", searchModel.AuthorId);
                objParam[5] = new SqlParameter("@Status", searchModel.ArticleStatus);
                objParam[6] = new SqlParameter("@ArrCategoryId", _ArrCategoryId);
                objParam[7] = new SqlParameter("@SearchType", searchModel.SearchType);
                objParam[8] = new SqlParameter("@CurentPage", currentPage);
                objParam[9] = new SqlParameter("@PageSize", pageSize);

                return _DbWorker.GetDataTable(ProcedureConstants.ARTICLE_SEARCH, objParam);
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetPagingList - ArticleDAL: " + ex);
            }
            return null;
        }

        public async Task<long> SaveArticle(ArticleModel model)
        {
            try
            {
                long articleId = model.Id;

                if (model.Id > 0)
                {
                    var entity = await FindAsync(model.Id);

                    entity.Title = model.Title;
                    entity.Lead = model.Lead;
                    entity.Body = model.Body;
                    entity.Image11 = model.Image11 ?? string.Empty;
                    entity.Image169 = model.Image169 ?? string.Empty;
                    entity.Image43 = model.Image43 ?? string.Empty;
                    entity.Status = model.Status;
                    entity.ArticleType = model.ArticleType;
                    entity.ModifiedOn = DateTime.Now;
                    entity.PublishDate = model.PublishDate == DateTime.MinValue ? (DateTime?)null : model.PublishDate;
                    entity.UpTime = model.PublishDate == DateTime.MinValue ? (DateTime?)null : model.PublishDate;
                    entity.DownTime = model.DownTime == DateTime.MinValue ? (DateTime?)null : model.DownTime;
                    entity.Position = (short?)model.Position;
                    await UpdateAsync(entity);
                }
                else
                {
                    var entity = new Article
                    {
                        Title = model.Title,
                        Lead = model.Lead,
                        Body = model.Body,
                        Status = model.Status,
                        Image11 = model.Image11 ?? string.Empty,
                        Image169 = model.Image169 ?? string.Empty,
                        Image43 = model.Image43 ?? string.Empty,
                        ArticleType = model.ArticleType,
                        AuthorId = model.AuthorId,
                        CreatedOn = DateTime.Now,
                        ModifiedOn = DateTime.Now,
                        PublishDate = model.PublishDate == DateTime.MinValue ? DateTime.Now : model.PublishDate,
                        UpTime = model.PublishDate == DateTime.MinValue ? DateTime.Now : model.PublishDate,
                        DownTime = model.DownTime == DateTime.MinValue ? (DateTime?)null : model.DownTime,
                        Position = (short?)model.Position
                    };
                    articleId = await CreateAsync(entity);
                }
                return articleId;
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("SaveArticle - ArticleDAL: " + ex);
                return 0;
            }
        }

        public async Task<ArticleModel> GetArticleDetail(long Id)
        {
            try
            {
                var model = new ArticleModel();
                using (var _DbContext = new EntityDataContext(_connection))
                {
                    using (var transaction = _DbContext.Database.BeginTransaction())
                    {
                        try
                        {
                            var article = await _DbContext.Articles.FindAsync(Id);
                            model = new ArticleModel
                            {
                                Id = article.Id,
                                Title = article.Title,
                                Lead = article.Lead,
                                Body = article.Body,
                                Status = article.Status,
                                ArticleType = article.ArticleType,
                                Image11 = article.Image11,
                                Image43 = article.Image43,
                                Image169 = article.Image169,
                                PublishDate = article.PublishDate ?? DateTime.MinValue,
                                DownTime = article.DownTime ?? DateTime.MinValue,
                                Position = article.Position ?? 0
                            };

                            var TagIds = await _DbContext.ArticleTags.Where(s => s.ArticleId == article.Id).Select(s => s.TagId).ToListAsync();
                            model.Tags = await _DbContext.Tags.Where(s => TagIds.Contains(s.Id)).Select(s => s.TagName).ToListAsync();
                            // get Cate
                            var categoryIds = await _DbContext.ArticleCategories.Where(s => s.ArticleId == article.Id && (s.IsMain == false || s.IsMain == null)).Select(s => (int)s.CategoryId).ToListAsync();
                            model.Categories = categoryIds.Any() ? categoryIds : null;

                            // Lấy thông tin chuyên mục chính
                            var mainCategory = await _DbContext.ArticleCategories
                                .Where(s => s.ArticleId == article.Id && s.IsMain == true) // Chỉ lấy chuyên mục chính
                                .Select(s => (int)s.CategoryId)
                                .FirstOrDefaultAsync();
                            model.MainCategoryId = mainCategory;
                            model.RelatedArticleIds = await _DbContext.ArticleRelateds.Where(s => s.ArticleId == article.Id).Select(s => (long)s.ArticleRelatedId).ToListAsync();

                            if (model.RelatedArticleIds != null && model.RelatedArticleIds.Count > 0)
                            {
                                model.RelatedArticleList = await (from _article in _DbContext.Articles.AsNoTracking()
                                                                  join a in _DbContext.Users.AsNoTracking() on _article.AuthorId equals a.Id into af
                                                                  from _user in af.DefaultIfEmpty()
                                                                  where model.RelatedArticleIds.Contains(_article.Id)
                                                                  select new ArticleRelationModel
                                                                  {
                                                                      Id = _article.Id,
                                                                      Author = _user.UserName,
                                                                      Image = _article.Image11 ?? article.Image43 ?? article.Image169,
                                                                      Title = _article.Title
                                                                  }).ToListAsync();
                            }

                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            LogHelper.InsertLogTelegram("GetArticleDetail - Transaction Rollback - ArticleDAL: " + ex);
                            transaction.Rollback();
                            return null;
                        }
                    }
                }
                return model;
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetArticleDetail - ArticleDAL: " + ex);
                return null;
            }
        }

        public async Task<int> MultipleInsertArticleTag(long ArticleId, List<long> ListTagId)
        {
            try
            {
                using (var _DbContext = new EntityDataContext(_connection))
                {
                    using (var transaction = _DbContext.Database.BeginTransaction())
                    {
                        try
                        {
                            var ExistList = await _DbContext.ArticleTags.Where(s => s.ArticleId == ArticleId).ToListAsync();
                            if (ExistList != null && ExistList.Count > 0)
                            {
                                foreach (var item in ExistList)
                                {
                                    var deleteModel = await _DbContext.ArticleTags.FindAsync(item.Id);
                                    _DbContext.ArticleTags.Remove(deleteModel);
                                    await _DbContext.SaveChangesAsync();
                                }
                            }

                            if (ListTagId != null && ListTagId.Count > 0)
                            {
                                foreach (var item in ListTagId)
                                {
                                    var model = new ArticleTag
                                    {
                                        TagId = item,
                                        ArticleId = ArticleId,
                                        UpdateLast = DateTime.Now
                                    };
                                    await _DbContext.ArticleTags.AddAsync(model);
                                    await _DbContext.SaveChangesAsync();
                                }
                            }

                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            LogHelper.InsertLogTelegram("MultipleInsertArticleTag - Transaction Rollback - ArticleDAL: " + ex);
                            transaction.Rollback();
                            return 0;
                        }
                    }
                }
                return 1;
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("MultipleInsertArticleTag - ArticleDAL: " + ex);
                return 0;
            }
        }

        public async Task<int> MultipleInsertArticleCategory(long ArticleId, List<int> ListCateId, int? MainCategoryId)
        {
            try
            {
                using (var _DbContext = new EntityDataContext(_connection))
                {
                    using (var transaction = _DbContext.Database.BeginTransaction())
                    {
                        try
                        {
                            // Lấy danh sách các chuyên mục hiện tại
                            var ExistList = await _DbContext.ArticleCategories.Where(s => s.ArticleId == ArticleId).ToListAsync();
                            if (ExistList != null && ExistList.Count > 0)
                            {
                                foreach (var item in ExistList)
                                {
                                    var deleteModel = await _DbContext.ArticleCategories.FindAsync(item.Id);
                                    deleteModel.UpdateLast = DateTime.Now;
                                    deleteModel.CategoryId = deleteModel.CategoryId * -1;
                                    deleteModel.ArticleId = deleteModel.ArticleId * -1;
                                    _DbContext.ArticleCategories.Update(deleteModel);
                                    await _DbContext.SaveChangesAsync();
                                }
                            }

                            // Thêm chuyên mục chính nếu có
                            if (MainCategoryId.HasValue)
                            {
                                var mainCategoryModel = new ArticleCategory
                                {
                                    CategoryId = MainCategoryId.Value,
                                    ArticleId = ArticleId,
                                    IsMain = true,
                                    UpdateLast = DateTime.Now
                                };
                                await _DbContext.ArticleCategories.AddAsync(mainCategoryModel);
                                await _DbContext.SaveChangesAsync();
                            }

                            // Thêm các chuyên mục phụ còn lại
                            if (ListCateId != null && ListCateId.Count > 0)
                            {
                                foreach (var item in ListCateId)
                                {
                                    var model = new ArticleCategory
                                    {
                                        CategoryId = item,
                                        ArticleId = ArticleId,
                                        IsMain = false,
                                        UpdateLast = DateTime.Now
                                    };
                                    await _DbContext.ArticleCategories.AddAsync(model);
                                    await _DbContext.SaveChangesAsync();
                                }
                            }

                            // Xóa chuyên mục phụ nếu trùng với chuyên mục chính
                            if (MainCategoryId.HasValue)
                            {
                                var duplicateCategories = await _DbContext.ArticleCategories
                                    .Where(s => s.ArticleId == ArticleId && s.CategoryId == MainCategoryId.Value && s.IsMain == false)
                                    .ToListAsync();

                                if (duplicateCategories != null && duplicateCategories.Count > 0)
                                {
                                    _DbContext.ArticleCategories.RemoveRange(duplicateCategories);
                                    await _DbContext.SaveChangesAsync();
                                }
                            }

                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            LogHelper.InsertLogTelegram("MultipleInsertArticleCategory - Transaction Rollback - ArticleDAL: " + ex);
                            transaction.Rollback();
                            return 0;
                        }
                    }
                }
                return 1;
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("MultipleInsertArticleCategory - ArticleDAL: " + ex);
                return 0;
            }
        }



        public async Task<int> MultipleInsertArticleRelation(long ArticleId, List<long> ListArticleId)
        {
            try
            {
                using (var _DbContext = new EntityDataContext(_connection))
                {
                    using (var transaction = _DbContext.Database.BeginTransaction())
                    {
                        try
                        {
                            var ExistList = await _DbContext.ArticleRelateds.Where(s => s.ArticleId == ArticleId).ToListAsync();
                            if (ExistList != null && ExistList.Count > 0)
                            {
                                foreach (var item in ExistList)
                                {
                                    var deleteModel = await _DbContext.ArticleRelateds.FindAsync(item.Id);
                                    _DbContext.ArticleRelateds.Remove(deleteModel);
                                    await _DbContext.SaveChangesAsync();
                                }
                            }

                            if (ListArticleId != null && ListArticleId.Count > 0)
                            {
                                foreach (var item in ListArticleId)
                                {
                                    var model = new ArticleRelated
                                    {
                                        ArticleRelatedId = item,
                                        ArticleId = ArticleId,
                                        UpdateLast = DateTime.Now
                                    };
                                    await _DbContext.ArticleRelateds.AddAsync(model);
                                    await _DbContext.SaveChangesAsync();
                                }
                            }

                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            LogHelper.InsertLogTelegram("MultipleInsertArticleRelation - Transaction Rollback - ArticleDAL: " + ex);
                            transaction.Rollback();
                            return 0;
                        }
                    }
                }
                return 1;
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("MultipleInsertArticleRelation - ArticleDAL: " + ex);
                return 0;
            }
        }

        public async Task<long> DeleteArticle(long Id)
        {
            try
            {
                using (var _DbContext = new EntityDataContext(_connection))
                {
                    using (var transaction = _DbContext.Database.BeginTransaction())
                    {
                        try
                        {
                            var ExistCategory = await _DbContext.ArticleCategories.Where(s => s.ArticleId == Id).ToListAsync();
                            if (ExistCategory != null && ExistCategory.Count > 0)
                            {
                                foreach (var item in ExistCategory)
                                {
                                    var deleteModel = await _DbContext.ArticleCategories.FindAsync(item.Id);
                                    _DbContext.ArticleCategories.Remove(deleteModel);
                                    await _DbContext.SaveChangesAsync();
                                }
                            }

                            var ExistRelated = await _DbContext.ArticleRelateds.Where(s => s.ArticleId == Id).ToListAsync();
                            if (ExistRelated != null && ExistRelated.Count > 0)
                            {
                                foreach (var item in ExistRelated)
                                {
                                    var deleteModel = await _DbContext.ArticleRelateds.FindAsync(item.Id);
                                    _DbContext.ArticleRelateds.Remove(deleteModel);
                                    await _DbContext.SaveChangesAsync();
                                }
                            }

                            var ExistTag = await _DbContext.ArticleTags.Where(s => s.ArticleId == Id).ToListAsync();
                            if (ExistTag != null && ExistTag.Count > 0)
                            {
                                foreach (var item in ExistTag)
                                {
                                    var deleteModel = await _DbContext.ArticleTags.FindAsync(item.Id);
                                    _DbContext.ArticleTags.Remove(deleteModel);
                                    await _DbContext.SaveChangesAsync();
                                }
                            }

                            var article = await _DbContext.Articles.FindAsync(Id);
                            _DbContext.Articles.Remove(article);
                            await _DbContext.SaveChangesAsync();

                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            LogHelper.InsertLogTelegram("DeleteArticle - Transaction Rollback - ArticleDAL: " + ex);
                            transaction.Rollback();
                            return -1;
                        }
                    }
                }
                return Id;
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("DeleteArticle - ArticleDAL: " + ex);
                return -1;
            }
        }

        /// <summary>
        /// cuonglv
        /// Lấy ra danh sách các bài thuộc 1 chuyên mục
        /// </summary>
        /// <param name="cate_id"></param>
        /// <returns></returns>
        public async Task<List<ArticleViewModel>> getArticleListByCategoryId(int cate_id)
        {
            try
            {
                var model = new ArticleModel();
                using (var _DbContext = new EntityDataContext(_connection))
                {
                    using (var transaction = _DbContext.Database.BeginTransaction())
                    {
                        try
                        {


                            var list_article = await (from _article in _DbContext.Articles.AsNoTracking()
                                                      join a in _DbContext.ArticleCategories.AsNoTracking() on _article.Id equals a.ArticleId into af
                                                      from detail in af.DefaultIfEmpty()
                                                      where detail.CategoryId == cate_id && _article.Status == ArticleStatus.PUBLISH
                                                      select new ArticleViewModel
                                                      {
                                                          Id = detail.Id,
                                                          Title = _article.Title,
                                                          PublishDate = _article.PublishDate ?? DateTime.Now,
                                                          Lead = _article.Lead,
                                                          Body = _article.Body
                                                      }
                                                     ).ToListAsync();


                            transaction.Commit();
                            return list_article;
                        }
                        catch (Exception ex)
                        {
                            LogHelper.InsertLogTelegram("getArticleListByCategoryId - Transaction Rollback - ArticleDAL: " + ex);
                            transaction.Rollback();
                            return null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("getArticleListByCategoryId - ArticleDAL: " + ex);
                return null;
            }
        }

        /// <summary>
        /// cuonglv
        /// Lấy ra chi tiết bài viết
        /// </summary>
        /// <param name="article_id"></param>
        /// <returns></returns>
        public async Task<ArticleModel> GetArticleDetailLite(long article_id)
        {
            try
            {
                var model = new ArticleModel();
                using (var _DbContext = new EntityDataContext(_connection))
                {
                    using (var transaction = _DbContext.Database.BeginTransaction())
                    {
                        try
                        {
                            var article = await _DbContext.Articles.FirstOrDefaultAsync(x => x.Status == ArticleStatus.PUBLISH && x.Id == article_id);
                            model = new ArticleModel
                            {
                                Id = article.Id,
                                Title = article.Title,
                                Lead = article.Lead,
                                Body = article.Body,
                                Status = article.Status,
                                ArticleType = article.ArticleType,
                                Image11 = article.Image11,
                                Image43 = article.Image43,
                                Image169 = article.Image169,
                                PublishDate = article.PublishDate ?? DateTime.Now,
                                DownTime = article.DownTime ?? DateTime.Now
                            };

                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            LogHelper.InsertLogTelegram("GetArticleDetailLite - Transaction Rollback - ArticleDAL: " + ex);
                            transaction.Rollback();
                            return null;
                        }
                    }
                }
                return model;
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("[article_id = " + article_id + "]GetArticleDetailLite - ArticleDAL: " + ex);
                return null;
            }
        }

        /// <summary>
        /// cuonglv
        /// Lọc những bài faq theo title
        /// </summary>
        /// <param name="article_id"></param>
        /// <returns></returns>
        public async Task<List<ArticleViewModel>> FindArticleByTitle(string title, int parent_cate_faq_id)
        {
            try
            {
                var list_article = new List<ArticleViewModel>();
                using (var _DbContext = new EntityDataContext(_connection))
                {
                    using (var transaction = _DbContext.Database.BeginTransaction())
                    {
                        list_article = await (from article in _DbContext.Articles.AsNoTracking()
                                              join a in _DbContext.ArticleCategories on article.Id equals a.ArticleId into af
                                              from detail in af.DefaultIfEmpty()
                                              where article.Status == ArticleStatus.PUBLISH && article.Title.ToUpper().Contains(title.ToUpper())
                                              select new ArticleViewModel
                                              {
                                                  Id = detail.Id,
                                                  Title = article.Title.Trim(),
                                                  PublishDate = article.PublishDate ?? DateTime.Now,
                                                  Lead = article.Lead.Trim(),
                                                  Body = article.Body
                                              }
                                                          ).ToListAsync();
                    }
                }
                return list_article;
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("[title = " + title + "] FindArticleByTitle - ArticleDAL: " + ex);
                return null;
            }
        }
        public async Task<List<int>> GetArticleCategoryIdList(long ArticleId)
        {
            var ListRs = new List<int>();
            try
            {
                using (var _DbContext = new EntityDataContext(_connection))
                {
                    ListRs = await _DbContext.ArticleCategories.Where(s => s.ArticleId == ArticleId).Select(s => (int)s.CategoryId).ToListAsync();
                }
            }
            catch
            {

            }
            return ListRs;
        }
        /// <summary>
        /// minh.nq
        /// Lấy ra danh sách các bài thuộc 1 chuyên mục, phân trang+ sắp xếp theo ngày mới nhất
        /// </summary>
        /// <param name="cate_id"></param>
        /// <returns></returns>
        public async Task<ArticleFEModelPagnition> getArticleListByCategoryIdOrderByDate(int cate_id, int skip, int take, string category_name)
        {
            try
            {
                var model = new ArticleModel();
                var list_postion_pinned = new List<short?> { 1, 2, 3 };
                using (var _DbContext = new EntityDataContext(_connection))
                {
                    using (var transaction = _DbContext.Database.BeginTransaction())
                    {
                        try
                        {

                            var list_article = await (from _article in _DbContext.Articles.AsNoTracking()
                                                      join a in _DbContext.ArticleCategories.AsNoTracking() on _article.Id equals a.ArticleId into af
                                                      from detail in af.DefaultIfEmpty()
                                                      where detail.CategoryId == cate_id && _article.Status == ArticleStatus.PUBLISH
                                                      orderby _article.PublishDate descending
                                                      select new ArticleFeModel
                                                      {
                                                          category_name = category_name,
                                                          title = _article.Title,
                                                          lead = _article.Lead,
                                                          image_169 = _article.Image169,
                                                          image_43 = _article.Image43,
                                                          image_11 = _article.Image11,
                                                          publish_date = (DateTime)_article.PublishDate,
                                                          link = CommonHelper.genLinkNews(_article.Title, _article.Id.ToString())
                                                      }
                                                     ).ToListAsync();
                            var list_pinned = await (from _article in _DbContext.Articles.AsNoTracking()
                                                     join a in _DbContext.ArticleCategories.AsNoTracking() on _article.Id equals a.ArticleId into af
                                                     from detail in af.DefaultIfEmpty()
                                                     where detail.CategoryId == cate_id && _article.Status == ArticleStatus.PUBLISH && _article.PublishDate <= DateTime.Now && _article.DownTime > DateTime.Now && list_postion_pinned.Contains(_article.Position)
                                                     orderby _article.Position ascending
                                                     select new ArticleFeModel
                                                     {
                                                         category_name = category_name,
                                                         title = _article.Title,
                                                         lead = _article.Lead,
                                                         image_169 = _article.Image169,
                                                         image_43 = _article.Image43,
                                                         image_11 = _article.Image11,
                                                         publish_date = (DateTime)_article.PublishDate,
                                                         link = CommonHelper.genLinkNews(_article.Title, _article.Id.ToString()),
                                                         postition = _article.Position
                                                     }).ToListAsync();

                            transaction.Commit();
                            var result = new ArticleFEModelPagnition();
                            if (list_pinned != null && list_pinned.Count > 0 && cate_id == 401)
                            {
                                foreach (var pinned in list_pinned)
                                {
                                    if (pinned.postition != null && pinned.postition > 0)
                                    {
                                        list_article.RemoveAll(x => x.title == pinned.title && x.lead == pinned.lead);
                                        list_article.Insert(((int)pinned.postition - 1), pinned);
                                    }
                                }
                            }
                            result.list_article_fe = list_article.Skip(skip).Take(take).ToList();
                            result.total_item_count = list_article.Count;
                            return result;
                        }
                        catch (Exception ex)
                        {
                            LogHelper.InsertLogTelegram("getArticleListByCategoryIdOrderByDate - Transaction Rollback - ArticleDAL: " + ex);
                            transaction.Rollback();
                            return null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("getArticleListByCategoryIdOrderByDate - ArticleDAL: " + ex);
                return null;
            }
        }
        /// <summary>
        /// Minh: Lấy ra bài viết được pinn trong time
        /// </summary>
        /// <param name="cate_id"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <param name="category_name"></param>
        /// <returns></returns>
        public async Task<ArticleFeModel> getPinnedArticleByPostition(int cate_id, string category_name, int position)
        {
            try
            {
                var model = new ArticleModel();
                using (var _DbContext = new EntityDataContext(_connection))
                {
                    using (var transaction = _DbContext.Database.BeginTransaction())
                    {
                        try
                        {

                            var article = await (from _article in _DbContext.Articles.AsNoTracking()
                                                 join a in _DbContext.ArticleCategories.AsNoTracking() on _article.Id equals a.ArticleId into af
                                                 from detail in af.DefaultIfEmpty()
                                                 where detail.CategoryId == cate_id && _article.Status == ArticleStatus.PUBLISH && _article.UpTime <= DateTime.Now && DateTime.Now < _article.DownTime && _article.Position == position
                                                 select new ArticleFeModel
                                                 {
                                                     category_name = category_name,
                                                     title = _article.Title,
                                                     lead = _article.Lead,
                                                     image_169 = _article.Image169,
                                                     image_43 = _article.Image43,
                                                     image_11 = _article.Image11,
                                                     publish_date = (DateTime)_article.PublishDate,
                                                     link = CommonHelper.genLinkNews(_article.Title, _article.Id.ToString())
                                                 }
                                                     ).FirstOrDefaultAsync();


                            transaction.Commit();
                            return article;
                        }
                        catch (Exception ex)
                        {
                            LogHelper.InsertLogTelegram("getPinnedArticleByPostition - Transaction Rollback - ArticleDAL: " + ex);
                            transaction.Rollback();
                            return null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("getPinnedArticleByPostition - ArticleDAL: " + ex);
                return null;
            }
        }
    }
}
