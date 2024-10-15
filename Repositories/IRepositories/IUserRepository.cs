using Entities.Models;
using Entities.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repositories.IRepositories
{
    public interface IUserRepository
    {
        Task<UserDetailViewModel> CheckExistAccount(AccountModel entity);
        Task<bool> ResetPassword(string input);
        GenericViewModel<UserGridModel> GetPagingList(string userName, string strRoleId, int status, int currentPage, int pageSize);
        Task<UserDetailViewModel> GetDetailUser(int Id);
        Task<UserDataViewModel> GetUser(int Id);
        Task<int> Create(UserViewModel model);
        Task<int> Update(UserViewModel model);
        Task<int> UpdateUserRole(int userId, int[] arrayRole, int type);
        Task<int> ChangeUserStatus(int userId);
        Task<User> FindById(int id);
        Task<List<User>> GetUserSuggestionList(string userName);
        Task<User> GetById(long userIds);
        Task<string> ResetPasswordByUserId(int userId);
        Task<int> ChangePassword(UserPasswordModel model);
        List<User> GetAll();
        Task<List<User>> GetUserSuggesstion(string txt_search);
        Task<List<User>> GetUserSuggesstion(string txt_search, List<int> ids);
        Task<User> GetClientDetailAsync(long clientId);
        Task<IEnumerable<RolePermission>> GetUserPermissionById(int Id);
        public List<UserPosition> GetUserPositions();
        public Task<UserPosition> GetUserPositionsByID(int id);
        Task<List<Role>> GetUserActiveRoleList(int user_id);
        string GetListUserByUserId(int user_id);
        Task<bool> CheckRolePermissionByUserAndRole(int UserId, int RoleId, int PermissionId, int MenuId);
        Task<bool> CheckRolePermissionByUserAndRole(int UserId, List<long> RoleIds, int PermissionId, int MenuId);

        Task<int> GetManagerByUserId(long UserId);

        Task<User> GetChiefofDepartmentByServiceType(int service_type);
        Task<List<User>> GetChiefofDepartmentByServiceTypeNew(int service_type);
        Task<int> GetLeaderByUserId(long UserId);
        List<User> GetAdminUser();
        List<User> GetHeadOfAccountantUser();
        bool IsAdmin(long userId);
        bool IsHeadOfAccountant(long userId);
        bool IsAccountant(long userId);
        List<User> GetHeadOfAccountantUser2();
        Task<List<User>> GetByIds(List<long> userIds);
        Task<List<UserDepart>> GetListUserDepartById(List<int?> ids);
        public Task<int> CountUser();

    }
}
