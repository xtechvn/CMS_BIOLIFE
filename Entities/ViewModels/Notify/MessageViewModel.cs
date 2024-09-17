using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ENTITIES.ViewModels.Notify
{    
    public class MessageViewModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }// khóa chính
        [BsonElement]
        public string content { get; set; } // nội dung notify
        [BsonElement]
        public string send_date { get; set; } // thời gian gửi
        [BsonElement]
        public string user_name_send { get; set; } //người gửi
        [BsonElement]
        public string user_id_send { get; set; } //id nguoi gui
        [BsonElement]
        public string code { get; set; } //  mã đối tượng gửi. Là khóa chính của module
        [BsonElement]
        public int module_type { get; set; } // loại module 
    }
}
