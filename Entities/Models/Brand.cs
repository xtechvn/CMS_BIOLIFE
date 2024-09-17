using System;
using System.Collections.Generic;

namespace Entities.Models;

public partial class Brand
{
    public int BrandId { get; set; }

    public string BrandName { get; set; }

    public string BrandCode { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }
}
