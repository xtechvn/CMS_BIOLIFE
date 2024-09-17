using Elasticsearch.Net;
using Entities.Models;
using Entities.ViewModels.ElasticSearch;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Utilities;

namespace Caching.Elasticsearch
{
    public class ClientESService : ESRepository<Client>
    {
        public string index = "client_hulotoys_store";
        private readonly IConfiguration configuration;
        private static string _ElasticHost;
        public ClientESService(string Host, IConfiguration _configuration) : base(Host)
        {
            _ElasticHost = Host;
            configuration = _configuration;
            index = _configuration["DataBaseConfig:Elastic:Index:Client"];
        }

        public List<ClientSeachESViewModel> GetClientByNameOrPhone(string phoneOrName) 
       {
            try 
            {
                bool check = Regex.IsMatch(phoneOrName, @"^\d+$");
                var nodes = new Uri[] { new Uri(_ElasticHost) };
                var connectionPool = new StaticConnectionPool(nodes);
                var connectionSettings = new ConnectionSettings(connectionPool).DisableDirectStreaming().DefaultIndex("people");
                var elasticClient = new ElasticClient(connectionSettings);

                var query = elasticClient.Search<ClientSeachESViewModel>(sd => sd
                .Index(index)
                .Query(q =>
                q.Bool(
                    qb => qb.Must(sh => sh.QueryString(m => m
                    .Fields(new[] { "clientname","phone","email" })
                    .Query("*" + phoneOrName + "*")
                    )
                    )
                    )
                ));

                if (query.IsValid)
                {
                    var data = query.Documents as List<ClientSeachESViewModel>;
                    var result = data.Select(a => new ClientSeachESViewModel
                    {
                        Id = a.Id,
                        clientname = a.clientname,
                        Phone = a.Phone,
                        email = a.email,
                    }).ToList();
                    return result;
                }
            }
            catch (Exception ex)
            {
                string error_msg = Assembly.GetExecutingAssembly().GetName().Name + "->" + MethodBase.GetCurrentMethod().Name + "=>" + ex.Message;
                LogHelper.InsertLogTelegram(error_msg);
            }
            return null;
        }
    }
}
