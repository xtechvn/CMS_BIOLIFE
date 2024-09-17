using System;
using System.Collections.Generic;

namespace Entities.Models;

public partial class TelegramDetail
{
    public int Id { get; set; }

    public string GroupLog { get; set; }

    public string Token { get; set; }

    public string GroupChatId { get; set; }

    public DateTime CreateDate { get; set; }

    public int ProjectType { get; set; }

    public int Status { get; set; }
}
