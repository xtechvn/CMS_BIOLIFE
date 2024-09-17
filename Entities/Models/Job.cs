using System;
using System.Collections.Generic;

namespace Entities.Models;

public partial class Job
{
    public long Id { get; set; }

    /// <summary>
    /// 1: sync client ; 2 : sync order
    /// </summary>
    public int? Type { get; set; }

    public long? DataId { get; set; }

    public int? SubType { get; set; }
}
