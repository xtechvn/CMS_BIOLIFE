using DAL.Generic;
using Entities.Models;
using Entities.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;
using Utilities.Contants;

namespace DAL
{
    public class NoteDAL : GenericService<Note>
    {
        public NoteDAL(string connection) : base(connection)
        {
        }

        public async Task<List<NoteViewModel>> GetListByType(long DataId, int Type)
        {
            try
            {
                using (var _DbContext = new EntityDataContext(_connection))
                {
                    return await (from n in _DbContext.Notes.AsNoTracking()
                                  join u in _DbContext.Users.AsNoTracking() on n.UserId equals u.Id
                                  where n.DataId == DataId && n.Type == Type
                                  select new NoteViewModel
                                  {
                                      Id = n.Id,
                                      DataId = n.DataId,
                                      Comment = n.Comment,
                                      CreateDate = n.CreateDate,
                                      UserId = n.UserId,
                                      UserName = u.UserName,
                                      UpdateTime = n.UpdateTime,
                                      Type = n.Type
                                  }).ToListAsync();
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetListByType - NoteDAL: " + ex);
                return null;
            }
        }

        public async Task<bool> MultipleInsertAsync(List<NoteModel> notes, long OrderId)
        {
            try
            {
                using (var _DbContext = new EntityDataContext(_connection))
                {
                    using (var transaction = _DbContext.Database.BeginTransaction())
                    {
                        try
                        {
                            foreach (var item in notes)
                            {
                                var noteModel = await _DbContext.Notes.FirstOrDefaultAsync(s => s.NoteMapId == item.NoteMapId && s.Type == item.Type);
                                if (noteModel != null)
                                {
                                    noteModel.Comment = item.Comment;
                                    noteModel.UpdateTime = item.UpdateTime;
                                    _DbContext.Notes.Update(noteModel);
                                    await _DbContext.SaveChangesAsync();
                                }
                                else
                                {
                                    var noteItem = new Note
                                    {
                                        DataId = 0,
                                        Type = item.Type,
                                        Comment = item.Comment,
                                        UserId = item.UserId,
                                        NoteMapId = item.NoteMapId,
                                        CreateDate = item.CreateDate,
                                        UpdateTime = item.UpdateTime
                                    };

                                    if (item.Type == (int)Constants.NoteType.ORDER)
                                    {
                                        noteItem.DataId = OrderId;
                                    }
                                    /*
                                    if (item.Type == (int)Constants.NoteType.ORDER_ITEM)
                                    {
                                        var OrderItemModel = await _DbContext.OrderItem.FirstOrDefaultAsync(s => s.OrderItempMapId == item.OrderItemMapId);
                                        if (OrderItemModel != null)
                                        {
                                            noteItem.DataId = OrderItemModel.Id;
                                        }
                                    }*/

                                    if (noteItem.DataId != 0)
                                    {
                                        await _DbContext.Notes.AddAsync(noteItem);
                                        await _DbContext.SaveChangesAsync();
                                    }
                                }
                            }
                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            LogHelper.InsertLogTelegram("MultipleInsertAsync - transaction.Rollback - NoteDAL: " + ex);
                            return false;
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("MultipleInsertAsync - NoteDAL: " + ex);
                return false;
            }
        }
    }
}
