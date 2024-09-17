using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.ViewModels.OrderManual
{
    public class ClientOrderManualSuggestion
    {
        public long Id { get; set; }

        public int? ClientType { get; set; }
        public string ClientName { get; set; }
        public string Email { get; set; }
        public int? Gender { get; set; }
        public string Avartar { get; set; }
        public string Phone { get; set; }
        public DateTime? Birthday { get; set; }
        
    }
}
