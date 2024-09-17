using DAL.Generic;
using DAL.StoreProcedure;
using Entities.Models;

namespace DAL
{
    public class DepartmentDAL : GenericService<Department>
    {
        private static DbWorker DbWorker;
        public DepartmentDAL(string connection) : base(connection)
        {
            DbWorker = new DbWorker(connection);
        }
       
    }
}
