using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.ViewModels
{
   public class QueueSettingViewModel
    {
        public string host { get; set; }
        public int port { get; set; }
        public string v_host  { get; set; }
        public string username { get; set; }
        public string password { get; set; }
    }
}
