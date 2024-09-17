using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;
using Utilities.Contants;

namespace Utilities
{
   public class CacheHelper
   {
        public static string cacheKeyProductDetail(string product_code, int label_id)
        {
            return CacheType.PRODUCT_DETAIL + product_code + "_" + label_id;
        }
        public static string getProductDetailCacheKeyFromURL(string url, int label_id)
        {
            var encoded_text = CommonHelper.GetCachePartFromURL(url,label_id);
            return CacheType.PRODUCT_DETAIL + encoded_text + "_" + label_id;
        }
        public static string cacheKeySearchByKeyWord(string keyword, int label_id)
        {
            int max_w = keyword.Length > 20 ? 20 : keyword.Length - 1;
            keyword = CommonHelper.Encode(keyword.Substring(0, max_w), label_id.ToString());
            return CacheType.KEYWORD + keyword;
        }
        public static string cacheKeyRateCurrent(int day, int month, int year)
        {
            return CacheType.RATE + day + month + year;
        }

    }
}
