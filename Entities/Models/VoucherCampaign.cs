using System;
using System.Collections.Generic;

namespace Entities.Models;

public partial class VoucherCampaign
{
    public int Id { get; set; }

    public string CampaignVoucher { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }
}
