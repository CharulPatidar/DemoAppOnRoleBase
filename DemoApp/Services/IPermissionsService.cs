using DemoApp.DTO;
using Microsoft.AspNetCore.Mvc;

namespace DemoApp.Services
{
    public interface IPermissionsService
    {
        public Task<object> GetAllPermissionAsync();

        public Task<IActionResult> GetAllPermissionByRoleId([FromQuery] string roleId);
        
        public Task<object> InsertPermissionAsync(string permissionName);

        public Task<IActionResult> AllocatePermissionToRole([FromBody] RolePermissionDto rolePermissionDto);

        public Task<object> DeletePermissionAsync(string PermissionId);

        public Task<IActionResult> DeAllocatePermissionToRole([FromBody] RolePermissionDto rolePermissionDto);

        

    }
}
