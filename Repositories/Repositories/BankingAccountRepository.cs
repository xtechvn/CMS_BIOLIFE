using DAL;
using Entities.ConfigModels;
using Entities.Models;
using Microsoft.Extensions.Options;
using Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using Utilities;

namespace Repositories.Repositories
{
    public class BankingAccountRepository : IBankingAccountRepository
    {
        private readonly BankingAccountDAL bankingAccountDAL;
        private readonly IOptions<DataBaseConfig> dataBaseConfig;

        public BankingAccountRepository(IOptions<DataBaseConfig> _dataBaseConfig)
        {
            bankingAccountDAL = new BankingAccountDAL(_dataBaseConfig.Value.SqlServer.ConnectionString);
            dataBaseConfig = _dataBaseConfig;
        }

        public List<BankingAccount> GetBankAccountByClientId(int clientId)
        {
            List<BankingAccount> data;
            try
            {
                data = bankingAccountDAL.GetBankAccountByClientId(clientId).ToList<BankingAccount>();
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetBankAccountByClientId - BankingAccountRepository: " + ex);
                data = new List<BankingAccount>();
            }
            return data;
        }

        public BankingAccount GetById(int bankAccountId)
        {
            return bankingAccountDAL.GetById(bankAccountId);
        }
    }
}
