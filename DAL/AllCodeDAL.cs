using DAL.Generic;
using DAL.StoreProcedure;
using Entities.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;
using Utilities.Contants;

namespace DAL
{
    public class AllCodeDAL : GenericService<AllCode>
    {
        private static DbWorker _DbWorker;
        public AllCodeDAL(string connection) : base(connection) 
        {
            _DbWorker = new DbWorker(connection);
        }

        public async Task<List<AllCode>> GetListByCodeValueAsync(int codevalue)
        {
            try
            {
                using (var _DbContext = new EntityDataContext(_connection))
                {
                    var detail = await _DbContext.Set<AllCode>().Where(n => n.CodeValue == codevalue).ToListAsync();
                    if (detail != null)
                    {
                        return detail;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetListByCodeValue - AllCodeDAL. " + ex);
                return null;
            }
        }

        public async Task<List<AllCode>> GetAllSortByIDAndType(int id ,string type)
        {
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[]
                {
                    new SqlParameter("@Ids", DBNull.Value),
                    new SqlParameter("@Type", type != null ? type : DBNull.Value)
                };

                var lstObj = _DbWorker.GetDataTable(StoreProcedureConstant.Sp_GetListAllCodeByTypeAndIds, sqlParameters);
                List<AllCode> allCodes = lstObj.AsEnumerable().Select(row => new AllCode
                {
                    Id = row.Field<int>("Id"),
                    CodeValue = row.Field<short>("CodeValue"),
                    Description = row.Field<string>("Description"),
                    OrderNo = row.Field<short>("OrderNo")
                }).ToList();

                // Tìm đối tượng có Id trùng với id
                var itemToMoveToTop = allCodes.FirstOrDefault(x => x.Id == id);

                // Nếu tìm thấy đối tượng, xóa khỏi danh sách và thêm vào đầu
                if (itemToMoveToTop != null)
                {
                    allCodes.Remove(itemToMoveToTop);
                    allCodes.Insert(0, itemToMoveToTop);
                }

                return allCodes;
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetListByName - AllCodeDAL. " + ex);
                return null;
            }
        }



        public List<AllCode> GetListByType(string type)
        {
            try
            {
                using (var _DbContext = new EntityDataContext(_connection))
                {
                    var detail = _DbContext.Set<AllCode>().Where(n => n.Type == type).ToList();
                    if (detail != null)
                    {
                        return detail;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetListByType - AllCodeDAL. " + ex);
                return null;
            }
        }

        public AllCode GetByType(string type)
        {
            try
            {
                using (var _DbContext = new EntityDataContext(_connection))
                {
                    var detail = _DbContext.Set<AllCode>().Where(n => n.Type == type).FirstOrDefault();
                    if (detail != null)
                    {
                        return detail;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetByType - AllCodeDAL. " + ex);
                return null;
            }
        }
        public async Task<AllCode> GetById(int id)
        {
            try
            {
                using (var _DbContext = new EntityDataContext(_connection))
                {
                    var detail = _DbContext.AllCodes.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
                    if (detail != null)
                    {
                        return await detail;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetById - AllCodeDAL: " + ex);
                return null;
            }
        }
        public async Task<short> GetLastestCodeValueByType(string type)
        {
            try
            {
                using (var _DbContext = new EntityDataContext(_connection))
                {
                    var detail = await _DbContext.AllCodes.AsNoTracking().Where(x => x.Type == type).OrderByDescending(x => x.CodeValue).FirstOrDefaultAsync();
                    if (detail != null)
                    {
                        return  detail.CodeValue;
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetLastestCodeValueByType - AllCodeDAL: " + ex);
            }
            return -1;
        }
        public async Task<short> GetLastestOrderNoByType(string type)
        {
            try
            {
                using (var _DbContext = new EntityDataContext(_connection))
                {
                    var detail = await _DbContext.AllCodes.AsNoTracking().Where(x => x.Type == type).OrderByDescending(x => x.OrderNo).FirstOrDefaultAsync();
                    if (detail != null)
                    {
                        return detail.OrderNo!=null?(short)detail.OrderNo: (short)-1;
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetLastestOrderNoByType - AllCodeDAL: " + ex);
            }
            return -1;
        }
        public async Task<AllCode> GetIDIfValueExists(string type, string description)
        {
            try
            {
                using (var _DbContext = new EntityDataContext(_connection))
                {
                    var detail = await _DbContext.AllCodes.AsNoTracking().Where(x => x.Type == type && x.Description==description).FirstOrDefaultAsync();
                    if (detail != null)
                    {
                        return detail;
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetIDIfValueExists - AllCodeDAL: " + ex);
            }
            return null;
        }
        public async Task<List<AllCode>> GetListSortByName(string type_name)
        {
            try
            {
                using (var _DbContext = new EntityDataContext(_connection))
                {
                    var detail = await _DbContext.AllCodes.AsNoTracking().Where(n => n.Type == type_name).OrderBy(x=>x.Description).ToListAsync();
                    if (detail != null)
                    {
                        return detail;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetListSortByName - AllCodeDAL. " + ex);
                return null;
            }
        }
        public async Task<AllCode> GetIfDescriptionExists(string type, string description)
        {
            try
            {
                using (var _DbContext = new EntityDataContext(_connection))
                {
                    var detail = await _DbContext.AllCodes.AsNoTracking().Where(x => x.Type == type && x.Description.ToLower().Contains(description.Trim().ToLower())).FirstOrDefaultAsync();
                    if (detail != null)
                    {
                        return detail;
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetIDIfValueExists - AllCodeDAL: " + ex);
            }
            return null;
        }
    }
}
