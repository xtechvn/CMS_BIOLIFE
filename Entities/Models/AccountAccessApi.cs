using System;
using System.Collections.Generic;

namespace Entities.Models;

public partial class AccountAccessApi
{
    public int Id { get; set; }

    public string UserName { get; set; }

    public string Password { get; set; }

    public short Status { get; set; }

    public DateTime? CreateDate { get; set; }

    public DateTime? UpdateLast { get; set; }

    public string Description { get; set; }

    public virtual ICollection<AccountAccessApiPermission> AccountAccessApiPermissions { get; set; } = new List<AccountAccessApiPermission>();
}
