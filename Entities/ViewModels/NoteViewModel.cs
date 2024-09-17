using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.ViewModels
{
    public class NoteViewModel : Note
    {
        public string UserName { get; set; }
    }

    public class NoteModel
    {
        public int UserId { get; set; }
        public int Type { get; set; }
        public string Comment { get; set; }
        public long NoteMapId { get; set; }
        public long OrderItemMapId { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateTime { get; set; }
    }
}
