using Microsoft.AspNetCore.Mvc;
using DemoApp.DTO;

namespace DemoApp.Services
{
    public interface IUsersService
    {

        public Task<string> LoginAsync(UserDto userDto);

        public Task<string> RegisterAsync(UserDto userDto);

        public  Task<object> GetAllUsersAsync();

        public Task<object> GetUserDataAsync(HttpContext httpContext);

        public Task<object> GetAllUserByRoleIdAsync(string roleId);

        public Task<object> DeleteUserAsync(string userId);
       
    }
}
