using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;


namespace Entities.ViewModels.Payment
{
    public class PaymentLogViewModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }

        [BsonElement]
        public string response_data { get; set; } // Dữ liệu trả về từ kênh thanh toán trung gian
        [BsonElement]
        public DateTime log_date { get; set; } 

        [BsonElement]
        public int payment_type { get; set; } // hình thức thanh toán: Payoo ATM, Payoo VISA, CKTT.....
        
        [BsonElement]
        public string order_no { get; set; } // Mã đơn hàng. UAM1B292929....
    }
}
