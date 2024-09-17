using System;
using System.Collections.Generic;

namespace Entities.Models;

public partial class ProvinceHotel
{
    public int Id { get; set; }

    public int ProvinceId { get; set; }

    public int? TotalOrder { get; set; }

    public int? Order { get; set; }

    public short? Status { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }
}
