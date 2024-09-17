using System;
using System.Collections.Generic;

namespace Entities.Models;

public partial class AffiliateGroupProduct
{
    public int Id { get; set; }

    public int GroupProductId { get; set; }

    public int AffType { get; set; }
}
