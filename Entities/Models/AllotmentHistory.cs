using System;
using System.Collections.Generic;

namespace Entities.Models;

public partial class AllotmentHistory
{
    public int Id { get; set; }

    public double AmountMove { get; set; }

    public long AccountClientId { get; set; }

    public DateTime CreateDate { get; set; }

    public double AccountBalance { get; set; }

    public int FundTypeFrom { get; set; }

    public int AllotmentFundId { get; set; }

    public string Description { get; set; }

    public int? FundTypeTo { get; set; }

    public int? PaymentType { get; set; }

    public virtual AllotmentFund AllotmentFund { get; set; }
}
