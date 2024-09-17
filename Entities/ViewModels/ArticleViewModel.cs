using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.ViewModels
{
    public class ArticleViewModel : Article
    {
        public string AuthorName { get; set; }
        public string ArticleStatusName { get; set; }
        public string ArticleCategoryName { get; set; }
		
    }

    public class ArticleModel
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Lead { get; set; }
        public string Body { get; set; }
        public int Status { get; set; }
        public int ArticleType { get; set; }
        public string Image169 { get; set; }
        public string Image43 { get; set; }
        public string Image11 { get; set; }

        public List<string> Tags { get; set; }
        public List<int> Categories { get; set; }
        public List<long> RelatedArticleIds { get; set; }
        public List<ArticleRelationModel> RelatedArticleList { get; set; }
        public DateTime PublishDate { get; set; }
        public int AuthorId { get; set; }
        public DateTime DownTime { get; set; }
        public int Position { get; set; }
    }

    public class ArticleSearchModel
    {
        public string Title { get; set; }
        public long ArticleId { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public int AuthorId { get; set; }
        public int ArticleStatus { get; set; }
        public int[] ArrCategoryId { get; set; }

        /// <summary>
        /// 0:Basic | 1:Advance
        /// </summary>
        public int SearchType { get; set; }
    }

    public class ArticleRelationModel
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Image { get; set; }
    }
    public class ArticleFeModel
    {
        public string link { get; set; }
        public string category_name { get; set; }
        public string title { get; set; }
        public string lead { get; set; }
        public string image_169 { get; set; }
        public string image_43 { get; set; }
        public string image_11 { get; set; }
        public DateTime publish_date { get; set; }
        
        public short? postition { get; set; }
    }
    public class ArticleFEModelPagnition
    {
       public List<ArticleFeModel> list_article_fe { get; set; }
       public int total_item_count { get; set; }
    }
}
