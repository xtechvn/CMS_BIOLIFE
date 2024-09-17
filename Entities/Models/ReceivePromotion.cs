using System;
using System.Collections.Generic;

namespace Entities.Models;

public partial class ReceivePromotion
{
    public int Id { get; set; }

    public string Email { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }
}
