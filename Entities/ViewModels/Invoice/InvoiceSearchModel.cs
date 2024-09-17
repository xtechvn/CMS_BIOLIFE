using System;
using System.Collections.Generic;
using System.Text;
using Utilities;

namespace Entities.ViewModels.Invoice
{
    public class InvoiceSearchModel
    {
        public string InvoiceCode { get; set; }
        public string InvoiceNo { get; set; }
        public string OrderNo { get; set; }
        public string InvoiceRequestNo { get; set; }
        public DateTime? ExportDateFrom
        {
            get
            {
                return DateUtil.StringToDate(ExportDateFromStr);
            }
        }
        public string ExportDateFromStr { get; set; }
        public DateTime? ExportDateTo
        {
            get
            {
                return DateUtil.StringToDate(ExportDateToStr);
            }
        }
        public string ExportDateToStr { get; set; }
        public DateTime? CreateDateFrom
        {
            get
            {
                return DateUtil.StringToDate(CreateDateFromStr);
            }
        }
        public string CreateDateFromStr { get; set; }
        public DateTime? CreateDateTo
        {
            get
            {
                return DateUtil.StringToDate(CreateDateToStr);
            }
        }
        public string CreateDateToStr { get; set; }
        public DateTime? VerifyDateFrom
        {
            get
            {
                return DateUtil.StringToDate(VerifyDateFromStr);
            }
        }
        public string VerifyDateFromStr { get; set; }
        public DateTime? VerifyDateTo
        {
            get
            {
                return DateUtil.StringToDate(VerifyDateToStr);
            }
        }
        public string VerifyDateToStr { get; set; }
        public long ClientId { get; set; }
        public List<int> CreateByIds { get; set; }
        public List<int> VerifyByIds { get; set; }
    }
}
