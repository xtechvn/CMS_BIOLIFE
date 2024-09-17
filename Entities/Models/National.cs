using System;
using System.Collections.Generic;

namespace Entities.Models;

public partial class National
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string NameVn { get; set; }

    public string Code { get; set; }

    public DateTime? UpdateTime { get; set; }
}
