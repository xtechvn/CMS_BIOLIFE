using System;
using System.Collections.Generic;

namespace Entities.Models;

public partial class Client
{
    public long Id { get; set; }

    public int? ClientMapId { get; set; }

    public int? SaleMapId { get; set; }

    public int? ClientType { get; set; }

    public string ClientName { get; set; }

    public string Email { get; set; }

    public int? Gender { get; set; }

    public int Status { get; set; }

    public string Note { get; set; }

    public string Avartar { get; set; }

    public DateTime JoinDate { get; set; }

    public bool? IsReceiverInfoEmail { get; set; }

    public string Phone { get; set; }

    public DateTime? Birthday { get; set; }

    public DateTime? UpdateTime { get; set; }

    public string TaxNo { get; set; }

    public int? AgencyType { get; set; }

    public int? PermisionType { get; set; }

    public string BusinessAddress { get; set; }

    public string ExportBillAddress { get; set; }

    public string ClientCode { get; set; }

    public bool? IsRegisterAffiliate { get; set; }

    public string ReferralId { get; set; }

    public int? ParentId { get; set; }

    public virtual ICollection<UserAgent> UserAgents { get; set; } = new List<UserAgent>();
}
