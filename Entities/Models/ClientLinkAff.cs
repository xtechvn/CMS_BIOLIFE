using System;
using System.Collections.Generic;

namespace Entities.Models;

public partial class ClientLinkAff
{
    public int Id { get; set; }

    public DateTime CreateDate { get; set; }

    public string LinkAff { get; set; }

    public long ClientId { get; set; }
}
