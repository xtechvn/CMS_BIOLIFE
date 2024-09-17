using Entities.Models;
using Entities.ViewModels;
using Entities.ViewModels.CustomerManager;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.IRepositories
{
    public interface ICustomerManagerRepository
    {
        //GenericViewModel<CustomerManagerViewModel> GetAllClient(string input,int currentPage , int pageSize);
        Task<CustomerManagerViewModel> GetDetailClient(long ClientId);
        int SetUpClient(CustomerManagerView model);

        Task<GenericViewModel<CustomerManagerViewModel>> GetPagingList(CustomerManagerViewSearchModel searchModel, int currentPage, int pageSize);
        int ResetStatusAc(long clientId, long Status,int type);
        Task<AmountRemainView> GetAmountRemainOfContractByClientId(long ClientId);
        Task<string> ExportDeposit(CustomerManagerViewSearchModel searchModel, string FilePath, field field);
    }
}
