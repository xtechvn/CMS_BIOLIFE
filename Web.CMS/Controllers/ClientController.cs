using Caching.Elasticsearch;
using Entities.ViewModels.ElasticSearch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities;
using Utilities.Contants;

namespace WEB.Adavigo.CMS.Controllers
{
    public class ClientController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IAllCodeRepository _allCodeRepository;
        private ClientESRepository clientESRepository;

        private IClientRepository _clientRepository;
        private IUserAgentRepository _userAgentRepository;
        private IUserRepository _userRepository;

        public ClientController(IConfiguration configuration, IAllCodeRepository allCodeRepository,  IUserRepository userRepository,
            IClientRepository clientRepository, IUserAgentRepository userAgentRepository)
        {

            _configuration = configuration;
            _allCodeRepository = allCodeRepository;
            clientESRepository = new ClientESRepository(_configuration["DataBaseConfig:Elastic:Host"]);
            _clientRepository = clientRepository;
            _userAgentRepository = userAgentRepository;
            _userRepository = userRepository;

        }
        public async Task<IActionResult> ClientSuggestion(string txt_search)
        {

            try
            {

                var clients = await clientESRepository.GetClientSuggesstion(txt_search);
                if(clients!=null && clients.Count > 0)
                {
                    return Ok(new
                    {
                        status = (int)ResponseType.SUCCESS,
                        data = clients,
                    });
                }
               
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("ClientSuggestion - ClientController: " + ex);
                return Ok(new
                {
                    status = (int)ResponseType.SUCCESS,
                    data = new List<CustomerESViewModel>()
                });
            }
            return Ok(new
            {
                status = (int)ResponseType.EMPTY,
                msg = "Không có dữ liệu nào thỏa mãn từ khóa " + txt_search
            });
        }
    }
}
