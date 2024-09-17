using Entities.Models;
using System;
using System.Collections.Generic;
using Utilities;

namespace Entities.ViewModels.Funding
{
    public class DepositFunding : DepositHistory
    {
        public int Index { get; set; }
        public string ServiceTypeStr { get; set; }
        public string TransTypeStr { get; set; }
        public string PaymentTypeStr { get; set; }
        public DateTime? TransactionDate { get; set; }
        public string TransactionDateStr
        {
            get
            {
                if (ApproveDate != null)
                    return ApproveDate.ToString("yyyy/MM/dd hh:mm:ss");

                return "";
            }
        }
        public string Email { get; set; }
        public string EmailVerify { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ApproveDate { get; set; }
        public string ApproveDateStr
        {
            get
            {
                if (ApproveDate != null)
                    return ApproveDate.ToString("yyyy/MM/dd hh:mm:ss");

                return "";
            }
        }
        public string Approver { get; set; }
        public string StatusStr { get; set; }
        public double TotalDeposit { get; set; }
        public List<CountStatus> countStatus { get; set; }
        public string ClientName { get; set; }
        public string ClientEmail { get; set; }
        public string Mobile { get; set; }
        public List<Models.Payment> Payments { get; set; }
        public List<ContractPayDetailViewModel> ContractPays { get; set; }
        public string ServiceName { get; set; }
        public string UserName { get; set; }
        public double TotalDisarmed { get; set; }
        public double TotalNeedPayment { get; set; }
        public int PayDetailId { get; set; }
        public bool IsChecked { get; set; }
        public double Payment { get; set; }
    }
}
