using Elasticsearch.Net;
using Entities.ViewModels.ElasticSearch;
using ENTITIES.ViewModels.ElasticSearch;
using Microsoft.Extensions.Configuration;
using Nest;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace Caching.Elasticsearch
{
    public class OrderESRepository : ESRepository<OrderElasticsearchViewModel>
    {
        public string index_name = "order_hulotoys_store";
        private readonly IConfiguration configuration;
        private static string _ElasticHost;
        public OrderESRepository(string Host, IConfiguration _configuration) : base(Host)
        {
            _ElasticHost = Host;
            configuration = _configuration;
            index_name = _configuration["DataBaseConfig:Elastic:Index:Order"];
        }
        public async Task<List<OrderElasticsearchViewModel>> GetOrderNoSuggesstion(string txt_search)
        {
            List<OrderElasticsearchViewModel> result = new List<OrderElasticsearchViewModel>();
            try
            {
                int top = 30;
                var nodes = new Uri[] { new Uri(_ElasticHost) };
                var connectionPool = new StaticConnectionPool(nodes);
                var connectionSettings = new ConnectionSettings(connectionPool).DisableDirectStreaming().DefaultIndex(index_name);
                var elasticClient = new ElasticClient(connectionSettings);

                var search_response = elasticClient.Search<OrderElasticsearchViewModel>(s => s
                          .Index(index_name + (_company_type.Trim() == "0" ? "" : "_" + _company_type.Trim()))
                          .Size(top)
                          .Query(q =>
                             q.QueryString(qs => qs
                               .Fields(new[] { "orderno" })
                               .Query("*" + txt_search.ToUpper() + "*")
                               .Analyzer("standard")
                           )
                          ));

                if (!search_response.IsValid)
                {
                    return result;
                }
                else
                {
                    result = search_response.Documents as List<OrderElasticsearchViewModel>;
                    return result;
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetOrderNoSuggesstion - OrderESRepository. " + ex);
                return null;
            }

        }
        public async Task<List<OrderElasticsearchViewModel>> GetOrderNoSuggesstion2(string txt_search)
        {
            List<OrderElasticsearchViewModel> result = new List<OrderElasticsearchViewModel>();
            try
            {
                int top = 30;
                var nodes = new Uri[] { new Uri(_ElasticHost) };
                var connectionPool = new StaticConnectionPool(nodes);
                var connectionSettings = new ConnectionSettings(connectionPool).DisableDirectStreaming().DefaultIndex(index_name);
                var elasticClient = new ElasticClient(connectionSettings);

                var search_response = elasticClient.Search<OrderElasticsearchViewModel>(s => s
                          .Index(index_name + (_company_type.Trim() == "0" ? "" : "_" + _company_type.Trim()))
                          .Size(top)
                          .Query(q =>
                           q.Bool(
                               qb => qb.Must(
                                   sh => sh.QueryString(qs => qs
                                   .Fields(new[] { "orderno" })
                                   .Query("*" + txt_search.ToUpper() + "*")
                                   .Analyzer("standard")

                            )
                           )
                          )));

                if (!search_response.IsValid)
                {
                    return result;
                }
                else
                {
                    result = search_response.Documents as List<OrderElasticsearchViewModel>;
                    return result;
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetOrderNoSuggesstion - OrderESRepository. " + ex);
                return null;
            }

        }
    }
}
