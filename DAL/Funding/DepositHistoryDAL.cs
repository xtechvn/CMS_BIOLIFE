using DAL.Generic;
using DAL.StoreProcedure;
using Entities.Models;
using Entities.ViewModels.Funding;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Utilities;
using Utilities.Contants;

namespace DAL.Funding
{
    public class DepositHistoryDAL : GenericService<DepositHistory>
    {
        private static DbWorker _DbWorker;
        public DepositHistoryDAL(string connection) : base(connection)
        {
            _connection = connection;
            _DbWorker = new DbWorker(connection);
        }

        public List<DepositHistory> GetDepositHistories(FundingSearch searchModel, out long total, out List<CountStatus> countStatus, int currentPage = 1, int pageSize = 20)
        {
            var listDepositHistory = new List<DepositHistory>();

            try
            {
                using (var _DbContext = new EntityDataContext(_connection))
                {
                    var query = _DbContext.DepositHistories.AsNoTracking().AsQueryable();

                    if (!string.IsNullOrEmpty(searchModel.TransNo))
                    {
                        query = query.Where(n => n.TransNo.Contains(searchModel.TransNo.Trim()));
                    }

                    if (searchModel.Status > -1)
                    {
                        query = query.Where(n => n.Status == searchModel.Status);
                    }

                    if (searchModel.StatusChoose > -1)
                    {
                        query = query.Where(n => n.Status == searchModel.StatusChoose);
                    }

                    //Compare params list 
                    if (searchModel.StatusList != null && searchModel.StatusList.Count > 0 && !searchModel.StatusList.Contains(-1))
                    {
                        query = query.Where(n => n.Status != null && searchModel.StatusList.Contains(n.Status.Value));
                    }

                    if (searchModel.PaymentTypes != null && searchModel.PaymentTypes.Count > 0 && !searchModel.PaymentTypes.Contains(-1))
                    {
                        query = query.Where(n => n.PaymentType != null && searchModel.PaymentTypes.Contains(n.PaymentType.Value));
                    }

                    if (searchModel.ServiceTypes != null && searchModel.ServiceTypes.Count > 0 && !searchModel.ServiceTypes.Contains(-1))
                    {
                        query = query.Where(n => n.ServiceType != null && searchModel.ServiceTypes.Contains(n.ServiceType.Value));
                    }

                    if (searchModel.TransTypes != null && searchModel.TransTypes.Count > 0 && !searchModel.TransTypes.Contains(-1))
                    {
                        query = query.Where(n => n.TransType != null && searchModel.TransTypes.Contains(n.TransType.Value));
                    }

                    if (searchModel.ApproverIds != null && searchModel.ApproverIds.Count > 0)
                    {
                        query = query.Where(n => n.UserVerifyId != null && searchModel.ApproverIds.Contains(n.UserVerifyId.Value));
                    }

                    if (searchModel.CreateByIds != null && searchModel.CreateByIds.Count > 0)
                    {
                        query = query.Where(n => n.UserId != null && searchModel.CreateByIds.Contains(n.UserId.Value));
                    }

                    if (searchModel.FromCreateDate != null)
                    {
                        query = query.Where(n => n.CreateDate != null && n.CreateDate >= searchModel.FromCreateDate);
                    }

                    if (searchModel.ToCreateDate != null)
                    {
                        query = query.Where(n => n.CreateDate != null && n.CreateDate < searchModel.ToCreateDate.Value.AddDays(1));
                    }

                    if (searchModel.FromApproveDate != null)
                    {
                        query = query.Where(n => n.VerifyDate != null && n.VerifyDate >= searchModel.FromApproveDate);
                    }

                    if (searchModel.ToApproveDate != null)
                    {
                        query = query.Where(n => n.VerifyDate != null && n.VerifyDate < searchModel.ToApproveDate.Value.AddDays(1));
                    }


                    if (!string.IsNullOrEmpty(searchModel.OrderBy))
                    {
                        if (searchModel.OrderBy.ToLower().Equals("TransNo".ToLower()))
                        {
                            query = query.OrderByDescending(n => n.TransNo);
                        }
                        if (searchModel.OrderBy.ToLower().Equals("TransNo".ToLower()))
                        {
                            query = query.OrderByDescending(n => n.UpdateLast);
                        }
                    }
                    else
                    {
                        query = query.OrderByDescending(n => n.UpdateLast);
                    }


                    var result = query.ToList();
                    total = result.Count;
                    if (pageSize == -1)
                    {
                        listDepositHistory = result;
                        countStatus = new List<CountStatus>();
                    }
                    else
                    {
                        listDepositHistory = result.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
                        countStatus = result.GroupBy(n => n.Status).Select(n => new CountStatus
                        {
                            Status = n.Key.Value,
                            Count = n.Count()
                        }).ToList();
                        countStatus.Add(new CountStatus()
                        {
                            Status = -1,
                            Count = (int)total
                        });
                    }

                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetDepositHistories - DepositHistoryDAL: " + ex);
                total = 0;
                countStatus = new List<CountStatus>();
            }
            return listDepositHistory;
        }

        public DepositHistory GetById(int depositHistoryId)
        {
            try
            {
                using (var _DbContext = new EntityDataContext(_connection))
                {
                    var detail = _DbContext.DepositHistories.AsNoTracking().FirstOrDefault(x => x.Id == depositHistoryId);
                    if (detail != null)
                    {
                        return detail;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetById - DepositHisotyDAL: " + ex);
                return null;
            }
        }

        public DepositHistory GetByNo(string depositHistoryNo)
        {
            try
            {
                using (var _DbContext = new EntityDataContext(_connection))
                {
                    var detail = _DbContext.DepositHistories.AsNoTracking().FirstOrDefault(x => x.TransNo == depositHistoryNo);
                    if (detail != null)
                    {
                        return detail;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetByNo - DepositHisotyDAL: " + ex);
                return null;
            }
        }

        public List<DepositHistory> GetByIds(List<int> depositHistoryIds)
        {
            try
            {
                using (var _DbContext = new EntityDataContext(_connection))
                {
                    var details = _DbContext.DepositHistories.AsNoTracking().Where(x => depositHistoryIds.Contains(x.Id)).ToList();
                    if (details != null)
                    {
                        return details;
                    }
                }
                return new List<DepositHistory>();
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetByIds - DepositHisotyDAL: " + ex);
                return new List<DepositHistory>();
            }
        }

        public List<DepositHistoryViewMdel> getDepositHistoryByUserId(long userId, int currentPage, int pageSize, out int totalRecord)
        {
            totalRecord = 0;
            try
            {
                using (var _DbContext = new EntityDataContext(_connection))
                {
                    var data_DepositHistory = _DbContext.DepositHistories.AsNoTracking().Where(s => s.UserId == userId);
                    var data = (from a in _DbContext.DepositHistories.AsNoTracking().Where(s => s.UserId == userId)
                                join b in _DbContext.AllCodes.AsNoTracking().Where(s => s.Type == AllCodeType.PAYMENT_TYPE) on a.PaymentType equals b.CodeValue
                                join c in _DbContext.AllCodes.AsNoTracking().Where(s => s.Type == AllCodeType.DEPOSIT_STATUS) on a.Status equals c.CodeValue
                                select new DepositHistoryViewMdel
                                {
                                    Id = a.Id,
                                    CreateDate = a.CreateDate,
                                    UpdateLast = a.UpdateLast,
                                    UserId = a.UserId,
                                    TransNo = a.TransNo,
                                    Title = a.Title,
                                    Price = a.Price,
                                    TransType = a.TransType,
                                    PaymentType = a.PaymentType,
                                    Status = a.Status,
                                    ImageScreen = a.ImageScreen,
                                    paymentName = b.Description,
                                    statusName = c.Description,

                                }).OrderByDescending(s => s.CreateDate).Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
                    totalRecord = data_DepositHistory.Count();
                    return data;
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("getDepositHistoryByUserId - DepositHistoryDAL: " + ex.ToString());
                return null;
            }

        }

        public DataTable GetListOrderByClientId(long clienId, string proc)
        {
            try
            {
                SqlParameter[] objParam = new SqlParameter[2];
                objParam[0] = new SqlParameter("@ClientId", clienId);
                objParam[1] = new SqlParameter("@IsFinishPayment", DBNull.Value);
                return _DbWorker.GetDataTable(proc, objParam);
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetListOrderByClientId - DepositHistoryDAL: " + ex);
            }
            return null;
        }
    }
}
