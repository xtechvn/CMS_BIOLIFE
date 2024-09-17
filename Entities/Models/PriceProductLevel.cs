using System;
using System.Collections.Generic;

namespace Entities.Models;

public partial class PriceProductLevel
{
    public int PriceId { get; set; }

    public double Offset { get; set; }

    public double Limit { get; set; }

    public DateTime? CreateDate { get; set; }

    public DateTime? FromDate { get; set; }

    public DateTime? ToDate { get; set; }

    public string LabelId { get; set; }

    public double Price { get; set; }

    public int? FeeType { get; set; }

    public double? Discount { get; set; }

    public string Note { get; set; }
}
