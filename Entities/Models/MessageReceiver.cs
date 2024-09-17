using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Entities.Models
{
    public partial class MessageReceiver
    {
        public long Id { get; set; }
        public int? ReceiverId { get; set; }
        public short? SeenStatus { get; set; }
        public int? NotifyId { get; set; }
        public DateTime? SeenDate { get; set; }
    }
}
