using Entities.Models;
using Entities.ViewModels.GroupProducts;
using ENTITIES.ViewModels.Articles;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repositories.IRepositories
{
    public interface IGroupProductRepository
    {
        Task<List<GroupProduct>> GetAll();
        Task<GroupProduct> GetById(int Id);
        Task<int> UpSert(GroupProduct model);
        Task<int> Delete(int id);
        Task<string> GetListTreeView(string name, int status);
        Task<string> GetListTreeViewCheckBox(int ParentId, int status, List<int> CheckedList = null, bool IsHasIconEdit = false);
        Task<List<GroupProduct>> getCategoryByParentId(int parent_id);
        Task<List<GroupProduct>> getCategoryDetailByCategoryId(int[] category_id);
        Task<List<GroupProduct>> getAllGroupProduct();
        Task<GroupProduct> getDetailByPath(string path);
        Task<string> GetHtmlHorizontalMenu(int ParentId);
        Task<int> GetRootParentId(int cateId);
        Task<string> GetGroupProductNameAsync(int cateID);
        Task<bool> IsGroupHeader(List<int> groups);
        Task<List<ProductGroupViewModel>> GetProductGroupByParentID(long parent_id, string url_static);
    }
}
