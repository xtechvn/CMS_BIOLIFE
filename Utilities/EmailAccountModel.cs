using System;
using System.Collections.Generic;
using System.Text;

namespace Utilities.ViewModels
{
    public class EmailAccountModel
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Display_name { get; set; }
    }
}
