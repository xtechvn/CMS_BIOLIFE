using DAL.Generic;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL
{
    public class BrandDAL : GenericService<Brand>
    {
        public BrandDAL(string connection) : base(connection)
        {
        }
    }
}
