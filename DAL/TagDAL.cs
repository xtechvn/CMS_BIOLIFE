using DAL.Generic;
using DAL.StoreProcedure;
using Entities.Models;
using Entities.ViewModels;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Utilities;
using Utilities.Contants;

namespace DAL
{
    public class TagDAL : GenericService<Tag>
    {
        private static DbWorker _DbWorker;
        public TagDAL(string connection) : base(connection)
        {
            _DbWorker = new DbWorker(connection);
        }

        public async Task<List<long>> MultipleInsertTag(List<string> TagList)
        {
            var ListResult = new List<long>();
            try
            {
                using (var _DbContext = new EntityDataContext(_connection))
                {
                    using (var transaction = _DbContext.Database.BeginTransaction())
                    {
                        try
                        {
                            if (TagList != null && TagList.Count >= 0)
                            {
                                foreach (var item in TagList)
                                {
                                    var tagItemModel = await _DbContext.Tags.FirstOrDefaultAsync(s => s.TagName == item.Trim());
                                    if (tagItemModel == null)
                                    {
                                        var tagModel = new Tag()
                                        {
                                            TagName = item,
                                            CreatedOn = DateTime.Now
                                        };
                                        await _DbContext.Tags.AddAsync(tagModel);
                                        await _DbContext.SaveChangesAsync();
                                        ListResult.Add(tagModel.Id);
                                    }
                                    else
                                    {
                                        ListResult.Add(tagItemModel.Id);
                                    }
                                }
                            }
                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            return null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("MultipleInsertTag - TagDAL: " + ex);
                return null;
            }
            return ListResult;
        }

        public async Task<List<string>> GetSuggestionTag(string name)
        {
            try
            {
                SqlParameter[] objParam = new SqlParameter[4];
                objParam[0] = new SqlParameter("@TagName", name );
                objParam[1] = new SqlParameter("@TagIds", DBNull.Value);
                objParam[2] = new SqlParameter("@PageIndex", 1);
                objParam[3] = new SqlParameter("@PageSize", 10);

                DataTable dt = _DbWorker.GetDataTable(StoreProcedureConstant.SP_GetListTag, objParam);
                if (dt != null && dt.Rows.Count > 0)
                {
                    var data = dt.ToList<Tag>();
                    var list = data.Select(s => s.TagName).ToList();
                    return list;
                }
                //using (var _DbContext = new EntityDataContext(_connection))
                //{
                //    return await _DbContext.Tags.Where(s => s.TagName.Trim().ToLower().Contains(name.ToLower())).Select(s => s.TagName).Take(10).ToListAsync();
                //}
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetSuggestionTag - TagDAL: " + ex);
                return null;
            }
            return null;
        }
        public async Task<List<string>> GetTagByListID(List<long> tag_id_list)
        {
            try
            {
                SqlParameter[] objParam = new SqlParameter[2];
                objParam[0] = new SqlParameter("@TagName", DBNull.Value);
                objParam[1] = new SqlParameter("@TagIds", tag_id_list.ToString());
                objParam[2] = new SqlParameter("@PageIndex", -1);
                objParam[3] = new SqlParameter("@PageSize", -1);
                DataTable dt = _DbWorker.GetDataTable(StoreProcedureConstant.SP_GetListTag, objParam);
                if (dt != null && dt.Rows.Count > 0)
                {
                    var data = dt.ToList<Tag>();
                    var list= data.Select(s => s.TagName).ToList();
                    return list;
                }
                //using (var _DbContext = new EntityDataContext(_connection))
                //{
                //    return await _DbContext.Tags.Where(s => tag_id_list.Contains(s.Id)).Select(s=>s.TagName).ToListAsync();
                //}
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetTagByListID - TagDAL: " + ex);
                return null;
            }
            return null;
        }
    }
}
