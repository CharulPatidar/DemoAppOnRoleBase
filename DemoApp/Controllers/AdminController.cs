using DemoApp.Hubs;
using DemoApp.Models;
using DemoApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Security;

namespace DemoApp.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class AdminController : BaseController
    {

        public AdminController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, IHubContext<NotesHub> notesHub)
            : base(context, httpContextAccessor, notesHub)
        {
        }


      
        
        #region GetAllPermission
        [HttpGet]
        [Route("GetAllPermission")]
        public async Task<IActionResult> GetAllPermission()
        {
            try
            {
                var permissions = _context.Permissions.ToList();
                return Ok(permissions);
            }
            catch (Exception ex)
            {
               
                Console.Error.WriteLine($"An error occurred while fetching permissions: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        #endregion


        #region addPermission
        [HttpPost]
        [Route("InsertPermission")]
        public async Task<IActionResult> InsertPermission([FromBody] string permissionName)
        {
            try
            {
                if (string.IsNullOrEmpty(permissionName))
                {
                    return BadRequest("Permisssion name cannot be empty");
                }

                var permission = _context.Permissions
                            .AsEnumerable()  //AsEnumerable or ToList to bring the data into memory before applying the case-insensitive comparison
                            .FirstOrDefault(p => p.PermissionName.Equals(permissionName, StringComparison.OrdinalIgnoreCase));

                if (permission != null)
                {
                    return BadRequest($"Permission With Name '{permissionName}' All Ready Present");
                }

                Permission newPermisssion = new Permission();
                newPermisssion.PermissionName = permissionName;
                _context.Permissions.Add(newPermisssion);
                _context.SaveChanges();

                return Ok($"Permission '{permissionName}' inserted");
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                Console.WriteLine($"An error occurred: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        #endregion


     



        #region deletePermission
        [HttpDelete]
        [Route("DeletePermission")]
        public async Task<IActionResult> DeletePermission([FromQuery] string PermissionId)
        {
            try
            {
                if (string.IsNullOrEmpty(PermissionId))
                {
                    return BadRequest("Permisssion name cannot be empty");
                }

                var permission = _context.Permissions
                            .AsEnumerable()  //AsEnumerable or ToList to bring the data into memory before applying the case-insensitive comparison
                            .FirstOrDefault(p => p.Id.ToString().Equals(PermissionId, StringComparison.OrdinalIgnoreCase));

                if (permission != null)
                {
                    _context.Permissions.Remove(permission);
                    _context.SaveChanges();

                    return Ok($"Permission With Name '{permission.PermissionName}' Deleted");
                }

                return BadRequest($"Permission '{permission.PermissionName}' Not Found ");
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                Console.WriteLine($"An error occurred: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
        #endregion
       
        #region GetAllUsers
        [HttpGet]
        [Route("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = _context.Users.ToList();
                return Ok(users);
            }
            catch (Exception ex)
            {
               
                Console.Error.WriteLine($"An error occurred while fetching users: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        #endregion


        #region Allocate Permission To Role
        [HttpPost]
        [Route("AllocatePermissionToRole")]
        public async Task<IActionResult> AllocatePermissionToRole([FromBody]DtoRolePermission dtoRolePermission )
        {
            try
            {
                var permission = _context.Permissions.Where(p => p.Id.ToString().Equals(dtoRolePermission.PermissionId)).FirstOrDefault();

                var role = _context.Roles.Where(r => r.Id.ToString().Equals(dtoRolePermission.RoleId)).FirstOrDefault();

                if(permission == null || role == null) { return BadRequest("Assign Proper Role And Permission"); }

                var RolePermissions = _context.RolePermissions.Where(rp => rp.PermissionId.ToString().Equals(dtoRolePermission.PermissionId) && rp.RoleId.ToString().Equals(dtoRolePermission.RoleId)).FirstOrDefault();


                if (RolePermissions != null) { return BadRequest($"Already  Permssion {permission.PermissionName} assign to role {role.RoleName}  "); }


                RolePermission newRolePermission = new RolePermission();

                newRolePermission.PermissionId = permission.Id;
                newRolePermission.RoleId = role.Id;

                newRolePermission.Role = role;
                newRolePermission.Permission = permission;

                _context.RolePermissions.Add(newRolePermission);
                _context.SaveChanges();




                return Ok($" Allocation Of Permission {permission.PermissionName} to Role { role.RoleName }");
            }
            catch(Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
        #endregion
       
       
       
        #region DeAllocate Permission To Role
        [HttpPost]
        [Route("DeAllocatePermissionToRole")]
        public async Task<IActionResult> DeAllocatePermissionToRole([FromBody] DtoRolePermission dtoRolePermission)
        {
            try
            {
                var permission = _context.Permissions.Where(p => p.Id.ToString().Equals(dtoRolePermission.PermissionId)).FirstOrDefault();

                var role = _context.Roles.Where(r => r.Id.ToString().Equals(dtoRolePermission.RoleId)).FirstOrDefault();

                if (permission == null || role == null) { return BadRequest("Assign Proper Role And Permission"); }


               var RolePermission =  _context.RolePermissions.Where(rp => rp.Permission.Equals(permission) && rp.Role.Equals(role)).FirstOrDefault();

             

                _context.RolePermissions.Remove(RolePermission);
                _context.SaveChanges();


                return Ok($" DeAllocation Of Permission {permission.PermissionName} from Role {role.RoleName}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
        #endregion
       
        #region DeAllocate Role To User
        [HttpPost]
        [Route("DeAllocateRoleToUser")]
        public async Task<IActionResult> DeAllocateRoleToUser([FromBody] DtoUserRole dtoUserRole)
        {
            try
            {
                var user = _context.Users.Where(p => p.Id.ToString().Equals(dtoUserRole.UserId)).FirstOrDefault();

                var role = _context.Roles.Where(r => r.Id.ToString().Equals(dtoUserRole.RoleId)).FirstOrDefault();

                if (user == null || role == null) { return BadRequest("Assign Proper Role And user"); }


                var userRole = _context.UserRoles.Where(rp => rp.User.Equals(user) && rp.Role.Equals(role)).FirstOrDefault();



                _context.UserRoles.Remove(userRole);
                _context.SaveChanges();


                return Ok($" DeAllocation Of User {user.UserName} from Role {role.RoleName}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
        #endregion


        #region GetAllPermissionByRoleId

        [HttpGet]
        [Route("GetAllPermissionByRoleId")]
        public async Task<IActionResult> GetAllPermissionByRoleId([FromQuery] string roleId)
        {
            try
            {
                if (!Guid.TryParse(roleId, out Guid parsedRoleId))
                {
                    return BadRequest(new { Error = "Invalid role ID format.", ErrorCode = "INVALID_ROLE_ID" });
                }

                var role = _context.Roles.AsEnumerable().FirstOrDefault(r => r.Id == parsedRoleId);

                if (role == null)
                {
                    return NotFound(new { Error = $"Role with ID '{roleId}' not found.", ErrorCode = "ROLE_NOT_FOUND" });
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

                return Ok(permissionsNamesAndIds);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return StatusCode(500, new { Error = "Internal server error", ErrorCode = "INTERNAL_SERVER_ERROR" });
            }
        }

        #endregion

       
    }



    #region bodyClasses

    public class DtoUserRole
    {
        public string UserId { get; set; }

        public string RoleId { get; set; }

    }

    public class DtoRolePermission
    {
        public string RoleId { get; set; }
        public string PermissionId { get; set; }
    }

    #endregion
}
