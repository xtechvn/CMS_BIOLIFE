using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Entities.Models
{
    public partial class Notify
    {
        public int Id { get; set; }
        public int UserSend { get; set; }
        public string Message { get; set; }
        public DateTime SendDate { get; set; }
        public long DataId { get; set; }
        public short DataType { get; set; }
    }
}
