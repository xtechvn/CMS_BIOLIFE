using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.ViewModels.SupplierConfig
{
    public class SupplierContactViewModel : SupplierContact
    {
        public string UserCreate { get; set; }
        public string UserUpdate { get; set; }
        public int TotalRow { get; set; }
    }
}
