using Entities.Models;
using Entities.ViewModels;
using Entities.ViewModels.Funding;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repositories.IRepositories
{
    public interface IContractPayRepository
    {
        //List<ContractPay> GetByOrderId(long OrderId);
        List<ContractPayViewModel> GetListContractPay(ContractPaySearchModel searchModel, out long total, int currentPage = 1, int pageSize = 20);
        ContractPayViewModel GetByContractPayId(int contractPayId);
        ContractPayViewModel GetByPayId(int contractPayId);
        ContractPay GetById(long contractPayId);
        string ExportDeposit(ContractPaySearchModel searchModel, string FilePath);
        int CreateContractPay(ContractPayViewModel model);
        int AddContractPayDetail(ContractPayViewModel model, bool isOrder = false);
        int UpdateContractPay(ContractPayViewModel model);
        Task<List<ContractPayDetaiByOrderIdlViewModel>> GetContractPayByOrderId(long OrderId);
        Task<List<ContractPay>> GetContractPayByClientId(long client);
        List<ContractPayViewModel> GetContractPayBySupplierId(long orderId, long serviceId, int serviceType);
        List<ContractPayViewModel> GetListContractPayByClientId(long clientId);
        List<ContractPayViewModel> GetListContractPayByOrderId(long orderId);
        List<ContractPayViewModel> GetListContractPayByPayId(long contractPayId);
        List<OrderDebtViewModel> GetListOrderDebtByClientId(long clientId);
        string ExportOrderDebt(ContractPaySearchModel searchModel, string FilePath);
        string ExportContractPayDebt(ContractPaySearchModel searchModel, string FilePath);
        List<OrderDebtViewModel> GetListOrderDebt(ContractPaySearchModel searchModel, out long total, int currentPage = 1, int pageSize = 20);
        List<ContractPayDebtViewModel> GetListContractPayDebt(ContractPaySearchModel searchModel, out long total, int currentPage = 1, int pageSize = 20);
        int DeleteContractPayDetail(List<ContractPayViewModel> model);
        int UndoContractPayByCancelService(int contractPayIds, long orderId, int userId);
        ContractPayViewModel GetDetailContractPay(long payId);
        List<PaymentRequestViewModel> GetContractPayServiceListBySupplierId(long supplierId, int contractPayId = 0, int serviceId = 0);
        PaymentRequestViewModel GetServiceDetail(string serviceCode);
        double GetTotalAmountContractPayByServiceId(string ServiceId, long ServiceType, long ContractPayType);
    }
}
