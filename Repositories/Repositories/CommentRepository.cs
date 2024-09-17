using DAL;
using Entities.ConfigModels;
using Entities.ViewModels;
using Entities.ViewModels.Comment;
using Microsoft.Extensions.Options;
using PdfSharp;
using Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace Repositories.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly CommentDAL _commentDAL;
        public CommentRepository(IOptions<DataBaseConfig> dataBaseConfig)
        {
            _commentDAL = new CommentDAL(dataBaseConfig.Value.SqlServer.ConnectionString);
        }
        public async Task<GenericViewModel<CommentViewModel>> GetAllComment(CommentParamRequest request)
        {
            var model = new GenericViewModel<CommentViewModel>();
            try 
            {
                DataTable table = await _commentDAL.GetAllComment(request);
                model.ListData = (from row in table.AsEnumerable() select new CommentViewModel
                {
                    ClientName = row.Field<string>("ClientName"),
                    Content = row.Field<string>("Content"),
                    Email = row.Field<string>("Email"),
                    Phone = row.Field<string>("Phone"),
                    CreatedDate = row.Field<DateTime>("CreatedDate")
                }).ToList();

                model.CurrentPage = request.PageIndex;
                model.PageSize = request.PageSize;
                try
                {
                    // Thử chuyển đổi và gán giá trị
                    model.TotalRecord = Convert.ToInt32(table.Rows[0]["TotalRow"]);
                }
                catch (Exception ex)
                {
                    // Nếu có lỗi xảy ra (ví dụ: không tìm thấy cột, không thể chuyển đổi), gán giá trị mặc định là 0
                    model.TotalRecord = 0;
                }
                model.TotalPage = (int)Math.Ceiling((double)model.TotalRecord / request.PageSize);
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetAllComment - CommentRepository: " + ex);
            }
            return model;

        }
    }
}
