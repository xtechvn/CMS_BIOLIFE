
using Entities.Models;
using Entities.ViewModels.AccountAccessApiPermission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.IRepositories
{
    public interface IAccountAccessApiPermissionRepository
    {
        Task<int> InsertAccountAccessApiPermission(AAAPSubmitModel model);
        Task<int> UpdateAccountAccessApiPermission(AAAPSubmitModel model);
        Task<List<AccountAccessApiPermission>> GetAllAccountAccessAPIPermissionAsync();
        Task<AccountAccessApiPermission> GetAccountAccessApiPermissionByID(int id);
    }
}
