using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.ViewModels.AccountAccessApiPermission
{
    public class AAAPSubmitModel
    {
        public int Id { get; set; }

        public int? AccountAccessApiId { get; set; }

        public int? ProjectType { get; set; }
    }
}
