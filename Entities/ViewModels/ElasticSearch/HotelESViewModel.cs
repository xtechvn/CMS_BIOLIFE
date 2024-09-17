using System;
using System.Collections.Generic;
using System.Text;

namespace ENTITIES.ViewModels.ElasticSearch
{
   public  class HotelESViewModel : HotelModel
    {
        public long id { get; set; } // ID ElasticSearch
        public string group_name { get; set; } // Chuỗi thương hiệu
        public string telephone { get; set; } // Chuỗi thương hiệu
        public DateTime check_in_time { get; set; }
        public DateTime check_out_time { get; set; }
        public void GenID()
        {
            string datetime = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString() + (new Random().Next(100, 999)).ToString();
            id = Convert.ToInt64(datetime);
        }
    }
}
