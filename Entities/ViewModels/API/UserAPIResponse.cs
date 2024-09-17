using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.ViewModels.API
{
    public class UserAPIResponse
    {
        public int status { get; set; }
        public string msg { get; set; }
        public int user_id { get; set; }

    }
    public class UserAPIDetail
    {
        public int status { get; set; }
        public string msg { get; set; }
        public List<UserAPIUserDetail> data { get; set; }

    }
    public class UserAPIUserDetail: User
    {
        public string CompanyType { get; set; }
    }
}
