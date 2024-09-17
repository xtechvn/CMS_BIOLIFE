using Microsoft.AspNetCore.Mvc;
using System;

namespace Entities.ViewModels
{
    public class MFAAccountViewModel
    {
        public int MFA_type { get; set; }
        public string MFA_token { get; set; }
        public string MFA_Code { get; set; }
        public DateTime MFA_timenow { get; set; }
        public string ReturnUrl { get; set; }

    }
}
