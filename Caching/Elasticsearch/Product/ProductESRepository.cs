using Elasticsearch.Net;
using Entities.ViewModels;
using Entities.ViewModels.Products;
using Nest;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities;
using Utilities.Contants;

namespace Caching.Elasticsearch
{
    //https://www.steps2code.com/post/how-to-use-elasticsearch-in-csharp
    public class ProductESRepository<TEntity> : IProductESRepository<TEntity> where TEntity : class
    {
        private static string _ElasticHost;

        public ProductESRepository(string Host)
        {
            _ElasticHost = Host;
        }

        public bool DeleteProductByCode(string index_name, string document_id)
        {
            string key_es_id = string.Empty;
            try
            {
                var nodes = new Uri[] { new Uri(_ElasticHost) };
                var connectionPool = new StaticConnectionPool(nodes);
                var connectionSettings = new ConnectionSettings(connectionPool).DisableDirectStreaming().DefaultIndex("people");
                var elasticClient = new ElasticClient(connectionSettings);
                // input
                //var document = new ProductViewModel
                //{
                //    product_code = document_id
                //};
                //// find key
                //var result = elasticClient.Index(document, i => i.Index(index_name));
                /*
                var result = elasticClient.Search<object>(s => s
                                             .Index(index_name)
                                             .Query(q => q
                                             .Term("product_code", document_id))
                                             );*/
                var result = elasticClient.Search<object>(sd => sd
                               .Index(index_name)
                               .Query(q => q
                                   .Match(m => m.Field("product_code").Query(document_id)
                                   )));

                if (result.IsValid && result.Documents.Count > 0)
                {
                    foreach (Hit<object> hit in (IHit<object>[])result.HitsMetadata.Hits)
                    {
                        key_es_id = ((Hit<object>)((IHit<object>[])result.HitsMetadata.Hits)[0]).Id;
                        var response = elasticClient.Delete<object>(key_es_id.ToString(), d => d
                        .Index(index_name)
                        );
                    }
                    return result.IsValid;
                }
                else
                {
                    return true; // chưa có mã này
                }
            }
            catch (Exception ex)
            {
                //LogHelper.InsertLogTelegram("DeleteProductByCode key_es_id= " + key_es_id + " document_id" + document_id + " Exception" + ex.ToString());
                return false;
            }
        }

        
        /// <summary>

        /// </summary>
        /// <param name="indexName"></param>
        /// <param name="value">Giá trị cần tìm kiếm</param>
        /// <param name="field_name">Tên cột cần search</param>
        /// <returns></returns>
        public TEntity FindById(string indexName, object value, string field_name = "id")
        {
            try
            {
                var nodes = new Uri[] { new Uri(_ElasticHost) };
                var connectionPool = new StaticConnectionPool(nodes);
                var connectionSettings = new ConnectionSettings(connectionPool).DisableDirectStreaming().DefaultIndex("people");
                var elasticClient = new ElasticClient(connectionSettings);

                var searchResponse = elasticClient.Search<object>(s => s
                    .Index(indexName)
                    .Query(q => q.Term(field_name, value))
                );

                var JsonObject = JsonConvert.SerializeObject(searchResponse.Documents);
                var ListObject = JsonConvert.DeserializeObject<List<TEntity>>(JsonObject);
                return ListObject.FirstOrDefault();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Lấy ra chi tiết sản phẩm theo code
        /// </summary>
        /// <param name="index_name"></param>
        /// <param name="value_search"></param>
        /// <returns></returns>

        public int UpSert(TEntity entity, string indexName)
        {
            try
            {
                var nodes = new Uri[] { new Uri(_ElasticHost) };
                var connectionPool = new StaticConnectionPool(nodes);
                var connectionSettings = new ConnectionSettings(connectionPool).DisableDirectStreaming().DefaultIndex("people");
                var elasticClient = new ElasticClient(connectionSettings);
                var indexResponse = elasticClient.Index(new IndexRequest<TEntity>(entity, indexName));

                if (!indexResponse.IsValid)
                {
                    //LogHelper.WriteLogActivity(Directory.GetCurrentDirectory(), indexResponse.OriginalException.Message);
                    //LogHelper.WriteLogActivity(Directory.GetCurrentDirectory(), "apicall" + indexResponse.ApiCall.OriginalException.Message);
                    return 0;
                }

                return 1;
            }
            catch
            {
                return -1;
            }
        }

        public async Task<int> UpSertAsync(TEntity entity, string indexName)
        {
            try
            {
                var nodes = new Uri[] { new Uri(_ElasticHost) };
                var connectionPool = new StaticConnectionPool(nodes);
                var connectionSettings = new ConnectionSettings(connectionPool).DisableDirectStreaming().DefaultIndex("people");
                var elasticClient = new ElasticClient(connectionSettings);

                var indexResponse = elasticClient.Index(entity, i => i.Index(indexName));
                if (!indexResponse.IsValid)
                {
                    // If the request isn't valid, we can take action here
                }

                var indexResponseAsync = await elasticClient.IndexDocumentAsync(entity);
            }
            catch
            {

            }
            return 0;
        }



        
    }




}
