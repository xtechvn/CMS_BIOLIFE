using System;
using System.Collections.Generic;

namespace Entities.Models;

public partial class Label
{
    public int Id { get; set; }

    public string StoreName { get; set; }

    public DateTime? CreateTime { get; set; }

    public DateTime? UpdateTime { get; set; }

    public string Icon { get; set; }

    public string PrefixOrderCode { get; set; }

    public string Domain { get; set; }

    public string DescExpire { get; set; }

    public short? Status { get; set; }
}
