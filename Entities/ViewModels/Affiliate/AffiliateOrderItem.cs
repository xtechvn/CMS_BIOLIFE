using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.ViewModels.Affiliate
{
    public class AffOrder
    {
        public long order_id { get; set; }
        public string aff_name { get; set; }
    }
   public class AffiliateOrderItem : AffOrder
   {
        [BsonElement("_id")]
        public string _id { get; set; }
        public void GenID()
        {
            _id = ObjectId.GenerateNewId().ToString();
        }
        public DateTime time_push { get; set; }
        public int status { get; set; }
        public string msg { get; set; }
   }
}
