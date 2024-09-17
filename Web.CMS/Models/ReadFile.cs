using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WEB.CMS.Models
{
    public class ReadFile
    {
        private static AppSettings _appconfig { get; set; }
        public static AppSettings LoadConfig()
        {
            if (_appconfig != null)
            {
                return _appconfig;
            }

            using (StreamReader r = new StreamReader("config.json"))
            {
                string json = r.ReadToEnd();
                _appconfig = JsonConvert.DeserializeObject<AppSettings>(json);
                return _appconfig;
            }
        }
    }
}
