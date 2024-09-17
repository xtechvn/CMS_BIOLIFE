using Caching.Elasticsearch;
using Elasticsearch.Net;
using Entities.Models;
using Entities.ViewModels.ElasticSearch;
using Microsoft.Extensions.Configuration;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Utilities;
using Utilities.Contants;

namespace HuloToys_Service.ElasticSearch.NewEs
{
    public class GroupProductESService : ESRepository<GroupProduct>
    {
        public string index = "group_product_hulotoys_store";
        private readonly IConfiguration configuration;
        private static string _ElasticHost;

        public GroupProductESService(string Host, IConfiguration _configuration) : base(Host)
        {
            _ElasticHost = Host;
            configuration = _configuration;
            index = _configuration["DataBaseConfig:Elastic:Index:GroupProduct"];

        }
        public List<GroupProduct> GetListGroupProductByParentId(long parent_id)
        {
            try
            {
                var nodes = new Uri[] { new Uri(_ElasticHost) };
                var connectionPool = new StaticConnectionPool(nodes);
                var connectionSettings = new ConnectionSettings(connectionPool).DisableDirectStreaming().DefaultIndex("people");
                var elasticClient = new ElasticClient(connectionSettings);

                var query = elasticClient.Search<GroupProductESModel>(sd => sd
                               .Index(index)
                               .Size(4000)
                          .Query(q =>
                           q.Bool(
                               qb => qb.Must(
                                   sh => sh.Match(m => m.Field("parentid").Query(parent_id.ToString())
                                   )
                                   )
                               )
                          ));

                if (query.IsValid)
                {
                    var data = query.Documents as List<GroupProductESModel>;
                    var result = data.Select(a => new GroupProduct
                    {
                        Id = a.id,
                        ParentId = a.parentid,
                        PositionId = a.positionid,                        
                        Name = a.name,
                        ImagePath = a.imagepath,
                        OrderNo = a.orderno,
                        Path = a.path,
                        Status = a.status,
                        Description = a.description,
                        IsShowHeader = a.isshowheader,
                        IsShowFooter = a.isshowfooter,

                    }).ToList();
                    return result;
                }
            }
            catch (Exception ex)
            {
                string error_msg = Assembly.GetExecutingAssembly().GetName().Name + "->" + MethodBase.GetCurrentMethod().Name + "=>" + ex.Message;
                LogHelper.InsertLogTelegram( error_msg);
            }
            return null;
        }
        public GroupProduct GetDetailGroupProductById(long id)
        {
            try
            {
                var nodes = new Uri[] { new Uri(_ElasticHost) };
                var connectionPool = new StaticConnectionPool(nodes);
                var connectionSettings = new ConnectionSettings(connectionPool).DisableDirectStreaming().DefaultIndex("people");
                var elasticClient = new ElasticClient(connectionSettings);

                var query = elasticClient.Search<GroupProductESModel>(sd => sd
                               .Index(index)
                          .Query(q =>
                           q.Bool(
                               qb => qb.Must(
                                  q => q.Match(m => m.Field("status").Query(((int)ArticleStatus.PUBLISH).ToString())),
                                   sh => sh.Match(m => m.Field("id").Query(id.ToString())
                                   )
                                   )
                               )
                          ));

                if (query.IsValid)
                {
                    var data = query.Documents as List<GroupProductESModel>;
                    var result = data.Select(a => new GroupProduct
                    {
                        Id = a.id,
                        ParentId = a.parentid,
                        PositionId = a.positionid,
                        Name = a.name,
                        ImagePath = a.imagepath,
                        OrderNo = a.orderno,
                        Path = a.path,
                        Status = a.status,
                        Description = a.description,
                        IsShowHeader = a.isshowheader,
                        IsShowFooter = a.isshowfooter,

                    }).ToList();
                    return result.FirstOrDefault();
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
