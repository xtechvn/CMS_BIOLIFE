using System;
using System.Collections.Generic;

namespace Entities.Models;

public partial class Mfauser
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string Username { get; set; }

    public string Email { get; set; }

    public short Status { get; set; }

    public string SecretKey { get; set; }

    public string BackupCode { get; set; }

    public string UserCreatedYear { get; set; }

    public DateTime? UpdateTime { get; set; }
}
