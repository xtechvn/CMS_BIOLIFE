using Elasticsearch.Net;
using Entities.ViewModels.ElasticSearch;
using Microsoft.Extensions.Configuration;
using Nest;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Caching.Elasticsearch
{
    public class UserESRepository : ESRepository<UserESViewModel>
    {
        public string index_name = "users_hulotoys_store";
        private readonly IConfiguration configuration;
        public UserESRepository(string Host, IConfiguration _configuration) : base(Host) {
            configuration = _configuration;
            index_name = _configuration["DataBaseConfig:Elastic:Index:users"];
        }

        public async Task<List<UserESViewModel>> GetUserSuggesstion(string txt_search)
        {
            List<UserESViewModel> result = new List<UserESViewModel>();
            try
            {
                int top = 4000;
                var nodes = new Uri[] { new Uri(_ElasticHost) };
                var connectionPool = new StaticConnectionPool(nodes);
                var connectionSettings = new ConnectionSettings(connectionPool).DisableDirectStreaming().DefaultIndex(index_name);
                var elasticClient = new ElasticClient(connectionSettings);

                var search_response = elasticClient.Search<UserESViewModel>(s => s
                          .Index(index_name + (_company_type.Trim() == "0" ? "" : "_" + _company_type.Trim()))
                          .Size(top)
                          .Query(q => q
                           .QueryString(qs => qs
                               .Fields(new[] { "username", "fullname", "email", "phone" })
                               .Query("*" + txt_search + "*")
                               .Analyzer("standard")
                           )
                          ));

                if (!search_response.IsValid)
                {
                    return result;
                }
                else
                {
                    result = search_response.Documents as List<UserESViewModel>;
                    return result;
                }
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        public async Task<UserESViewModel> GetUserByID(string id, string index_name = "user")
        {
            List<UserESViewModel> result = new List<UserESViewModel>();
            try
            {
                int top = 4000;
                var nodes = new Uri[] { new Uri(_ElasticHost) };
                var connectionPool = new StaticConnectionPool(nodes);
                var connectionSettings = new ConnectionSettings(connectionPool).DisableDirectStreaming().DefaultIndex(index_name);
                var elasticClient = new ElasticClient(connectionSettings);

                var search_response = elasticClient.Search<UserESViewModel>(s => s
                          .Index(index_name + (_company_type.Trim() == "0" ? "" : "_" + _company_type.Trim()))
                          .Size(top)
                          .Query(q => q
                           .Match(qs => qs
                               .Field(s => s._id)
                               .Query(id)

                           )
                          ));
                if (!search_response.IsValid)
                {
                    return null;
                }
                else
                {
                    result = search_response.Documents as List<UserESViewModel>;
                    return result.Count > 0 ? result[0] : null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }

        }
       
    }
}
