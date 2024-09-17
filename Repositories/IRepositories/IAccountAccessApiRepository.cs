using Entities.Models;
using Entities.ViewModels.AccountAccessAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.IRepositories
{
    public interface IAccountAccessApiRepository
    {
        Task<int> InsertAccountAccessAPI(AccountAccessApiSubmitModel model);
        Task<int> UpdateAccountAccessAPI(AccountAccessApiSubmitModel model);
        Task<List<AccountAccessApiViewModel>> GetAllAccountAccessAPI();
        Task<AccountAccessApiViewModel> GetAccountAccessApiByID(int id);
        Task<int> ResetPassword(int id);
    }
}
