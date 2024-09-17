using System;
using System.Collections.Generic;

namespace Entities.Models;

public partial class Position
{
    public int Id { get; set; }

    public string PositionName { get; set; }

    public int Width { get; set; }

    public int Height { get; set; }
}
