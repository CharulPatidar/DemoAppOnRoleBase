using DemoApp.DTO;
using Microsoft.AspNetCore.Mvc;

namespace DemoApp.Services
{
    public interface IPermissionsService
    {
        public Task<object> GetAllPermissionAsync();

        public Task<object> GetAllPermissionByRoleId(string roleId);
        
        public Task<object> InsertPermissionAsync(string permissionName);

        public Task<string> AllocatePermissionToRoleAsync(RolePermissionDto rolePermissionDto);

        public Task<object> DeletePermissionAsync(string PermissionId);

        public Task<string> DeAllocatePermissionToRole([FromBody] RolePermissionDto rolePermissionDto);

        

    }
}
