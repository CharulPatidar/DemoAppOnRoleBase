using DemoApp.Hubs;
using DemoApp.Models;
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


        //done
        #region GetAllRoles
        [HttpGet]
        [Route("GetAllRoles")]
        public async Task<IActionResult> GetAllRoles()
        {
            try
            {
                var roles = _context.Roles.ToList();
                return Ok(roles);
            }
            catch (Exception ex)
            {
              
                Console.Error.WriteLine($"An error occurred while fetching roles: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        #endregion 
        //done
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

        #region addRole

        [HttpPost]
        [Route("InsertRole")]
        public async Task<IActionResult> InsertRole([FromBody] string roleName)
        {
            try
            {
                if (string.IsNullOrEmpty(roleName))
                {
                    return BadRequest("Role name cannot be empty");
                }

                var role = _context.Roles
                            .AsEnumerable()  //AsEnumerable or ToList to bring the data into memory before applying the case-insensitive comparison
                            .FirstOrDefault(r => r.RoleName.Equals(roleName, StringComparison.OrdinalIgnoreCase));

                if (role != null)
                {
                    return BadRequest($"Role With Name '{roleName}' All Ready Present");
                }

                Role newRole = new Role();
                newRole.RoleName = roleName;
                _context.Roles.Add(newRole);
                _context.SaveChanges();

                return Ok(newRole);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                Console.WriteLine($"An error occurred: {ex.Message}");
                return StatusCode(500, "Internal server error");
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


        #region DeleteUser
        [HttpDelete]
        [Route("DeleteUser")]
        public async Task<IActionResult> DeleteUser([FromQuery] string userId)
        {
            try
            {
                if (string.IsNullOrEmpty(userId))
                {
                    return BadRequest("UserId  cannot be empty");
                }

                var user = _context.Users
                            .AsEnumerable()  //AsEnumerable or ToList to bring the data into memory before applying the case-insensitive comparison
                            .FirstOrDefault(u => u.Id.ToString().Equals(userId, StringComparison.OrdinalIgnoreCase));

                if (user != null)
                {
                    _context.Users.Remove(user);
                    _context.SaveChanges();

                    return Ok($"User With Name '{user.UserName}' Deleted");
                }

                return BadRequest($"User '{user.UserName}' Not Found ");
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                Console.WriteLine($"An error occurred: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
        #endregion


        #region deleteRole
        [HttpDelete]
        [Route("DeleteRole")]
        public async Task<IActionResult> DeleteRole([FromQuery] string RoleId)
        {
            try
            {
                if (string.IsNullOrEmpty(RoleId))
                {
                    return BadRequest("Role name cannot be empty");
                }

                var Role = _context.Roles
                            .AsEnumerable()  
                            .FirstOrDefault(r => r.Id.ToString().Equals(RoleId, StringComparison.OrdinalIgnoreCase));

                if (Role != null)
                {
                    _context.Roles.Remove(Role);
                    _context.SaveChanges();

                    return Ok($"Role With Name '{Role.RoleName}' Deleted");
                }

                return BadRequest($"Role '{Role.RoleName}' Not Found ");
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
        //done
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


        //done
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
        //done
        #region Allocate Role To User
        [HttpPost]
        [Route("AllocateRoleToUser")]
        public async Task<IActionResult> AllocateRoleToUser([FromBody] DtoUserRole dtoUserRole)
        {
            try
            {
                var user = _context.Users.Where(u => u.Id.ToString().Equals(dtoUserRole.UserId)).FirstOrDefault();

                var role = _context.Roles.Where(r => r.Id.ToString().Equals(dtoUserRole.RoleId)).FirstOrDefault();

                if (user == null || role == null) { return BadRequest("Assign Proper Role And User"); }


                var userRole =  _context.UserRoles.Where(ur => ur.UserId.ToString().Equals(dtoUserRole.UserId)  && ur.RoleId.ToString().Equals(dtoUserRole.RoleId)).FirstOrDefault();

                if(userRole != null) { return BadRequest($"Already role {role.RoleName} assign to user {user.UserName} "); }

                UserRole newUserRole = new UserRole();

                newUserRole.UserId = user.Id;
                newUserRole.RoleId = role.Id;

                newUserRole.Role = role;
                newUserRole.User = user;

                _context.UserRoles.Add(newUserRole);
                _context.SaveChanges();




                return Ok($" Allocation Of Role {role.RoleName} to User {user.UserName}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
        #endregion
        //done
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
        //done
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


        //done
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

        //done
        #region GetAllUserByRoleId
        [HttpGet]
        [Route("GetAllUserByRoleId")]
        public async Task<IActionResult> GetAllUserByRoleId([FromQuery] string roleId)
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

                // Retrieve users associated with the role, adjust as needed based on your entity relationships.
                var userRoles = _context.UserRoles
                    .Include(rp => rp.User)
                    .Where(rp => rp.RoleId == role.Id)
                    .ToList();


                var userNames = userRoles.Select(rp => rp.User.UserName).Distinct().ToList();
                var userNamesAndIds = userRoles
                                        .Select(rp => new { UserName = rp.User.UserName, UserId = rp.User.Id })  // Select user name and user ID
                                        .Distinct()  // Ensure distinct combinations of user name and user ID
                                        .ToList();


                return Ok(userNamesAndIds);


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
