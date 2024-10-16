using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.ViewModels.Products
{
    public class ProductMongoDbModel
    {
        [BsonElement("_id")]
        public string _id { get; set; }
        public void GenID()
        {
            _id = ObjectId.GenerateNewId().ToString();
        }
        public string code { get; set; }

        public double price { get; set; }
        public double profit { get; set; }
        public double amount { get; set; }
        public double? amount_min { get; set; }
        public double? amount_max { get; set; }
        public int quanity_of_stock { get; set; }

        public double discount { get; set; }
        public List<string> images { get; set; }
        public string avatar { get; set; }
        public List<string> videos { get; set; }
        public string name { get; set; }
        public string group_product_id { get; set; }
        public string description { get; set; }
        public List<ProductDetailVariationAttributesMongoDbModel> variation_detail { get; set; }
        public List<ProductSpecificationDetailMongoDbModel> specification { get; set; }
        public List<ProductAttributeMongoDbModel> attributes { get; set; }
        public List<ProductAttributeMongoDbModelItem> attributes_detail { get; set; }
        public List<ProductDiscountOnGroupsBuyModel> discount_group_buy { get; set; }

        public int preorder_status { get; set; }
        public float star { get; set; }
        public int condition_of_product { get; set; }
        public string sku { get; set; }
        public DateTime created_date { get; set; }
        public DateTime updated_last { get; set; }
        public string parent_product_id { get; set; }
        public int status { get; set; }
        public bool? is_one_weight { get; set; }
        public float? weight { get; set; }
        public float? package_width { get; set; }
        public float? package_height { get; set; }
        public float? package_depth { get; set; }

    }
}
