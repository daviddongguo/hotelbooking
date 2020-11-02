using david.hotelbooking.domain.Entities.RBAC;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace david.hotelbooking.domain.Services
{
    public interface IUserService
    {
        Task<RBAC_User> AddOrUpdateUser(RBAC_User inputUser);
        Task<bool> DeleteUser(int? id);
        Task<IQueryable<Role>> GetAllRoles();
        Task<IQueryable<RBAC_User>> GetAllUsers();
        Task<IQueryable<Permission>> GetAllPermissions();
        Task<Role> GetSingleRole(int? id);
        Task<Role> GetSingleRole(string roleName);
        Task<RBAC_User> GetSingleUser(int? id);
        Task<RBAC_User> GetSingleUser(string email);
        Task<bool> IsEmailExisted(string email);
        Task<Role> UpdateRole(Role toUpdateRole);
        Task<List<RolePermission>> UpdateRolePermissions(int toUpdateRoleId, List<int> toUpdatePermissionIds);
        Task<List<UserRole>> UpdateUserRoles(int toUpdateUserId, List<int> toAddOrUpdateRoleIds);
        Task<Permission> GetSinglePermission(int permissionId);
        Task<RolePermission> AddRolePermission(RolePermission rolePermission);
        Task<IQueryable<RolePermission>> GetAllRolePermissions();
    }
}
