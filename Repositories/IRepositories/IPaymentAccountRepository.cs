using Entities.Models;
using Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.IRepositories
{
    public interface IPaymentAccountRepository
    {
        GenericViewModel<PaymentAccount> GetAllByClientId(long id, int currentPage, int pageSize);
        int CreatePaymentAccount(PaymentAccount model);
   
        int Delete(int Id);
        Task<PaymentAccount> getPaymentAccountById(int Id);
        int UpsertBankingAccount(BankingAccount model);
        BankingAccount GetBankingAccountById(int Id);
        int DeleteBankingAccountById(int id);
    }
}
