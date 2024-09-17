using System;
using System.Collections.Generic;

namespace Entities.Models;

public partial class Cashback
{
    public long Id { get; set; }

    public long OrderId { get; set; }

    public int UserId { get; set; }

    public DateTime CashbackDate { get; set; }

    public double Amount { get; set; }

    public string Note { get; set; }

    public DateTime? CreatedOn { get; set; }

    public DateTime? ModifiedOn { get; set; }
}
