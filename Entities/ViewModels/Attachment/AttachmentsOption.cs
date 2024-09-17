using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.ViewModels.Attachment
{
    public class AttachmentsOption
    {
        public bool allow_edit { get; set; } = true;
        public bool allow_preview { get; set; } = false;
        public bool separate_confirm { get; set; } = false;
    }
}
