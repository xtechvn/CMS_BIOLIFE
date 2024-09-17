using System;
using System.Collections.Generic;
using System.Text;

namespace Utilities.Contants
{
   public struct TaskQueueName
    {
        public const string product_detail_more_crawl_queue = "product_detail_more_crawl_queue"; // queue này sẽ xử lý các trang chi tiết ko phải amz
        public const string product_crawl_queue = "product_crawl_queue"; // danh rieng cho amazon
        public const string client_old_convert_queue = "client_old_convert_queue";// queue này chứa client bên hệ thống cũ. Mục đích để push về hệ thống mới
        public const string order_old_convert_queue = "order_old_convert_queue";// queue này chứa đơn hàng bên hệ thống cũ. Mục đích để push về hệ thống mới
        public const string product_detail_amazon_crawl_queue = "product_detail_amazon_crawl_queue";// queue này chứa product_label. Mục đích cs đọc ra để crawl chi tiết sản phẩm
        public const string product_offer_listing_amazon_crawl_queue = "product_offer_listing_amazon_crawl_queue"; // queue này chứa product_label. Mục đích cs đọc ra để crawl giá
        public const string product_es_queue = "product_es_queue"; // queue này chứa nhưng resdis key về sản phẩm. Mục đích đẩy data sản phẩm lên ES
     
        public const string client_new_convert_queue = "client_new_convert_queue"; // queue này chứa nhưng clientID của hệ thống mới. Mục đích sẽ push qua con cũ để tiến hành đặt mua 
        public const string order_new_convert_queue = "order_new_convert_queue"; // queue này chứa nhưng order của hệ thống mới. Mục đích sẽ push qua con cũ để tiến hành đặt mua 

        public const string black_friday_2020_datafeed = "black_friday_2020_datafeed"; // queue này chứa các sản phẩm sale trên amazon vào ngày black friday
        public const string data_feed = "data_feed"; // queue này chứa các sản phấm show ra ngoài mặt trang home
        public const string log_front_end = "log_front_end"; //// queue này chứa các log ghi lại trong qúa trình hoạt động. Log sẽ được xử lý và lưu ở nhiều nơi khác nhau phục vụ cho việc trace.
        public const string group_product_mapping = "group_product_mapping"; // Queue naỳ chưa object để Bot crawl đọc và crawl các link sản phẩm mapping về cho Menu của hệ thống

        public const string joma_detail = "joma_detail"; // Queue này chưa object để Bot crawl đọc và crawl các link sản phẩm Jomashop về cho Menu của hệ thống


        public const string keyword_crawl_queue = "keyword_crawl_queue";// queue này chứa tham số keywors theo các nhãn hàng. Mục đích cs đọc ra để crawl trang search nhãn
        public const string group_product_mapping_detail = "group_product_mapping_detail"; // Quene này dùng để push các sản phẩm được kéo về từ mapping ngành hàng
        public const string affiliate_datafeed = "affiliate_datafeed"; //Queue này dùng để push data feed của sang Bot để xử lý lưu file csv

        public const string product_detail_manual_queue = "product_detail_manual_queue";// queue này chứa product detail manual. Mục đích sẽ sync lên ES
    }
}
