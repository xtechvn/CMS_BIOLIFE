using System;
using System.Collections.Generic;

namespace Entities.Models;

public partial class UserAgent
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public long ClientId { get; set; }

    /// <summary>
    /// Quyền danh cho  0: Đối tác | 1: nhân viên của đối tác | 2: Saler phụ trách chính | 3: saler phụ trách cùng
    /// </summary>
    public short? MainFollow { get; set; }

    public DateTime? CreateDate { get; set; }

    public DateTime? UpdateLast { get; set; }

    public DateTime? VerifyDate { get; set; }

    public int? VerifyStatus { get; set; }

    public int? CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public virtual Client Client { get; set; }

    public virtual User User { get; set; }
}
