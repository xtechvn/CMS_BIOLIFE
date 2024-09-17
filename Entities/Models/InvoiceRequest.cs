using System;
using System.Collections.Generic;

namespace Entities.Models;

public partial class InvoiceRequest
{
    public long Id { get; set; }

    public string InvoiceRequestNo { get; set; }

    public int? ClientId { get; set; }

    public DateTime? PlanDate { get; set; }

    public string TaxNo { get; set; }

    public string CompanyName { get; set; }

    public string Address { get; set; }

    public long? OrderId { get; set; }

    public string AttachFile { get; set; }

    public int? UserVerify { get; set; }

    public DateTime? VerifyDate { get; set; }

    public string Note { get; set; }

    public int? Status { get; set; }

    public string DeclineReason { get; set; }

    public bool? IsDelete { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }
}
