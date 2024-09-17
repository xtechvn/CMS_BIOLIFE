using Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.IRepositories
{
    public interface IArticleRepository
    {
        GenericViewModel<ArticleViewModel> GetPagingList(ArticleSearchModel searchModel, int currentPage, int pageSize);
        Task<long> SaveArticle(ArticleModel model);
        string SeverVieo(ArticleModel model);
        Task<ArticleModel> GetArticleDetail(long Id);
        Task<long> ChangeArticleStatus(long Id, int Status);
        Task<List<string>> GetSuggestionTag(string name);
        Task<List<ArticleViewModel>> getArticleListByCategoryId(int cate_id);
        Task<ArticleModel> GetArticleDetailLite(long article_id);
        Task<List<ArticleViewModel>> FindArticleByTitle(string title,int parent_cate_faq_id);
        Task<List<int>> GetArticleCategoryIdList(long Id);
        Task<long> DeleteArticle(long Id);
        Task<ArticleFEModelPagnition> getArticleListByCategoryIdOrderByDate(int cate_id, int skip, int take, string category_name);
        Task<ArticleFeModel> GetArticleDetailLiteFE(long article_id);
        Task<ArticleFeModel> GetPinnedArticleByPostition(int cate_id, string category_name, int position);

    }
}
