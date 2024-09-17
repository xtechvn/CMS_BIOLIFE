using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.ViewModels
{
    public class PaymentViewModel : Entities.Models.Payment
    {
        public string UserName { get; set; }
        public string ProductCode { get; set; }
        public string OrderNo { get; set; }
        public string PaymentTypeName { get; set; }
    }
}
