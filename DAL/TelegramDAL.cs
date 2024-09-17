using DAL.Generic;
using DAL.StoreProcedure;
using System;
using System.Collections.Generic;
using System.Text;
using Entities.Models;
using System.Linq;
using MongoDB.Bson.IO;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Utilities;
using Entities.ViewModels;

namespace DAL
{
    public class TelegramDAL : GenericService<TelegramDetail>
    {

        public TelegramDAL(string connection) : base(connection)
        {

        }

        public List<TelegramDetail> GetTelegramPagingList(string TokenName, int Projectmodel, int statusmodel, int currentPage, int pageSize, out int totalRecord)
        {
            totalRecord = 0;

            try
            {
                using (var _DbContext = new EntityDataContext(_connection))
                {
                    var datalist = _DbContext.TelegramDetails.AsQueryable();
                    datalist = datalist.OrderByDescending(x => x.CreateDate);
                    if (!string.IsNullOrEmpty(TokenName))
                    {
                        datalist = datalist.Where(s => s.Token.Contains(TokenName));
                    }
                    if (Projectmodel > 0)
                    {
                        datalist = datalist.Where(s => s.ProjectType == Projectmodel);
                    }
                    if (Projectmodel == 0)
                    {
                        datalist = datalist.Where(s => s.Id != 0);
                    }
                    if (statusmodel > 0)
                    {
                        datalist = datalist.Where(s => s.Status == statusmodel);
                    }
                    if (statusmodel == 0)
                    {
                        datalist = datalist.Where(s => s.Status == statusmodel);
                    }
                    totalRecord = datalist.Count();
                    var data = datalist.Select(a => new TelegramDetail
                    {

                        Id = a.Id,
                        Token = a.Token,
                        GroupChatId = a.GroupChatId,
                        GroupLog = a.GroupLog,
                        CreateDate = a.CreateDate,
                        ProjectType = a.ProjectType,
                        Status = a.Status,

                    }).Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
                    return data;
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetRolePagingList - TelegramDAL: " + ex);
            }

            return null;
        }

        public List<TelegramDetail> GetAllTelegram()
        {

            try
            {
                using (var _DbContext = new EntityDataContext(_connection))
                {
                    var a = _DbContext.TelegramDetails.Where(s => s.Id != 0).ToList();
                    return a;
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetAllTelegram -TelegramDAL" + ex);
                return null;
            }
        }

        public TelegramDetail GetTelegrambyid(int id)
        {

            try
            {
                using (var _DbContext = new EntityDataContext(_connection))
                {
                    var data = _DbContext.TelegramDetails.Where(s => s.Id == id).FirstOrDefault();

                    return data;
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetTelegrambyid - TelegramDAL: " + ex);
            }
            return null;
        }
        public async Task<int> AddTelegram(TelegramDetail telegrammodel)
        {

            try
            {
                using (var _DbContext = new EntityDataContext(_connection))
                {
                    var datalist = _DbContext.TelegramDetails.AsQueryable();
                    datalist = datalist.Where(s => s.GroupChatId == telegrammodel.GroupChatId);                   
                    var a = datalist.Count();
                    if (telegrammodel.Id == 0 && a == 0)
                    {
                        var add = _DbContext.TelegramDetails.Add(telegrammodel);
                        await _DbContext.SaveChangesAsync();
                        return 0;
                    }
                    var data = GetTelegrambyid(telegrammodel.Id);
                    if ((telegrammodel.Id!=0 && a == 0)||(a==1 &&data.GroupChatId==telegrammodel.GroupChatId) )
                    {
                        var add = _DbContext.TelegramDetails.Update(telegrammodel);
                        await _DbContext.SaveChangesAsync();
                        return 0;
                    }
                    
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("AddTelegram - TelegramDAL: " + ex);
            }
            return -1;
        }

    }
}
