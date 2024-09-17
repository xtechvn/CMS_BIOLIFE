using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.ConfigModels
{
    public class DataBaseConfig
    {
        public DBConfig SqlServer { get; set; }
        public IPConfig Redis { get; set; }
        public IPConfig Elastic { get; set; }
    }

    public class DBConfig
    {
        public string ConnectionString { get; set; }
    }

    public class IPConfig
    {
        public string Host { get; set; }
    }

    public class DomainConfig
    {
        public string ImageStatic { get; set; }
    }
}
