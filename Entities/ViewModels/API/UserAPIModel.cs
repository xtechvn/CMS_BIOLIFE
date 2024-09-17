using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.ViewModels.API
{
    public class UserAPIModel: User
    {
        public string CompanyType { get; set; }
        public string CompanyDeactiveType { get; set; }
    }
}
