using DemoApp.Controllers;
using DemoApp.DTO;
using DemoApp.Hubs;
using DemoApp.Models;
using DemoApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace DemoApp.ServicesImplement
{
    public class RoleServicesImplement : BaseServiceImplement, IRolesService
    {


   

        public RoleServicesImplement(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, IHubContext<NotesHub> notesHub)
             : base(context, httpContextAccessor, notesHub)
        {
            

        }


        public async Task<string> AllocateRoleToUserAsync(UserRoleDto userRoleDto)
        {
            try
            {
                var user = _context.Users.Where(u => u.Id.ToString().Equals(userRoleDto.UserId)).FirstOrDefault();

                var role = _context.Roles.Where(r => r.Id.ToString().Equals(userRoleDto.RoleId)).FirstOrDefault();

                if (user == null || role == null) 
                {
                    return ("Assign Proper Role And User"); 
                }

                var userRole = await _context.UserRoles.Where(ur => ur.UserId.ToString().Equals(userRoleDto.UserId) && ur.RoleId.ToString().Equals(userRoleDto.RoleId)).FirstOrDefaultAsync();
                
                if (userRole != null) 
                {
                    return ($"Already role is assign to {user.UserName}, Remove first to reassign "); 
                }

                var allReadyuserAssign = await _context.UserRoles.Where(ur => ur.UserId.ToString().Equals(userRoleDto.UserId) ).FirstOrDefaultAsync();

                if (allReadyuserAssign != null)
                {
                    return ($"Already role is assign to {user.UserName}, Remove first to reassign ");
                }

                UserRole newUserRole = new UserRole();

                newUserRole.UserId = user.Id;
                newUserRole.RoleId = role.Id;

                newUserRole.Role = role;
                newUserRole.User = user;

                _context.UserRoles.Add(newUserRole);
                _context.SaveChanges();


                return ($" Allocation Of Role {role.RoleName} to User {user.UserName}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                throw;
            }
        }

        public async Task<string> DeAllocateRoleToUserAsync(UserRoleDto userRoleDto)
        {
            try
            {
                var user = _context.Users.Where(p => p.Id.ToString().Equals(userRoleDto.UserId)).FirstOrDefault();

                var role = _context.Roles.Where(r => r.Id.ToString().Equals(userRoleDto.RoleId)).FirstOrDefault();

                if (user == null || role == null) { return ("Assign Proper Role And user"); }


                var userRole = _context.UserRoles.Where(rp => rp.User.Equals(user) && rp.Role.Equals(role)).FirstOrDefault();

                _context.UserRoles.Remove(userRole);
                _context.SaveChanges();


                return ($" DeAllocation Of User {user.UserName} from Role {role.RoleName}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                throw;
            }
        }

        public async Task<object> DeleteRoleAsync(string RoleId)
        {
            try
            {
                if (string.IsNullOrEmpty(RoleId))
                {
                    return ("Role name cannot be empty");
                }

                var Role =  _context.Roles
                            .AsEnumerable()
                            .FirstOrDefault(r => r.Id.ToString().Equals(RoleId, StringComparison.OrdinalIgnoreCase));

                if (Role != null)
                {
                    _context.Roles.Remove(Role);
                    _context.SaveChanges();

                    return (Role);
                }

                return ($"Role Not Found ");
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                Console.WriteLine($"An error occurred: {ex.Message}");
                throw;
            }
        }

        public async Task<object> GetAllRolesAsync()
        {
            try
            {
                var roles = await _context.Roles.ToListAsync();
                return (roles);
            }
            catch (Exception ex)
            {

                Console.Error.WriteLine($"An error occurred while fetching roles: {ex.Message}");
                //return StatusCode(500, "Internal Server Error");
                throw;
            }
        }

        public async Task<object> InsertRoleAsync(string roleName)
        {
            try
            {
                if (string.IsNullOrEmpty(roleName))
                {
                    return ("Role name cannot be empty");
                }

                var role = _context.Roles
                            .AsEnumerable()  //AsEnumerable or ToList to bring the data into memory before applying the case-insensitive comparison
                            .FirstOrDefault(r => r.RoleName.Equals(roleName, StringComparison.OrdinalIgnoreCase));

                if (role != null)
                {
                    return ($"Role With Name '{roleName}' All Ready Present");
                }

                Role newRole = new Role();
                newRole.RoleName = roleName;
                _context.Roles.Add(newRole);
                _context.SaveChanges();

                return (newRole);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                Console.WriteLine($"An error occurred: {ex.Message}");
                throw;
            }
        }
    }
}
