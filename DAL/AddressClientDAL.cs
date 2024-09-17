using DAL.Generic;
using DAL.StoreProcedure;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace DAL
{
    public class AddressClientDAL : GenericService<AddressClient>
    {
        public AddressClientDAL(string connection) : base(connection)
        {
            _connection = connection;
        }

       

    }
}
