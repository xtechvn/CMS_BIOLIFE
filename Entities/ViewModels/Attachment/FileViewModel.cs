using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ENTITIES.ViewModels.AttachFiles
{
    public class FileViewModel
    {
        public IFormFile data { get; set; }
        public string name { get; set; }
        public long data_id { get; set; }
        public int type { get; set; }
        public string token { get; set; }
    }
}
