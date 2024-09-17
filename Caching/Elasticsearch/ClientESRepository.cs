using Elasticsearch.Net;
using Entities.ViewModels.ElasticSearch;
using Microsoft.Extensions.Configuration;
using Nest;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace Caching.Elasticsearch
{
    public class ClientESRepository : ESRepository<CustomerESViewModel>
    {
        public ClientESRepository(string Host) : base(Host) { }


        public async Task<List<CustomerESViewModel>> GetClientSuggesstion(string txt_search, string index_name = "client_hulotoys_store")
        {
            List<CustomerESViewModel> result = new List<CustomerESViewModel>();
            try
            {
                int top = 4000;
                var nodes = new Uri[] { new Uri(_ElasticHost) };
                var connectionPool = new StaticConnectionPool(nodes);
                var connectionSettings = new ConnectionSettings(connectionPool).DisableDirectStreaming().DefaultIndex(index_name);
                var elasticClient = new ElasticClient(connectionSettings);
                if (txt_search == null)
                {
                    var result_all = elasticClient.Search<CustomerESViewModel>(s => s
                          .Index(index_name + (_company_type.Trim() == "0" ? "" : "_" + _company_type.Trim()))
                          .Size(30)
                          .Query(q => q.MatchAll()

                           ));
                    result = result_all.Documents as List<CustomerESViewModel>;
                    return result;
                }
                var search_response = elasticClient.Search<CustomerESViewModel>(s => s
                          .Index(index_name)
                          .Size(top)
                          .Query(q =>
                            q.Bool(
                                qb => qb.Should(
                                    sh => sh.QueryString(m => m
                                    .DefaultField(f => f.phone)
                                    .Query("*" + txt_search + "*")),
                                    sh => sh.QueryString(m => m
                                    .DefaultField(f => f.email)
                                    .Query("*" + txt_search + "*")),
                                    sh => sh.QueryString(m => m
                                    .DefaultField(f => f.clientname)
                                    .Query("*" + txt_search + "*"))

                                ))
                           ));

                if (!search_response.IsValid)
                {
                    return result;
                }
                else
                {
                    result = search_response.Documents as List<CustomerESViewModel>;
                    return result;
                }
            }
            catch (Exception ex)
            {
                return null;
            }

        }
    
        public int UpSert(ClientESViewModel entity, string index_name = "customer")
        {
            try
            {
                var nodes = new Uri[] { new Uri(_ElasticHost) };
                var connectionPool = new StaticConnectionPool(nodes);
                var connectionSettings = new ConnectionSettings(connectionPool).DisableDirectStreaming().DefaultIndex(index_name);
                var elasticClient = new ElasticClient(connectionSettings);
                var indexResponse = elasticClient.Index(new IndexRequest<ClientESViewModel>(entity, index_name + (_company_type.Trim() == "0" ? "" : "_" + _company_type.Trim())));

                if (!indexResponse.IsValid)
                {

                    return 0;
                }

                return 1;
            }
            catch
            {
                return 0;
            }
        }
    }
    public class esService
    {
        private IConfiguration configuration;
        public static string _company_type;


        public esService(IConfiguration _configuration)
        {
            configuration = _configuration;
            AppSettings _appconfig = new AppSettings();
            using (StreamReader r = new StreamReader("appsettings.json"))
            {
                string json = r.ReadToEnd();
                _appconfig = JsonConvert.DeserializeObject<AppSettings>(json);
                _company_type = _appconfig.CompanyType;
            }
        }

        //cuonglv
        // Tìm kiếm thông tin  theo từ khóa trong nhiều cột
        /// <summary>
        /// ///
        /// </summary>
        /// <param name="keyword">từ khóa cần tìm kiếm</param>
        /// <param name="file_name">file chứa input cần tìm kiếm chuẩn json</param>        
        /// <returns></returns>
        public async Task<string> search(string keyword, string file_name)
        {
            try
            {
                string endpoint = string.Empty;
                string url_es = configuration["DataBaseConfig:Elastic:Host"];
                string Client = configuration["DataBaseConfig:Elastic:Index:Client"];
                var workingDirectory = Environment.CurrentDirectory;
                //  var currentDirectory = Directory.GetParent(workingDirectory);
                var query = workingDirectory + @"\QueryEs\" + file_name;

                var body_raw_input = File.ReadAllText(query);
                body_raw_input = body_raw_input.Replace("{index_name}", Client);

                var j_input = JObject.Parse(body_raw_input);
                endpoint = j_input["endpoint"].ToString();
                body_raw_input = j_input["input_query"].ToString();




                using (var client = new HttpClient())
                {
                    var request = new HttpRequestMessage(System.Net.Http.HttpMethod.Post, url_es + endpoint);
                    var content = new StringContent(body_raw_input.Replace("{keyword}", keyword), Encoding.UTF8, "application/json");
                    var response = await client.PostAsync(url_es + endpoint, content);
                    var data= await response.Content.ReadAsStringAsync();
                    return data;
                }
                return string.Empty;

            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram(" esService - searchMultiMatch: " + ex.ToString());
                return string.Empty;
            }
        }
    }
}
