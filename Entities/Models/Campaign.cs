using System;
using System.Collections.Generic;

namespace Entities.Models;

public partial class Campaign
{
    public int Id { get; set; }

    public string CampaignCode { get; set; }

    public DateTime FromDate { get; set; }

    public DateTime ToDate { get; set; }

    public int? ClientTypeId { get; set; }

    public string Description { get; set; }

    public byte Status { get; set; }

    public DateTime CreateDate { get; set; }

    public DateTime UpdateLast { get; set; }

    public long UserUpdateId { get; set; }

    public int UserCreateId { get; set; }

    public short? ContractType { get; set; }

    public virtual ICollection<RunningScheduleService> RunningScheduleServices { get; set; } = new List<RunningScheduleService>();
}
