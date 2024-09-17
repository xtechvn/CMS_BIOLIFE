using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.ViewModels.Affiliate
{
    public class AffiliateViewModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        [BsonElement]
        public double client_id { get; set; }
        [BsonElement]
        public string utm_source { get; set; }
        [BsonElement]
        public string aff_id { get; set; }
        [BsonElement]
        public DateTime update_time { get; set; }

        public string utm_medium { get; set; }
        public string utm_campaign { get; set; }
        public string utm_first_time { get; set; }
    }
}
