using DemoApp.DTO;
using DemoApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace DemoApp.ServicesImplement
{
    public class PermissionServicesImplement : IPermissionsService
    {
        public Task<IActionResult> AllocatePermissionToRole([FromBody] RolePermissionDto rolePermissionDto)
        {
            throw new NotImplementedException();
        }

        public Task<IActionResult> DeAllocatePermissionToRole([FromBody] RolePermissionDto rolePermissionDto)
        {
            throw new NotImplementedException();
        }

        public Task<IActionResult> DeletePermission([FromQuery] string PermissionId)
        {
            throw new NotImplementedException();
        }

        public Task<IActionResult> GetAllPermission()
        {
            throw new NotImplementedException();
        }

        public Task<IActionResult> GetAllPermissionByRoleId([FromQuery] string roleId)
        {
            throw new NotImplementedException();
        }

        public Task<IActionResult> InsertPermission([FromBody] PermissionDto permissionDto)
        {
            throw new NotImplementedException();
        }
    }
}
