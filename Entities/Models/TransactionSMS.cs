using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using Utilities;

namespace Entities.Models
{
    public class TransactionSMS
    {
        public ObjectId _id { get; set; }
        public double Amount { get; set; }
        public string BankName { get; set; }
        public string OrderNo { get; set; }
        //public DateTime? ReceiveTime { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)] public DateTime? ReceiveTime { get; set; }
        [BsonIgnore]
        public string ReceiveTimeStr
        {
            get
            {
                return DateUtil.DateTimeToString(ReceiveTime);
            }
            set
            {
                ReceiveTime = DateUtil.StringToDateTime(value);
            }
        }
        public string MessageContent { get; set; }
        //public DateTime? CreatedTime { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)] public DateTime? CreatedTime { get; set; }
        [BsonIgnore]
        public string CreatedTimeStr
        {
            get
            {
                return DateUtil.DateTimeToString(CreatedTime);
            }
            set
            {
                CreatedTime = DateUtil.StringToDateTime(value);
            }
        }
        public bool StatusPush { get; set; }
        public string StatusName
        {
            get
            {
                if (StatusPush == false)
                {
                    return "Thất bại";
                }
                else
                    return "Hoàn thành";
            }
        }
    }
}
