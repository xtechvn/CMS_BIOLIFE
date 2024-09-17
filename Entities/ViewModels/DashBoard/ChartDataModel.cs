using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.ViewModels.DashBoard
{
    public class ChartDataModel
    {
        public DateTime? Date { get; set; }
        public decimal TotalRevenue { get; set; }
        public string DimName { get; set; }
    }
}
