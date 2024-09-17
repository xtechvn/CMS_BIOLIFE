using DAL;
using Entities.ConfigModels;
using Entities.Models;
using Entities.ViewModels.AccountAccessApiPermission;
using Microsoft.Extensions.Options;
using Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace Repositories.Repositories
{
    public class AccountAccessApiPermissionRepository : IAccountAccessApiPermissionRepository
    {
        private readonly AccountAccessApiPermissionDAL _permissionDAL;
        public AccountAccessApiPermissionRepository(IOptions<DataBaseConfig> dataBaseConfig) 
        {
            _permissionDAL = new AccountAccessApiPermissionDAL(dataBaseConfig.Value.SqlServer.ConnectionString);
        }

        public async Task<AccountAccessApiPermission> GetAccountAccessApiPermissionByID(int id)
        {
            return await _permissionDAL.FindAsync(id);
        }

        public async Task<List<AccountAccessApiPermission>> GetAllAccountAccessAPIPermissionAsync()
        {
            return await _permissionDAL.GetAllAsync();
        }

        public async Task<int> InsertAccountAccessApiPermission(AAAPSubmitModel model) 
        {
            try
            {
                return await _permissionDAL.InsertAccountAccessApiPermission(model);
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("Insert - AccountAccessAPIPermission" + ex);
                return -1;
            }
        }

        public async Task<int> UpdateAccountAccessApiPermission(AAAPSubmitModel model)
        {
            try
            {
                return await _permissionDAL.UpdateAccountAccessApiPermission(model);
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("Update - AccountAccessAPIPermission" + ex);
                return -1;
            }
        }
    }
}
