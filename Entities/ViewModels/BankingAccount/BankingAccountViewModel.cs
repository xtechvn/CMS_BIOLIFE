using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.ViewModels.BankingAccount
{
   public class BankingAccountViewModel : Entities.Models.BankingAccount
    {
        public double Amount { get; set; }
        public double SumAmount { get; set; }
        public double SumAmountTR { get; set; }
    }
}
