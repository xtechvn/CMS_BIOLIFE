using Entities.Models;
using System;

namespace ENTITIES.ViewModels.ElasticSearch
{
   public class OrderElasticsearchViewModel
    {

        public long id { get; set; } // ID ElasticSearch

        public long orderid { get; set; }
        public string orderno { get; set; }

        public void GenID()
        {
            string datetime = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString() + (new Random().Next(100, 999)).ToString();
            id = Convert.ToInt64(datetime);
        }
    }
}
