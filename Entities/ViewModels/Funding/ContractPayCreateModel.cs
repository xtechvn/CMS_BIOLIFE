using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.ViewModels.Funding
{
    public class ContractPayCreateModel
    {
        public DateTime export_date { get; set; }
        public double total_amount { get; set; }
        public string contract_pay_code { get; set; }
        public List<ContractPayCreateModelOrder> data { get; set; }
    }
    public class ContractPayCreateModelOrder
    {
        public long data_id { get; set; }
        public double amount { get; set; }
        public string note { get; set; }

    }
}
