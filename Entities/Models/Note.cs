using System;
using System.Collections.Generic;

namespace Entities.Models;

public partial class Note
{
    public long Id { get; set; }

    public long DataId { get; set; }

    public int UserId { get; set; }

    public int? Type { get; set; }

    public string Comment { get; set; }

    public DateTime? CreateDate { get; set; }

    public DateTime? UpdateTime { get; set; }

    public long? NoteMapId { get; set; }
}
