using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.ViewModels.TransferSms
{
   public class TransactionSMSViewModel: TransactionSMS
    {
        public string BankId { get; set; }
        public string AccountNumber { get; set; }
        public double SumAmount { get; set; }
        public string logo { get; set; }
        public int BankTransferType { get; set; }
   
    }
    public class BankingAccountTransactionSMs
    {
        public string BankId { get; set; }
        public string AccountNumber { get; set; }
        public double AccountName { get; set; }
        public string logo { get; set; }
        public string Amount { get; set; }
        public string ClientId { get; set; }
        public string UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public string CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string SupplierId { get; set; }
        public string Branch { get; set; }
        public double SumAmount { get; set; }

    }
    
}
