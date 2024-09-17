using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Entities.ViewModels.Affiliate
{
    public class MyAffiliateLinkViewModel
    {
        [BsonElement("_id")]
        public string _id { get; set; }

        [BsonElement("client_id")]
        public long client_id { get; set; }

        [BsonElement("create_date")]
        public DateTime create_date { get; set; }

        [BsonElement("update_time")]
        public DateTime update_time { get; set; }

        [BsonElement("link_aff")]
        public string link_aff { get; set; }

        public void GenID()
        {
            _id = ObjectId.GenerateNewId().ToString();
        }
    }
}
