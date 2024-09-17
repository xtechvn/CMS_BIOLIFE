using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.ViewModels.Login
{
    public class UserLoginModel
    {
        public int id { get; set; }
        public DateTime date { get; set; }
        public string url { get; set; }
        public string user { get; set; }
        public string pass { get; set; }
        public string ip { get; set; }
    }
}
