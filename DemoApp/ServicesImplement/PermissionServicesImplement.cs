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
    public class PermissionServicesImplement : BaseServiceImplement, IPermissionsService
    {



       
        public PermissionServicesImplement(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, IHubContext<NotesHub> notesHub)
             : base(context, httpContextAccessor, notesHub)
        {
        }



        public async Task<string> AllocatePermissionToRoleAsync(RolePermissionDto rolePermissionDto)
        {
            try
            {
                var permission = _context.Permissions.Where(p => p.Id.ToString().Equals(rolePermissionDto.PermissionId)).FirstOrDefault();

                var role = _context.Roles.Where(r => r.Id.ToString().Equals(rolePermissionDto.RoleId)).FirstOrDefault();

                if (permission == null || role == null)
                {
                    return ("Assign Proper Role And Permission");
                }


                var RolePermissions = _context.RolePermissions.Where(rp => rp.PermissionId.ToString().Equals(rolePermissionDto.PermissionId) && rp.RoleId.ToString().Equals(rolePermissionDto.RoleId)).FirstOrDefault();
               
                if (RolePermissions != null)
                { 
                    return ($"Already  Permssion {permission.PermissionName} assign to role {role.RoleName}  "); 
                }


                RolePermission newRolePermission = new RolePermission();

                newRolePermission.PermissionId = permission.Id;
                newRolePermission.RoleId = role.Id;

                newRolePermission.Role = role;
                newRolePermission.Permission = permission;

                _context.RolePermissions.Add(newRolePermission);
                _context.SaveChanges();




                return ($" Allocation Of Permission {permission.PermissionName} to Role {role.RoleName}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                throw;
            }
        }

        public async Task<string> DeAllocatePermissionToRole(RolePermissionDto rolePermissionDto)
        {
            try
            {
                var permission = _context.Permissions.Where(p => p.Id.ToString().Equals(rolePermissionDto.PermissionId)).FirstOrDefault();

                var role = _context.Roles.Where(r => r.Id.ToString().Equals(rolePermissionDto.RoleId)).FirstOrDefault();

                if (permission == null || role == null)
                {
                    return ("Assign Proper Role And Permission");   
                }


                var RolePermission = await _context.RolePermissions.Where(rp => rp.Permission.Equals(permission) && rp.Role.Equals(role)).FirstOrDefaultAsync();



                _context.RolePermissions.Remove(RolePermission);
                _context.SaveChanges();


                return ($" DeAllocation Of Permission {permission.PermissionName} from the Role {role.RoleName}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                throw;
            }
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

        public async Task<object> GetAllPermissionByRoleId(string roleId)
        {
            try
            {
                if (!Guid.TryParse(roleId, out Guid parsedRoleId))
                {
                    return (new { Error = "Invalid role ID format.", ErrorCode = "INVALID_ROLE_ID" });
                }

                var role = _context.Roles.AsEnumerable().FirstOrDefault(r => r.Id == parsedRoleId);

                if (role == null)
                {
                    return (new { Error = $"Role with ID '{roleId}' not found.", ErrorCode = "ROLE_NOT_FOUND" });
                }

                // Retrieve permissions associated with the role, adjust as needed based on your entity relationships.
                var rolePermissions = _context.RolePermissions
                    .Include(rp => rp.Permission)
                    .Where(rp => rp.RoleId == role.Id)
                    .ToList();

                var permissions = rolePermissions.Select(rp => rp.Permission.PermissionName).Distinct().ToList();
                var permissionsNamesAndIds = rolePermissions
                                .Select(rp => new { PermissionName = rp.Permission.PermissionName, PermissionId = rp.Permission.Id })
                                .Distinct()
                                .ToList();

                return (permissionsNamesAndIds);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                throw;
            }
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
