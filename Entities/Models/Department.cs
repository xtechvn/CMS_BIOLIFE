using System;
using System.Collections.Generic;

namespace Entities.Models;

public partial class Department
{
    public int Id { get; set; }

    public string DepartmentName { get; set; }

    public string DepartmentCode { get; set; }

    public int? ParentId { get; set; }

    public string FullParent { get; set; }

    public bool? IsDelete { get; set; }

    public int? Status { get; set; }

    public int? Sort { get; set; }

    public string Description { get; set; }

    public int? Branch { get; set; }

    public bool? IsReport { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }
}
