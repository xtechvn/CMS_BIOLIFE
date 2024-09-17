using DAL;
using Entities.ConfigModels;
using Entities.Models;
using Entities.ViewModels;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Utilities;

namespace Repositories.Repositories
{
    public class AllCodeRepository : IAllCodeRepository
    {
        private readonly ILogger<AllCodeRepository> _logger;
        private readonly AllCodeDAL _AllCodeDAL;
        private readonly BankingAccountDAL bankingAccountDAL;

        public AllCodeRepository(IOptions<DataBaseConfig> dataBaseConfig, ILogger<AllCodeRepository> logger)
        {
            _logger = logger;
            _AllCodeDAL = new AllCodeDAL(dataBaseConfig.Value.SqlServer.ConnectionString);
            bankingAccountDAL = new BankingAccountDAL(dataBaseConfig.Value.SqlServer.ConnectionString);
        }

        public async Task<long> Create(AllCode model)
        {
            try
            {
                var entity = new AllCode()
                {
                    CodeValue = model.CodeValue,
                    CreateDate = DateTime.Now,
                    Description = model.Description,
                    OrderNo = model.CodeValue,
                    Type = model.Type
                };
                var rs = await _AllCodeDAL.CreateAsync(entity);
                if (entity.Id > 0)
                {
                    var allCode = _AllCodeDAL.GetById(entity.Id);
                    if (allCode != null && allCode.Result != null)
                    {
                        var allCodeEntity = allCode.Result;
                        allCodeEntity.CodeValue = (short)allCodeEntity.Id;
                        allCodeEntity.OrderNo = (short)allCodeEntity.Id;
                        await _AllCodeDAL.UpdateAsync(allCodeEntity);
                    }
                }
                return entity.Id;
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("Create - AllCodeRepository" + ex);
                return -1;
            }
        }

        public Task<long> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public List<AllCode> GetAll()
        {
            return _AllCodeDAL.GetAll();
        }

        public async Task<AllCode> GetById(int Id)
        {
            return await _AllCodeDAL.GetById(Id);
        }

        public AllCode GetByType(string type)
        {
            return _AllCodeDAL.GetByType(type);
        }

        public List<AllCode> GetListAllAsync()
        {
            return GetListAllAsync().ToList();
        }

        public List<AllCode> GetListByType(string type)
        {
            return _AllCodeDAL.GetListByType(type);
        }

        public async Task<long> Update(AllCode model)
        {
            try
            {
                var entity = await _AllCodeDAL.GetById(model.Id);
                entity.CodeValue = model.CodeValue;
                entity.Description = model.Description;
                entity.OrderNo = model.CodeValue;
                entity.Type = model.Type;
                entity.UpdateTime = DateTime.Now;
                await _AllCodeDAL.UpdateAsync(entity);
                return model.Id;
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("Update - AllCodeRepository" + ex);
                return -1;
            }
        }
        public async Task<short> GetLastestCodeValueByType(string type)
        {
            return await _AllCodeDAL.GetLastestCodeValueByType(type);
        }
        public async Task<short> GetLastestOrderNoByType(string type)
        {
            return await _AllCodeDAL.GetLastestOrderNoByType(type);
        }
        public async Task<AllCode> GetIDIfValueExists(string type, string description)
        {
            return await _AllCodeDAL.GetIfDescriptionExists(type, description);
        }
        public async Task<List<AllCode>> GetListSortByName(string type_name)
        {
            return await _AllCodeDAL.GetListSortByName(type_name);
        }

        public async Task<T> GetAllCodeValueByType<T>(string apiPrefix, string keyToken, string key, string type)
        {
            HttpClient httpClient = new HttpClient();
            var j_param = new Dictionary<string, string> {
                    { key,type }
                };
            var token = CommonHelper.Encode(JsonConvert.SerializeObject(j_param), keyToken);
            var content = new FormUrlEncodedContent(new[] { new KeyValuePair<string, string>("token", token) });
            var response = await httpClient.PostAsync(apiPrefix, content);
            var contents = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(contents);
        }

        public async Task<T> Sendata<T>(string apiPrefix, string keyToken, Dictionary<string, string> keyValuePairs)
        {
            HttpClient httpClient = new HttpClient();

            var token = CommonHelper.Encode(JsonConvert.SerializeObject(keyValuePairs), keyToken);
            var content = new FormUrlEncodedContent(new[] { new KeyValuePair<string, string>("token", token) });
            var response = await httpClient.PostAsync(apiPrefix, content);
            var contents = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(contents);
        }

        public List<BankingAccount> GetBankingAccounts()
        {
            return bankingAccountDAL.GetAllBankingAccount();
        }
        public BankOnePay GetBankOnePayByBankName(string BankName)
        {
            return bankingAccountDAL.GetBankOnePayByBankName(BankName);
        }

        public List<BankingAccount> GetBankingAccountsBySupplierId(int supplierId)
        {
            return bankingAccountDAL.GetBankAccountDataTableBySupplierId(supplierId).ToList<BankingAccount>();
        }

        public async Task<List<AllCode>> GetListByCodeValueAsync(int codevalue)
        {
            return await _AllCodeDAL.GetListByCodeValueAsync(codevalue);
        }

        public async Task<List<AllCode>> GetAllCodeAsync() 
        {
            return await _AllCodeDAL.GetAllAsync();
        }

        public async Task<List<AllCode>> GetAllSortByIDAndType(int id,string type)
        {
            return await _AllCodeDAL.GetAllSortByIDAndType(id,type);
        }
    }
}
