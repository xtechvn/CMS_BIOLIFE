using Elasticsearch.Net;
using Entities.Models;
using Entities.ViewModels;
using Entities.ViewModels.AccountAccessAPI;
using Entities.ViewModels.AccountAccessApiPermission;
using Microsoft.AspNetCore.Mvc;
using Nest;
using Repositories.IRepositories;
using System.Collections.Generic;
using Utilities;
using Utilities.Contants;
using WEB.CMS.Customize;
using static Utilities.Contants.Constants;

namespace WEB.CMS.Controllers
{
    [CustomAuthorize]
    public class AccountAccessAPIController : Controller
    {
        private readonly IAccountAccessApiRepository _accountAccessApiRepository;
        private readonly IAllCodeRepository _allCodeRepository;
        private readonly IAccountAccessApiPermissionRepository _accountAccessApiPermissionRepository;
        public AccountAccessAPIController(IAccountAccessApiRepository accountAccessApiRepository, IAllCodeRepository allCodeRepository, IAccountAccessApiPermissionRepository accountAccessApiPermissionRepository)
        {
            _accountAccessApiRepository = accountAccessApiRepository;
            _allCodeRepository = allCodeRepository;
            _accountAccessApiPermissionRepository = accountAccessApiPermissionRepository;
        }

        public async Task<IActionResult> Index()
        {
            List<AllCode> lstAllCode = await _allCodeRepository.GetAllSortByIDAndType(((int)AllCodeTypeEqualsPROJECT_TYPESortById.Default),AllCodeType.PROJECT_TYPE);
            List<AccountAccessApiViewModel> lstAcountAccessAPIVM = await _accountAccessApiRepository.GetAllAccountAccessAPI();
            return View(lstAcountAccessAPIVM);
        }


        [HttpPost]
        [HttpGet]
        public async Task<IActionResult> Update(int id,int id_AllCode, int id_AccountAccessAPIPermission) 
        {
            AccountAccessApiViewModel AccountAccessApiViewModel = await _accountAccessApiRepository.GetAccountAccessApiByID(id);
            List<AllCode> Code = await _allCodeRepository.GetAllSortByIDAndType(id_AllCode, AllCodeType.PROJECT_TYPE);
            AccountAccessApiPermission AAAP = await _accountAccessApiPermissionRepository.GetAccountAccessApiPermissionByID(id_AccountAccessAPIPermission);

            ViewBag.ViewModel = AccountAccessApiViewModel;
            return View(Code);
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            List<AllCode> lstAllCode = await _allCodeRepository.GetAllSortByIDAndType(((int)AllCodeTypeEqualsPROJECT_TYPESortById.Default), AllCodeType.PROJECT_TYPE);
            return View(lstAllCode);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(int id)
        {
            try
            {
                var rs =await _accountAccessApiRepository.ResetPassword(id);
                if (rs > 0)
                {
                    return new JsonResult(new
                    {
                        isSuccess = true,
                        message = "Reset mật khẩu thành công"
                    });
                }
                else
                {
                    return new JsonResult(new
                    {
                        isSuccess = false,
                        message = "Reset mật khẩu thất bại"
                    });
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("Reset - AccountAccessAPIController: " + ex);
                return new JsonResult(new
                {
                    isSuccess = false,
                    message = "Reset mật khẩu thất bại"
                });
            }
        }



        [HttpPost]
        public async Task<IActionResult> InsertAccountAccessAPI(AccountAccessApiSubmitModel model,int codevalue) 
        {
            try 
            {
                var rsAAA = await _accountAccessApiRepository.InsertAccountAccessAPI(model);
                AAAPSubmitModel AAApermission = new AAAPSubmitModel() 
                {
                    ProjectType = codevalue,
                    AccountAccessApiId = rsAAA
                };
                var rsAAAP = await _accountAccessApiPermissionRepository.InsertAccountAccessApiPermission(AAApermission);
                if (rsAAA > 0 && rsAAAP > 0)
                {
                    return new JsonResult(new
                    {
                        isSuccess = true,
                        message = "Thêm mới thành công"
                    });
                }
                else 
                {
                    return new JsonResult(new
                    {
                        isSuccess = false,
                        message = "Thêm mới thất bại"
                    });
                }
            } 
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("Insert - AccountAccessAPIController: " + ex);
                return new JsonResult(new
                {
                    isSuccess = false,
                    message = "Thêm mới thất bại"
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateAccountAccessAPI(AccountAccessApiSubmitModel model, int codevalue, int id_accountAccessapipermission)
        {
            try
            {
                var rsAAA = await _accountAccessApiRepository.UpdateAccountAccessAPI(model);

                AAAPSubmitModel AAApermission = new AAAPSubmitModel()
                {
                    Id = id_accountAccessapipermission,
                    ProjectType = codevalue,
                    AccountAccessApiId = rsAAA
                };
                var rsAAAP = await _accountAccessApiPermissionRepository.UpdateAccountAccessApiPermission(AAApermission);
                if (rsAAA > 0 && rsAAAP > 0)
                {
                    return new JsonResult(new
                    {
                        isSuccess = true,
                        message = "Cập nhật thành công"
                    });
                }
                else
                {
                    return new JsonResult(new
                    {
                        isSuccess = false,
                        message = "Cập nhật thất bại"
                    });
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("Insert - AccountAccessAPIController: " + ex);
                return new JsonResult(new
                {
                    isSuccess = false,
                    message = "Cập nhật thất bại"
                });
            }
        }
    }
}
