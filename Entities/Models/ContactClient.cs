using System;
using System.Collections.Generic;

namespace Entities.Models;

public partial class ContactClient
{
    public long Id { get; set; }

    public string Name { get; set; }

    public string Mobile { get; set; }

    public string Email { get; set; }

    public DateTime CreateDate { get; set; }

    public long ClientId { get; set; }

    public long? OrderId { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
