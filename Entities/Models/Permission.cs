using System;
using System.Collections.Generic;

namespace Entities.Models;

public partial class Permission
{
    public int Id { get; set; }

    public string Name { get; set; }

    public int? SortOrder { get; set; }

    public int Status { get; set; }

    public DateTime? CreatedOn { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public virtual ICollection<Action> Actions { get; set; } = new List<Action>();

    public virtual ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
}
