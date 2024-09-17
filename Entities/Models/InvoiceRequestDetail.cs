using System;
using System.Collections.Generic;

namespace Entities.Models;

public partial class InvoiceRequestDetail
{
    public long Id { get; set; }

    public long? InvoiceRequestId { get; set; }

    public string ProductName { get; set; }

    public string Unit { get; set; }

    public int? Quantity { get; set; }

    public double? Price { get; set; }

    public double? PriceVat { get; set; }

    public double? PriceExtra { get; set; }

    public double? PriceExtraExport { get; set; }

    public int? Vat { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }
}
