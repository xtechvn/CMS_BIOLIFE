using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.ViewModels.DashBoard
{
    public class DashboardSearchModel
    {
        public DateTime from_date { get; set; }
        public DateTime to_date { get; set; }
        public int date_type { get; set; }
        public int status { get; set; }
        public int type { get; set; }
    }
}
