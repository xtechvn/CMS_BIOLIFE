using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.ConfigModels
{
    public class MailConfig
    {
        public string Address { get; set; }
        public string Password { get; set; }

        public string Host { get; set; }
        public int Port { get; set; }
    }
}
