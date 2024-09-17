using System;
using System.Collections.Generic;
using System.Text;
using Utilities;

namespace Entities.ViewModels
{
    public class ChartRevenuViewModel
    {
        public double TotalRevenu { get; set; }//doanh thu 
        public double TotalShipFee { get; set; } // Phi mua ho
        public string StoreName { get; set; }//doanh thu theo label
        public int OrderCount { get; set; }//so order
        public DateTime? Date { get; set; }//ngay
        public string DateStr
        {
            get
            {
                return DateUtil.DateToString(Date);
            }
            set
            {
                Date = DateUtil.StringToDate(value);
            }
        }
        public double TotalRevenuPass { get; set; } //doanh thu cung ngay truoc do 
        public double TotalShipFeePass { get; set; } // Phi mua ho cung ngay truoc do 
        public string StoreNamePass { get; set; }//doanh thu label cung ngay truoc do
        public int OrderCountPass { get; set; }//so don hang cung ngay truoc do
        public DateTime? DatePass { get; set; }// ngay  doanh thu truoc do
        public string DatePassStr
        {
            get
            {
                return DateUtil.DateToString(DatePass);
            }
            set
            {
                DatePass = DateUtil.StringToDate(value);
            }
        }
    }
}
