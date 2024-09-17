using DAL.Generic;
using Entities.Models;
using Entities.ViewModels.GroupProducts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities;
using Utilities.Contants;

namespace DAL
{
    public class GroupProductDAL : GenericService<GroupProduct>
    {
        public GroupProductDAL(string connection) : base(connection)
        {
        }

        /// <summary>
        /// Delete Group Product
        /// </summary>
        /// <param name="Id"></param>
        /// <returns>
        ///  0 : errors
        /// -1 : is used
        /// -2 : has child
        /// >0 : success
        /// </returns>
        public async Task<int> DeleteGroupProduct(int Id)
        {
            try
            {
                using (var _DbContext = new EntityDataContext(_connection))
                {
                    var IsHasChild = _DbContext.GroupProducts.Any(s => s.ParentId == Id);

                   
                    if (IsHasChild)
                    {
                        return -2;
                    }

                    var entity = await FindAsync(Id);
                    _DbContext.GroupProducts.Remove(entity);
                    await _DbContext.SaveChangesAsync();
                    return Id;
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("DeleteGroupProduct - GroupProductDAL: " + ex);
                return 0;
            }
        }

       

        public async Task<List<GroupProduct>> getCategoryDetailByCategoryId(int[] category_id)
        {
            try
            {
                using (var _DbContext = new EntityDataContext(_connection))
                {
                    var group_product = _DbContext.GroupProducts.AsNoTracking().Where(s => category_id.Contains(s.Id)).ToListAsync();

                    return await group_product;
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("getCategoryDetailByCategoryId - GroupProductDAL: " + ex);
                return null;
            }
        }
       
        
       
       
       

        /// <summary>
        /// cuonglv
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public async Task<List<GroupProduct>> getAllGroupProduct()
        {
            try
            {
                using (var _DbContext = new EntityDataContext(_connection))
                {
                    var group_product = _DbContext.GroupProducts.AsNoTracking().Where(s => s.Status == (int)StatusType.BINH_THUONG).ToListAsync();

                    return await group_product;
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("getAllGroupProduct - GroupProductDAL: " + ex);
                return null;
            }
        }

        /// <summary>
        /// cuonglv
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public async Task<GroupProduct> getDetailByPath(string path)
        {
            try
            {
                using (var _DbContext = new EntityDataContext(_connection))
                {
                    var group_product = await _DbContext.GroupProducts.AsNoTracking().FirstOrDefaultAsync(s => s.Status == (int)StatusType.BINH_THUONG && s.Path == path.Trim());
                    return group_product;
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("getDetailByPath - GroupProductDAL: " + ex);
                return null;
            }
        }

        public async Task<bool> IsGroupHeader(List<int> groups)
        {
            try
            {
                using (var _DbContext = new EntityDataContext(_connection))
                {
                    var group_product = await _DbContext.GroupProducts.AsNoTracking().Where(s => s.Status == (int)StatusType.BINH_THUONG && groups.Contains(s.Id) && s.IsShowFooter==true).ToListAsync();
                    if (group_product != null && group_product.Count > 0)
                    {
                        return false;
                    }
                    else return true;
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("IsGroupHeader - GroupProductDAL: " + ex);
                return false;
            }
        }
        public List<GroupProduct> GetByParentId(long parent_id)
        {
            try
            {
                using (var _DbContext = new EntityDataContext(_connection))
                {
                    return _DbContext.GroupProducts.Where(s => s.ParentId == parent_id && s.Status == (int)ArticleStatus.PUBLISH).ToList();
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetByParentId - GroupProductDAL: " + ex);

            }
            return null;
        }


    }
}
