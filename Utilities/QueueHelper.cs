using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utilities.Contants;

namespace Utilities
{
    public static class QueueHelper
    {
        public class QueueSettingViewModel
        {
            public string host { get; set; }
            public int port { get; set; }
            public string v_host { get; set; }
            public string username { get; set; }
            public string password { get; set; }
            public string queue_name { get; set; }
        }
        public static bool InsertQueueSimple(QueueSettingViewModel queue_setting, string message)
        {
            var factory = new ConnectionFactory()
            {
                HostName = queue_setting.host,
                UserName = queue_setting.username,
                Password = queue_setting.password,
                VirtualHost = queue_setting.v_host,
                Port = Protocols.DefaultProtocol.DefaultPort
            };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                try
                {
                    channel.QueueDeclare(queue: queue_setting.queue_name,
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                    var body = Encoding.UTF8.GetBytes(message);

                    channel.BasicPublish(exchange: "",
                                         routingKey: queue_setting.queue_name,
                                         basicProperties: null,
                                         body: body);
                    return true;

                }
                catch (Exception ex)
                {
                    LogHelper.InsertLogTelegram("InsertQueueSimple ==> error:  " + ex.Message);
                    return false;
                }
            }
        }
        /*
        public void PutQueue(List<string> listLink, int group_id, string queueName)
        {
            var factory = new ConnectionFactory()
            {
                UserName = "qcenv",
                Password = "qc123456",
                VirtualHost = "vhost_qc",
                Port = Protocols.DefaultProtocol.DefaultPort,
                HostName = "103.74.121.156"
            };
            QueueModel queueModel = AddQueueModel(listLink, group_id);
            var jsonStr = JsonConvert.SerializeObject(queueModel);
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

                var body = Encoding.UTF8.GetBytes(jsonStr);

                var properties = channel.CreateBasicProperties();
                properties.Persistent = true;

                channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: properties, body: body);
            }
        }
        public QueueModel AddQueueModel(List<string> listLink, int group_id)
        {
            List<string> listAmz = listLink.Where(n => n.Contains("Amazon")|| n.Contains("amazon")).ToList();
            List<string> listCostco = listLink.Where(n => n.Contains("CostCo")|| n.Contains("costCo")).ToList();
            List<string> listJomashop = listLink.Where(n => n.Contains("Jomashop")|| n.Contains("jomashop")).ToList();
            List<string> listSephora = listLink.Where(n => n.Contains("Sephora")|| n.Contains("sephora")).ToList();
            List<string> listVitoriaSecret = listLink.Where(n => n.Contains("VitoriaSecret") || n.Contains("vitoriaSecret")).ToList();
            List<string> listHautelook = listLink.Where(n => n.Contains("Hautelook") || n.Contains("hautelook")).ToList();
            List<string> listBestbuy = listLink.Where(n => n.Contains("Bestbuy") || n.Contains("bestbuy")).ToList();
            
            List<string> listNordstromRack = listLink.Where(n => n.Contains("NordstromRack") || n.Contains("nordstromRack")).ToList();
            LinkModel linkModel = new LinkModel();
            QueueModel queueModel = new QueueModel();
            queueModel.Group_id = group_id;
            linkModel.Lable_id = (int)LabelType.amazon;
            linkModel.ListLink = listAmz;
            queueModel.Amazon = linkModel;
            linkModel = new LinkModel();
            linkModel.Lable_id = (int)LabelType.costco;
            linkModel.ListLink = listCostco;
            queueModel.CostCo = linkModel;
            linkModel = new LinkModel();
            linkModel.Lable_id = (int)LabelType.jomashop;
            linkModel.ListLink = listJomashop;
            queueModel.Jomashop = linkModel;
            linkModel = new LinkModel();
            linkModel.Lable_id = (int)LabelType.sephora;
            linkModel.ListLink = listSephora;
            queueModel.Sephora = linkModel;
            linkModel = new LinkModel();
            linkModel.Lable_id = (int)LabelType.victoria_secret;
            linkModel.ListLink = listVitoriaSecret;
            queueModel.VitoriaSecret = linkModel;
            linkModel = new LinkModel();
            linkModel.Lable_id = (int)LabelType.hautelook;
            linkModel.ListLink = listHautelook;
            queueModel.Hautelook = linkModel;
            linkModel = new LinkModel();
            linkModel.Lable_id = (int)LabelType.bestbuy;
            linkModel.ListLink = listBestbuy;
            queueModel.Bestbuy = linkModel;
            linkModel = new LinkModel();
            
            queueModel.Lacoste = linkModel;
            linkModel = new LinkModel();
            
            linkModel.Lable_id = (int)LabelType.costco;
            queueModel.Macy = linkModel;
            linkModel = new LinkModel();
            linkModel.Lable_id = (int)LabelType.nordstromrack;
            linkModel.ListLink = listNordstromRack;
            queueModel.NordstromRack = linkModel;

            return queueModel;
        }
        private static string GetMessage(string[] args)
        {
            return ((args.Length > 0) ? string.Join(" ", args) : "");
        }
    }
    public class QueueModel
    {
        public int Group_id { get; set; }
        public LinkModel Amazon { get; set; }
        public LinkModel CostCo { get; set; }
        public LinkModel Jomashop { get; set; }
        public LinkModel Sephora { get; set; }
        public LinkModel VitoriaSecret { get; set; }
        public LinkModel Hautelook { get; set; }
        public LinkModel Bestbuy { get; set; }
        public LinkModel Lacoste { get; set; }
        public LinkModel Macy { get; set; }
        public LinkModel NordstromRack { get; set; }
    }
    public class LinkModel
    {
        public int Lable_id { get; set; }
        public List<string> ListLink { get; set; }
    }
        */



    }
}
