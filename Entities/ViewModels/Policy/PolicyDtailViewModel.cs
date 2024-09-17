using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.ViewModels.Policy
{
    public class PolicyDtailViewModel
    {
        public int PolicyId { get; set; }
        public string PolicyName { get; set; }
        public string PolicyCode { get; set; }
        public string EffectiveDate { get; set; }
        public int PermissionType { get; set; }
        public int Id { get; set; }
        public string ClientType { get; set; }
        public string DebtType { get; set; } 
        public double ProductFlyTicketDebtAmount { get; set; } 
        public double HotelDebtAmout { get; set; }
        public double ProductFlyTicketDepositAmount { get; set; }
        public double HotelDepositAmout { get; set; }
        public double VinWonderDebtAmount { get; set; }
        public double TourDebtAmount { get; set; }
        public double TouringCarDebtAmount { get; set; }
        public double VinWonderDepositAmount { get; set; }
        public double TourDepositAmount { get; set; }
        public double TouringCarDepositAmount { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public int UpdatedBy { get; set; }
        public string UpdatedDate { get; set; }
        public string PermissionTypeName { get; set; }
        public string ClientTypeName { get; set; }
        public string DebtTypeName { get; set; }
     
    }
    public class AddPolicyDtailViewModel
    {
        public int PolicyId { get; set; }
        public string PolicyName { get; set; }
        public string EffectiveDate { get; set; }
        public string PolicyCode { get; set; }
        public int PermissionType { get; set; }
        public int CreatedBy { get; set; }

        public List<PolicyDtailViewModel> extra_policy { get; set; }
    }
}
