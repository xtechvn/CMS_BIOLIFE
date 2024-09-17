using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Contants;

namespace Entities.ViewModels.AccountAccessAPI
{
    public class AccountAccessApiSubmitModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }

        public string Password { get; set; }

        public AccountAccessAPIStatus Status { get; set; }
            
        public string Description { get; set; }
    }
}
