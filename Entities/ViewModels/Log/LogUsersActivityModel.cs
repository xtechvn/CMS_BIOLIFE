using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.ViewModels.Log
{
    public class LogUsersActivityModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        [BsonElement]
        public int user_type { get; set; }
        [BsonElement]
        public long user_id { get; set; }
        [BsonElement]
        public string user_name { get; set; }
        [BsonElement]
        public DateTime log_date { get; set; }
        [BsonElement]
        public int log_type { get; set; }
        [BsonElement]
        public string action_log { get; set; }
        [BsonElement]
        public string j_data_log { get; set; }
        [BsonElement]
        public string key_word_search { get; set; }
        
    }
}
