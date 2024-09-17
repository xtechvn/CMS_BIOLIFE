using DAL;
using Entities.ConfigModels;
using Entities.Models;
using Entities.ViewModels.GroupProducts;
using ENTITIES.ViewModels.Articles;
using Microsoft.Extensions.Options;
using Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;
using Utilities.Contants;

namespace Repositories.Repositories
{
    public class GroupProductRepository : IGroupProductRepository
    {
        private const int NEWS_CATEGORY_ID = 10;
        private readonly GroupProductDAL _GroupProductDAL;
        private readonly AllCodeDAL _AllCodeDAL;

        public GroupProductRepository(IOptions<DataBaseConfig> dataBaseConfig)
        {
            _GroupProductDAL = new GroupProductDAL(dataBaseConfig.Value.SqlServer.ConnectionString);
            _AllCodeDAL = new AllCodeDAL(dataBaseConfig.Value.SqlServer.ConnectionString);
        }

        public async Task<List<GroupProduct>> GetAll()
        {
            return await _GroupProductDAL.GetAllAsync();
        }

        public async Task<GroupProduct> GetById(int Id)
        {
            return await _GroupProductDAL.FindAsync(Id);
        }

        public async Task<int> UpSert(GroupProduct model)
        {
            try
            {
                var listdata = _GroupProductDAL.GetAll().Where(s => s.ParentId == model.ParentId && s.Id != model.Id).ToList();
                if (listdata.Where(n => n.Name.ToLower().Equals(model.Name.ToLower())).FirstOrDefault() != null)
                {
                    return -1;
                }

                model.Path = StringHelpers.ReplaceUrlPathSpecialCharacter(model.Name.Trim().ToLower());

                if (model.Id != 0)
                {
                    var entity = await _GroupProductDAL.FindAsync(model.Id);
                    entity.Name = model.Name;
                    entity.OrderNo = model.OrderNo;
                    entity.ImagePath = model.ImagePath;
                    entity.Status = model.Status;
                    entity.Path = model.Path ;
                    entity.Description = model.Description;
                    entity.PositionId = model.PositionId;
                    entity.ModifiedOn = DateTime.Now;
                    entity.IsShowHeader = model.IsShowHeader;
                    entity.IsShowFooter = model.IsShowFooter;
                    entity.Code = model.Code;
                    await _GroupProductDAL.UpdateAsync(entity);

                    // Update children status
                    if (model.Status == 1)
                    {
                        var groupList = await _GroupProductDAL.GetAllAsync();
                        var childList = new List<GroupProduct>();
                        FindChildList(model.Id, groupList, ref childList);
                        if (childList.Count > 0)
                        {
                            foreach (var item in childList)
                            {
                                if (item.Status != model.Status)
                                {
                                    var entitychild = await _GroupProductDAL.FindAsync(item.Id);
                                    entitychild.Status = model.Status;
                                    await _GroupProductDAL.UpdateAsync(entitychild);
                                }
                            }
                        }
                    }

                    return model.Id;
                }
                else
                {
                    model.CreatedOn = DateTime.Now;

                    var id = (int)await _GroupProductDAL.CreateAsync(model);
                    model.Path = model.Path + "-" + model.Id;
                    await _GroupProductDAL.UpdateAsync(model);

                    return id;
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("UpSert - GroupProductRepository" + ex);
                return 0;
            }
        }

      

        public async Task<int> Delete(int id)
        {
            return await _GroupProductDAL.DeleteGroupProduct(id);
        }

        public async Task<string> GetListTreeView(string name, int status)
        {
            var _strHtml = new StringBuilder();
            try
            {
                var _parentList = new List<GroupProduct>();
                var _groupList = await _GroupProductDAL.GetAllAsync();
                var _listSearch = _groupList;

                if (!string.IsNullOrEmpty(name))
                {
                    _listSearch = _groupList.Where(s => StringHelpers.ConvertStringToNoSymbol(s.Name.ToLower()).Contains(StringHelpers.ConvertStringToNoSymbol(name.ToLower()))).ToList();
                }

                if (status != -1)
                {
                    _listSearch = _listSearch.Where(s => s.Status == status).ToList();
                }

                foreach (var item in _listSearch)
                {
                    _parentList.Add(GetParentModel(item, _groupList));
                }

                _parentList = _parentList.Where(x=>x!=null).Distinct().OrderBy(s => s.OrderNo).ToList();

                foreach (var item in _parentList)
                {
                    _strHtml.Append(RenderHtmlTreeView(item.Id, _groupList));
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetListTreeView - GroupProductRepository: " + ex);
            }
            return _strHtml.ToString();
        }

        public void FindChildList(int Id, List<GroupProduct> ListData, ref List<GroupProduct> rsList)
        {
            try
            {
                var childlist = ListData.Where(s => s.ParentId == Id).ToList();
                if (childlist != null && childlist.Count > 0)
                {
                    rsList.AddRange(childlist);
                    foreach (var item in childlist)
                    {
                        FindChildList(item.Id, ListData, ref rsList);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("FindChildList - GroupProductRepository: " + ex);

            }
        }

        public string RenderHtmlTreeView(int parentId, List<GroupProduct> listData)
        {
            var _strHtml = new StringBuilder();
            var _parentModel = listData.Where(s => s.Id == parentId).FirstOrDefault();
            var _childList = listData.Where(s => s.ParentId == parentId).OrderBy(s => s.OrderNo);

            _strHtml.Append(@"<li class=" + "item-category-" + _parentModel.Id + " >");

            if (_childList != null && _childList.Count() > 0)
            {
                _strHtml.Append(@"<button class=""colspan btn-expand-child"" data-action=""collapse"" type=""button""><i class=""fa fa-plus""></i></button>");
            }

            _strHtml.Append(@"<div class=""dd-handle active"">");
            _strHtml.Append(@"<a class=""item-category-name"" data-id=" + _parentModel.Id + ">" + _parentModel.Name + "</a>");
            _strHtml.Append(@"<div class=""control"">");
            _strHtml.Append(@"<a class=""btn-add-group-product"" data-id=" + _parentModel.Id + "><img src=/images/icons/sql.png /></a>");
            _strHtml.Append(@"<a class=""btn-edit-group-product"" data-id=" + _parentModel.Id + "><img src=/images/icons/edit.png /></a>");
            _strHtml.Append("<a class=\"btn-clearcache-group-product\"data-name=\""+ _parentModel.Name + "\" data-id=\"" + _parentModel.Id + "\"><img src=/images/icons/edit.png /></a>");
            _strHtml.Append(@"</div>");

            if (_parentModel.Status == 1)
                _strHtml.Append(@"<span class=""text-inactive"">(Ngừng hoạt động)</span>");

            _strHtml.Append(@"</div>");

            if (_childList != null && _childList.Count() > 0)
            {
                _strHtml.Append(@"<ul class=""expand lever2"">");

                foreach (var item in _childList)
                {
                    _strHtml.Append(RenderHtmlTreeView(item.Id, listData));
                }

                _strHtml.Append(@"</ul>");
            }

            _strHtml.Append(@"</li>");

            return _strHtml.ToString();
        }

        public static GroupProduct GetParentModel(GroupProduct item, List<GroupProduct> groupList)
        {
            try
            {
                if (item!=null && item.ParentId != -1)
                {
                    var _model = groupList.Where(s => s.Id == item.ParentId).FirstOrDefault();
                    return GetParentModel(_model, groupList);
                }
                else
                {
                    return item;
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetParentModel - GroupProductRepository: " + ex);
                return null;
            }

        }

        public async Task<string> GetListTreeViewCheckBox(int ParentId, int status = -1, List<int> CheckedList = null, bool IsHasIconEdit = false)
        {
            var _strHtml = new StringBuilder();
            try
            {
                var _parentList = new List<GroupProduct>();
                var _groupList = await _GroupProductDAL.GetAllAsync();
                _groupList = _groupList.Where(s => s.Status == (int)StatusType.BINH_THUONG).ToList();
                var _relationCheckedList = new List<int>();
                if (CheckedList != null && CheckedList.Count > 0)
                    CheckedList.ForEach(s => GetFullParentIdFromListChecked(s, _groupList, ref _relationCheckedList));

                if (_relationCheckedList != null && _relationCheckedList.Count > 0)
                    _relationCheckedList.AddRange(CheckedList);

                _strHtml.Append(RenderHtmlTreeViewCheckBox(ParentId, _groupList, CheckedList, _relationCheckedList, IsHasIconEdit));
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetListTreeViewCheckBox - GroupProductRepository: " + ex);
            }
            return _strHtml.ToString();
        }

        public static string RenderHtmlTreeViewCheckBox(int parentId, List<GroupProduct> listData, List<int> CheckedList = null, List<int> ParentCheckedList = null, bool IsHasIconEdit = false)
        {
            try
            {
                var _strHtml = new StringBuilder();
                var _parentModel = listData.Where(s => s.Id == parentId).FirstOrDefault();
                var _childList = listData.Where(s => s.ParentId == parentId).OrderBy(s => s.OrderNo);

                #region Rendering html dom
                _strHtml.Append(@"<li>");

                if (IsHasIconEdit) _strHtml.Append(@"<div class=""onclick"">");

                if (_childList != null && _childList.Count() > 0)
                {
                    if (ParentCheckedList != null && ParentCheckedList.Count > 0 && ParentCheckedList.Contains(parentId))
                        _strHtml.Append(@"<a class=""cur-pointer btn-toggle-cate minus icofont-minus ""></a>");
                    else
                        _strHtml.Append(@"<a class=""cur-pointer btn-toggle-cate plus icofont-plus""></a>");
                }

                if (parentId != NEWS_CATEGORY_ID /*true*/)
                {
                    _strHtml.Append(@"<label class=""check-list mb10 mr25 ml10"">");
                    var strChecked = string.Empty;
                    if (CheckedList != null && CheckedList.Count > 0 && CheckedList.Contains(parentId)) strChecked = "checked";
                    if (_parentModel != null)
                    {
                        _strHtml.Append(@"<input type=""checkbox"" class=""ckb-news-cate"" value='" + _parentModel.Id + "' " + strChecked + "/>");
                        _strHtml.Append(@"<span class=""checkmark""></span>" + _parentModel.Name);
                    }
                   
                    _strHtml.Append(@"</label>");
                }
                else
                {
                    if (_parentModel != null)
                    {
                        _strHtml.Append(@"<label class=""mb10 mr25 ml10"">" + _parentModel.Name + "</label>");

                    }
                }

                if (IsHasIconEdit && CheckedList != null && CheckedList.Count > 0 && CheckedList.Contains(parentId))
                {
                    _strHtml.Append(@"<a class=""edit btn btn-extension-cate"" data-id=" + parentId + ">" + @"<i class=""fa fa-pencil-square-o""></i></a>");
                }

                if (IsHasIconEdit) _strHtml.Append(@"</div>");

                if (_childList != null && _childList.Count() > 0)
                {
                    if (ParentCheckedList != null && ParentCheckedList.Count > 0 && ParentCheckedList.Contains(parentId))
                        _strHtml.Append(@"<ul class=""lever2 mb0"" style=""display: block;"">");
                    else
                        _strHtml.Append(@"<ul class=""lever2 mb0"">");

                    foreach (var item in _childList)
                    {
                        _strHtml.Append(RenderHtmlTreeViewCheckBox(item.Id, listData, CheckedList, ParentCheckedList, IsHasIconEdit));
                    }

                    _strHtml.Append(@"</ul>");
                }

                _strHtml.Append(@"</li>");
                #endregion
                return _strHtml.ToString();
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("RenderHtmlTreeViewCheckBox - GroupProductRepository: " + ex);
                return string.Empty;
            }
        }

        /// <summary>
        /// Cuonglv
        /// Lấy ra chuyên mục theo id của thư mục cha
        /// </summary>
        /// <param name="parent_id"></param>
        /// <returns></returns>
        public async Task<List<GroupProduct>> getCategoryByParentId(int parent_id)
        {
            try
            {
                List<GroupProduct> cate_list = await _GroupProductDAL.GetAllAsync();
                var _parentModel = cate_list.Where(s => s.ParentId == parent_id).OrderBy(x => x.OrderNo).ToList();
                return _parentModel;
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("[CMS] getCategoryByParentId - GroupProductRepository: " + ex);
                return null;
            }
        }
   

        /// <summary>
        /// Cuonglv
        ///API TRẢ CHO FRONTEND
        /// Lấy ra chi tiết ngành hàng theo id
        /// </summary>
        /// <param name="parent_id"></param>
        /// <returns></returns>
        public async Task<List<GroupProduct>> getCategoryDetailByCategoryId(int[] category_id)
        {
            try
            {
                var cate_list = await _GroupProductDAL.getCategoryDetailByCategoryId(category_id);

                return cate_list;
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("[CMS-Repository] getCategoryDetailByCategoryId - GroupProductRepository: " + ex);
                return null;
            }
        }

      
        public void GetFullParentIdFromListChecked(int checkListId, List<GroupProduct> GroupList, ref List<int> Result)
        {
            var dataModel = GroupList.Where(s => s.Id == checkListId).FirstOrDefault();
            if (dataModel != null && dataModel.ParentId > 0)
            {
                Result.Add(dataModel.ParentId);
                GetFullParentIdFromListChecked(dataModel.ParentId, GroupList, ref Result);
            }
        }

      

        public async Task<List<GroupProduct>> getAllGroupProduct()
        {
            return await _GroupProductDAL.getAllGroupProduct();
        }

        public async Task<GroupProduct> getDetailByPath(string path)
        {
            return await _GroupProductDAL.getDetailByPath(path);
        }

        public async Task<string> GetHtmlHorizontalMenu(int ParentId)
        {
            var _strHtml = new StringBuilder();
            try
            {
                var _parentList = new List<GroupProduct>();
                var _groupList = await _GroupProductDAL.GetAllAsync();

                _strHtml.Append(@"<ul class=""nav"">");
                _strHtml.Append(RenderHtmlHorizontalMenu(ParentId, _groupList));
                _strHtml.Append(@"</ul>");

            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetHtmlHorizontalMenu - GroupProductRepository: " + ex);
            }
            return _strHtml.ToString();
        }

        public string RenderHtmlHorizontalMenu(int parentId, List<GroupProduct> listData)
        {
            var _strHtml = new StringBuilder();
            var _parentModel = listData.Where(s => s.Id == parentId).FirstOrDefault();
            var _childList = listData.Where(s => s.ParentId == parentId).OrderBy(s => s.OrderNo);

            if (parentId != NEWS_CATEGORY_ID)
            {
                _strHtml.Append(@"<li>");
                _strHtml.Append(@"<a href=/" + _parentModel.Path + ">" + _parentModel.Name + "</a>");
            }

            if (_childList != null && _childList.Count() > 0)
            {
                if (parentId != NEWS_CATEGORY_ID)
                {
                    _strHtml.Append(@"<span class=""down""></span>");
                    _strHtml.Append(@"<ul class=""level2"">");
                }

                foreach (var item in _childList)
                {
                    _strHtml.Append(RenderHtmlHorizontalMenu(item.Id, listData));
                }

                if (parentId != NEWS_CATEGORY_ID) _strHtml.Append(@"</ul>");
            }

            if (parentId != NEWS_CATEGORY_ID) _strHtml.Append(@"</li>");

            return _strHtml.ToString();
        }

        public int GetRootParentId(int CateId, List<GroupProduct> GroupList)
        {
            var dataModel = GroupList.Where(s => s.Id == CateId).FirstOrDefault();
            if (dataModel.ParentId > 0)
            {
                return GetRootParentId(dataModel.ParentId, GroupList);
            }
            else
            {
                return dataModel.Id;
            }
        }

        public async Task<int> GetRootParentId(int cateId)
        {
            var _groupList = await _GroupProductDAL.GetAllAsync();
            return GetRootParentId(cateId, _groupList);
        }

        public async Task<string> GetGroupProductNameAsync(int cateID)
        {
            string group_name = null;
            try
            {
                var _groupList = await _GroupProductDAL.GetAllAsync();
                var dataModel = _groupList.Where(s => s.Id == cateID).FirstOrDefault();
                group_name = dataModel.Name;
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetGroupProductNameAsync - GroupProductRepository: : " + ex);
            }
            return group_name;
        }
        public async Task<bool> IsGroupHeader(List<int> groups)
        {
            return await _GroupProductDAL.IsGroupHeader(groups);
        }
        public async Task<List<ProductGroupViewModel>> GetProductGroupByParentID(long parent_id, string url_static)
        {
            try
            {
                var group = _GroupProductDAL.GetByParentId(parent_id);
                var list = new List<ProductGroupViewModel>();
                list.AddRange(group.Select(x => new ProductGroupViewModel() { id = x.Id, image = url_static + x.ImagePath, name = x.Name, code = Convert.ToInt32(x.Code), link = CommonHelper.RemoveSpecialCharacters(CommonHelper.RemoveUnicode(x.Name.ToLower())).Replace(" ", "-").Replace("--", "-") }).ToList());
                return list;
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetProductGroupByParentID - GroupProductRepository: " + ex);
            }
            return null;
        }


    }
}
