using DemoApp.DTO;
using Microsoft.AspNetCore.Mvc;

namespace DemoApp.Services
{
    public interface IRolesService
    {
        public  Task<object> GetAllRolesAsync();

        public  Task<object> InsertRoleAsync(string roleName);

        public Task<object> DeleteRoleAsync(string RoleId);

        public Task<string> AllocateRoleToUserAsync(UserRoleDto userRoleDto);

        public Task<string> DeAllocateRoleToUserAsync(UserRoleDto userRoleDto);
    }
}
