using System;
using System.Collections.Generic;

namespace Entities.Models;

public partial class BankingAccount
{
    public int Id { get; set; }

    public string BankId { get; set; }

    public string AccountNumber { get; set; }

    public string AccountName { get; set; }

    public string Branch { get; set; }

    public int? SupplierId { get; set; }

    public int? ClientId { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public bool? IsDisplayWebsite { get; set; }
}
