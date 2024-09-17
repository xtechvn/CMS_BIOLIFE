using DAL;
using Entities.ConfigModels;
using Entities.Models;
using Entities.ViewModels.AccountAccessAPI;
using Microsoft.Extensions.Options;
using Nest;
using Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace Repositories.Repositories
{
    public class AccountAccessApiRepository : IAccountAccessApiRepository
    {
        private readonly AccountAccessApiDAL _accountAccessApiDAL;
        public AccountAccessApiRepository(IOptions<DataBaseConfig> dataBaseConfig)
        {
            _accountAccessApiDAL = new AccountAccessApiDAL(dataBaseConfig.Value.SqlServer.ConnectionString);
        }

        public async Task<AccountAccessApiViewModel> GetAccountAccessApiByID(int id)
        {
            try
            {
                var lst = await _accountAccessApiDAL.GetAllAccountAccessAPI();
                return lst.FirstOrDefault(x => x.Id == id);
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetAccountAccessApiByID - AccountAccessAPI" + ex);
                return null;
            }
        }

        public async Task<List<AccountAccessApiViewModel>> GetAllAccountAccessAPI()
        {
            try
            {
                return await _accountAccessApiDAL.GetAllAccountAccessAPI();
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetAccountAccessApis - AccountAccessAPI" + ex);
                return null;
            }
        }

        public async Task<int> InsertAccountAccessAPI(AccountAccessApiSubmitModel model)
        {
            try 
            {
                return await _accountAccessApiDAL.InsertAccountAccessAPI(model);
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("Insert - AccountAccessAPI" + ex);
                return -1;
            }
        }

        public async Task<int> ResetPassword(int id)
        {
            try
            {
                return await _accountAccessApiDAL.ResetPassword(id);
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("ResetPassword - AccountAccessAPI" + ex);
                return -1;
            }
        }

        public async Task<int> UpdateAccountAccessAPI(AccountAccessApiSubmitModel model)
        {
            try
            {
                return await _accountAccessApiDAL.UpdateAccountAccessAPI(model);
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("Update - AccountAccessAPI" + ex);
                return -1;
            }
        }
    }
}
