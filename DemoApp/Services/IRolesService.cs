using DemoApp.DTO;
using Microsoft.AspNetCore.Mvc;

namespace DemoApp.Services
{
    public interface IRolesService
    {
        public  Task<object> GetAllRolesAsync();

        public  Task<object> InsertRoleAsync(string roleName);

        public Task<object> DeleteRoleAsync(string RoleId);

        public Task<string> AllocateRoleToUser(UserRoleDto userRoleDto);

        public Task<IActionResult> DeAllocateRoleToUser(UserRoleDto userRoleDto);
    }
}
