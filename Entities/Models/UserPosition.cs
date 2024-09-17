using System;
using System.Collections.Generic;

namespace Entities.Models;

public partial class UserPosition
{
    public int Id { get; set; }

    public string Name { get; set; }

    public short Rank { get; set; }

    public short? OrderBy { get; set; }
}
