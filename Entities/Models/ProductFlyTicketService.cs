using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Entities.Models
{
    public partial class ProductFlyTicketService
    {
        public int Id { get; set; }
        public int CampaignId { get; set; }
        public string GroupProviderType { get; set; }

        public virtual Campaign Campaign { get; set; }
    }
}
