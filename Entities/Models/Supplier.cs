using System;
using System.Collections.Generic;

namespace Entities.Models;

public partial class Supplier
{
    public int SupplierId { get; set; }

    public string SupplierCode { get; set; }

    public string FullName { get; set; }

    public string ShortName { get; set; }

    public string Email { get; set; }

    public string Phone { get; set; }

    public int? ProvinceId { get; set; }

    public string TaxCode { get; set; }

    public int? EstablishedYear { get; set; }

    public int? RatingStar { get; set; }

    public string Address { get; set; }

    public int? ChainBrands { get; set; }

    public int? VerifyDate { get; set; }

    public int? SalerId { get; set; }

    public string Description { get; set; }

    public string ServiceType { get; set; }

    public string ResidenceType { get; set; }

    public bool? IsDisplayWebsite { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }
}
