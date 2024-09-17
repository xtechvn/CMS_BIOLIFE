using Entities.Models;
using Entities.ViewModels;
using Entities.ViewModels.Funding;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repositories.IRepositories
{
    public interface IDepositHistoryRepository
    {
        List<DepositFunding> GetDepositHistories(FundingSearch searchModel, out long total, out List<CountStatus> countStatus, int currentPage = 1, int pageSize = 2);
        DepositFunding GetById(int depositHistoryId);
        DepositFunding GetByNo(string depositHistoryNo);
        GenericViewModel<DepositHistoryViewMdel> getDepositHistoryByUserId(long userId, int currentPage = 1, int pageSize = 20);
        List<ContractPay> GetContractPays();
        int DeleteContractPay(long contractPayId);
        string ExportDeposit(FundingSearch searchModel, string FilePath);
        List<DepositFunding> GetByClientId(long clientId, int payId = 0);
    }
}
