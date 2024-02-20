using DemoApp.DTO;
using DemoApp.Hubs;
using DemoApp.Models;
using DemoApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace DemoApp.ServicesImplement
{
    public class PermissionServicesImplement : IPermissionsService
    {



        protected readonly ApplicationDbContext _context;
        protected readonly IHttpContextAccessor _httpContextAccessor;
        protected readonly IHubContext<NotesHub> _notesHub;


        public PermissionServicesImplement(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, IHubContext<NotesHub> notesHub)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _notesHub = notesHub;


        }



        public Task<IActionResult> AllocatePermissionToRole([FromBody] RolePermissionDto rolePermissionDto)
        {
            throw new NotImplementedException();
        }

        public Task<IActionResult> DeAllocatePermissionToRole([FromBody] RolePermissionDto rolePermissionDto)
        {
            throw new NotImplementedException();
        }

        public async Task<object> DeletePermissionAsync(string PermissionId)
        {
            try
            {
                if (string.IsNullOrEmpty(PermissionId))
                {
                    return ("Permisssion name cannot be empty");
                }

                var permission =  _context.Permissions
                            .AsEnumerable()  //AsEnumerable or ToList to bring the data into memory before applying the case-insensitive comparison
                            .FirstOrDefault(p => p.Id.ToString().Equals(PermissionId, StringComparison.OrdinalIgnoreCase));

                if (permission != null)
                {
                    _context.Permissions.Remove(permission);
                    _context.SaveChanges();

                    return (permission);
                }

                return ($"Permission Not Found ");
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                Console.WriteLine($"An error occurred: {ex.Message}");
                throw;
            }
        }

        public async Task<object> GetAllPermissionAsync()
        {

            try
            {
                var permissions = _context.Permissions.ToList();

                if (permissions == null ) 
                {
                    return null;
                }

                return (permissions);
            }
            catch (Exception ex)
            {

                Console.Error.WriteLine($"An error occurred while fetching permissions: {ex.Message}");
                throw;
            }
        }

        public Task<IActionResult> GetAllPermissionByRoleId([FromQuery] string roleId)
        {
            throw new NotImplementedException();
        }

        public async Task<object> InsertPermissionAsync(string permissionName)
        {
            try
            {
                if (string.IsNullOrEmpty(permissionName))
                {
                    return ("Permisssion name cannot be empty");
                }

                var permission = _context.Permissions
                            .AsEnumerable()  //AsEnumerable or ToList to bring the data into memory before applying the case-insensitive comparison
                            .FirstOrDefault(p => p.PermissionName.Equals(permissionName, StringComparison.OrdinalIgnoreCase));

                if (permission != null)
                {
                    return ($"Permission With Name '{permissionName}' All Ready Present");
                }

                Permission newPermisssion = new Permission();
                newPermisssion.PermissionName = permissionName;
                _context.Permissions.Add(newPermisssion);
                _context.SaveChanges();

                return (newPermisssion);
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
