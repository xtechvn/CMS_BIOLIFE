using DAL.Generic;
using DAL.StoreProcedure;
using Entities.Models;
using Entities.ViewModels.Comment;
using Microsoft.Data.SqlClient;
using PdfSharp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Contants;

namespace DAL
{
    public class CommentDAL : GenericService<Comment>
    {
        private static DbWorker _DbWorker;
        public CommentDAL(string connection) : base(connection)
        {
            _DbWorker = new DbWorker(connection);
        }

        public async Task<DataTable> GetAllComment(CommentParamRequest request)
        {
            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@ClientID", request.ClientID != null ? request.ClientID : DBNull.Value),
                new SqlParameter("@CreateDateFrom", request.CreateDateFrom != DateTime.MinValue ? request.CreateDateFrom : DBNull.Value),
                new SqlParameter("@CreateDateTo", request.CreateDateTo != DateTime.MinValue ? request.CreateDateTo : DBNull.Value ),
                new SqlParameter("@PageIndex", request.PageIndex != null ? request.PageIndex : DBNull.Value),
                new SqlParameter("@PageSize", request.PageSize != null ? request.PageSize : DBNull.Value) 
            };
/*            List<CommentViewModel> Comments = lstObj.AsEnumerable().Select(row => new CommentViewModel
            {
                ClientName = row.Field<string>("ClientName"),
                Content = row.Field<string>("Content"),
                Email = row.Field<string>("Email"),
                Phone = row.Field<string>("Phone"),
                CreatedDate = row.Field<DateTime>("CreatedDate")
            }).ToList();*/
            return _DbWorker.GetDataTable(ProcedureConstants.Sp_GetListComments, sqlParameters);
        }
    }
}
