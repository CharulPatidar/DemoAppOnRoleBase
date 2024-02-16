using DemoApp.DTO;
using Microsoft.AspNetCore.Mvc;

namespace DemoApp.Services
{
    public interface IPermissionsService
    {
        public Task<IActionResult> GetAllPermission();

        public Task<IActionResult> GetAllPermissionByRoleId([FromQuery] string roleId);
        
        public Task<IActionResult> InsertPermission([FromBody] PermissionDto permissionDto);

        public Task<IActionResult> AllocatePermissionToRole([FromBody] RolePermissionDto rolePermissionDto);

        public Task<IActionResult> DeletePermission([FromQuery] string PermissionId);

        public Task<IActionResult> DeAllocatePermissionToRole([FromBody] RolePermissionDto rolePermissionDto);

        

    }
}
