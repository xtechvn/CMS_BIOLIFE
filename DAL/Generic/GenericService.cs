using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Serilog.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace DAL.Generic
{
    public class GenericService<TEntity> : IGenericService<TEntity> where TEntity : class
    {
        public static string _connection;

        public GenericService(string connection)
        {
            _connection = connection;
        }

        public long Create(TEntity entity)
        {
            try
            {
                using (var _DbContext = new EntityDataContext(_connection))
                {
                    _DbContext.Set<TEntity>().Add(entity);
                    _DbContext.SaveChangesAsync();
                    return Convert.ToInt64(entity.GetType().GetProperty("Id").GetValue(entity, null));
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("Create in GenericService : " + JsonConvert.SerializeObject(entity) + " => " + ex.ToString());
                return 0;
            }
        }
        public async Task<long> CreateAsync(TEntity entity)
        {
            try
            {
                using (var _DbContext = new EntityDataContext(_connection))
                {
                    await _DbContext.Set<TEntity>().AddAsync(entity);
                    await _DbContext.SaveChangesAsync();
                    return Convert.ToInt64(entity.GetType().GetProperty("Id").GetValue(entity, null));
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("CreateAsync in GenericService: " + JsonConvert.SerializeObject(entity) + " => " + ex.ToString());
                return 0;
            }
        }


        public void Delete(object id)
        {
            try
            {
                using (var _DbContext = new EntityDataContext(_connection))
                {
                    var entity = Find(id);
                    _DbContext.Set<TEntity>().Remove(entity);
                    _DbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("Delete in GenericService => " + typeof(TEntity).Name + " : " + id + " => " + ex.ToString());
            }
        }

        public async Task DeleteAsync(object id)
        {
            try
            {
                using (var _DbContext = new EntityDataContext(_connection))
                {
                    var entity = await FindAsync(id);
                    _DbContext.Set<TEntity>().Remove(entity);
                    await _DbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("DeleteAsync in GenericService => " + typeof(TEntity).Name + " : " + id + " => " + ex.ToString());
            }
        }


        public void Update(TEntity entity)
        {
            try
            {
                using (var _DbContext = new EntityDataContext(_connection))
                {
                    _DbContext.Set<TEntity>().Update(entity);
                    _DbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("Update in GenericService :" + JsonConvert.SerializeObject(entity) + " => " + ex.ToString());
            }
        }
        public async Task UpdateAsync(TEntity entity)
        {
            try
            {
                using (var _DbContext = new EntityDataContext(_connection))
                {
                    _DbContext.Set<TEntity>().Update(entity);
                    await _DbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("UpdateAsync in GenericService : " + JsonConvert.SerializeObject(entity) + " => " + ex.ToString());
            }
        }


        public List<TEntity> GetAll()
        {
            try
            {
                var _DbContext = new EntityDataContext(_connection);
                return _DbContext.Set<TEntity>().AsNoTracking().ToList();
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetAll in GenericService" + ex);
                return null;
            }
        }
        public async Task<List<TEntity>> GetAllAsync()
        {
            try
            {
                using (var _DbContext = new EntityDataContext(_connection))
                {
                    return await _DbContext.Set<TEntity>().AsNoTracking().ToListAsync();
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetAllAsync in GenericService" + ex);
                return null;
            }
        }


        public TEntity Find(object id)
        {
            try
            {
                using (var _DbContext = new EntityDataContext(_connection))
                {
                    return _DbContext.Set<TEntity>().Find(id);
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("Find in GenericService" + ex);
                return null;
            }
        }
        public async Task<TEntity> FindAsync(object id)
        {
            try
            {
                using (var _DbContext = new EntityDataContext(_connection))
                {
                    return await _DbContext.Set<TEntity>().FindAsync(id);
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("FindAsync in GenericService" + ex);
                return null;
            }
        }

        public List<TEntity> GetByCondition(Expression<Func<TEntity, bool>> expression)
        {
            try
            {
                using (var _DbContext = new EntityDataContext(_connection))
                {
                    return _DbContext.Set<TEntity>().AsNoTracking().Where(expression).ToList();
                }
            }
            catch
            {
                return null;
            }
        }
        public async Task<List<TEntity>> GetByConditionAsync(Expression<Func<TEntity, bool>> expression)
        {
            try
            {
                using (var _DbContext = new EntityDataContext(_connection))
                {
                    return await _DbContext.Set<TEntity>().AsNoTracking().Where(expression).ToListAsync();
                }
            }
            catch
            {
                return null;
            }
        }
    }
}
