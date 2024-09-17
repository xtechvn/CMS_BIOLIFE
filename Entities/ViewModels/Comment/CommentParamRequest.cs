using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.ViewModels.Comment
{
    public class CommentParamRequest
    {
        public string ClientID { get; set; }
        public DateTime CreateDateFrom { get; set; }
        public DateTime CreateDateTo { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}
