using System;
using System.Collections.Generic;

namespace Entities.Models;

public partial class InvoiceRequestHistory
{
    public long Id { get; set; }

    public int? InvoiceRequestId { get; set; }

    public string Actioin { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }
}
