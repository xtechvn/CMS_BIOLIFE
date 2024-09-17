using System;
using System.Collections.Generic;

namespace Entities.Models;

public partial class SupplierContact
{
    public long Id { get; set; }

    public int? SupplierId { get; set; }

    public string Name { get; set; }

    public string Mobile { get; set; }

    public string Email { get; set; }

    public string Position { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime CreateDate { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? UpdatedBy { get; set; }
}
