using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.ViewModels.AccountAccessAPI
{
    public class AccountAccessApiViewModel
    {
        public int Id { get; set; } // của AccountAcessAPI
        public int Id_AccountAccessAPIPermission { get; set; }
        public int Id_AllCode { get; set; } 
        public string UserName { get; set; } // của AccountAcessAPI

        public string CodeName { get; set; } //Field này là Description của AllCode

        public short Status { get; set; } // của AccountAcessAPI

        public DateTime? CreateDate { get; set; } // của AccountAcessAPI

        public DateTime? UpdateLast { get; set; } // của AccountAcessAPI

        public string Description { get; set; } // của AccountAcessAPI
    }
}
