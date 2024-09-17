using DAL;
using Entities.ConfigModels;
using Entities.Models;
using Microsoft.Extensions.Options;
using Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities;

namespace Repositories.Repositories
{
    public class ClientRepository : IClientRepository
    {
        private readonly ClientDAL _ClientDAL;

        public ClientRepository(IOptions<DataBaseConfig> dataBaseConfig)
        {
            _ClientDAL = new ClientDAL(dataBaseConfig.Value.SqlServer.ConnectionString);
        }
        public async Task<Client> GetClientDetailAsync(long clientId)
        {
            try
            {
                var AccountClient = await _ClientDAL.GetAccountClientDetai(clientId);
                if (AccountClient != null)
                {
                    return await _ClientDAL.GetClientDetail((long)AccountClient.ClientId);
                }
                else
                { return null; }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetClientDetailAsync - ClientRepository: " + ex);
                return null;
            }
        }
        public async Task<Client> GetClientDetailByClientId(long clientId)
        {
            try
            {
                var data = await _ClientDAL.GetClientDetail(clientId);
                return data;
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetClientDetailByClientId - ClientRepository: " + ex);
                return null;
            }
        }
        public async Task<AccountClient> GetAccountClientDetailAsync(long Id)
        {
            try
            {
                return await _ClientDAL.GetAccountClientDetai(Id);
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetAccountClientDetailAsync - ClientRepository: " + ex);
                return null;
            }
        }
        public Task<List<Client>> GetClientType(int Type)
        {
            try
            {
                return _ClientDAL.GetClientType(Type);
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetClientType - ClientRepository: " + ex);
                return null;
            }
        }

        public async Task<List<AccountClient>> GetUserCreateSuggest(string name)
        {
            List<AccountClient> data = new List<AccountClient>();
            try
            {
                data = await _ClientDAL.GetAccountClientAsync();
                data = data.Where(s => s.ClientType == 2 || s.ClientType == 3 || s.ClientType == 4).ToList();
                if (!string.IsNullOrEmpty(name))
                {
                    data = data.Where(s => s.UserName.Trim().ToLower().Contains(name.Trim().ToLower())).ToList();
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetUserCreateSuggest - ClientRepository: " + ex);
            }
            return data;
        }
        public List<Client> GetAllClient()
        {
            try
            {
                return _ClientDAL.GetAll();
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetAllClient - ClientRepository: " + ex);
                return null;
            }
        }
        public Client GetClientByEmail(string email)
        {
            try
            {
                return _ClientDAL.GetClientByEmail(email);
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetClientByEmail - ClientRepository: " + ex);
                return null;
            }
        }
        public Client GetClientByTaxNo(string Maso_id)
        {
            try
            {
                return _ClientDAL.GetClientByTaxNo(Maso_id);
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetClientByTaxNo - ClientRepository: " + ex);
                return null;
            }
        }
        public int SetUpClient(Client model)
        {
            try
            {
                return _ClientDAL.SetUpClient(model);
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("SetUpClient - ClientRepository: " + ex);
                return 0;
            }
        }
        public async Task<Client> GetClientByClientCode(string client_code)
        {
            try
            {
                return await _ClientDAL.GetClientByClientCode(client_code);
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetClientByTaxNo - ClientRepository: " + ex);
                return null;
            }
        }
    }
}
