using System;
using System.Collections.Generic;

namespace Entities.Models;

public partial class AttachFile
{
    public long Id { get; set; }

    public long DataId { get; set; }

    public int UserId { get; set; }

    public int? Type { get; set; }

    public string Path { get; set; }

    public string Ext { get; set; }

    public double? Capacity { get; set; }

    public DateTime? CreateDate { get; set; }
}
