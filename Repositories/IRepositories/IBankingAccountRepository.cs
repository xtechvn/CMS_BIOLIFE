using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repositories.IRepositories
{
    public interface IBankingAccountRepository
    {
        BankingAccount GetById(int bankAccountId);
        List<BankingAccount> GetBankAccountByClientId(int clientId);
    }
}
