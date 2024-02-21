using DemoApp.DTO;
using DemoApp.Hubs;
using DemoApp.Models;
using DemoApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace DemoApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionController : BaseController
    {
        private readonly IPermissionsService  _permissionsService;


        public PermissionController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, IHubContext<NotesHub> notesHub, IPermissionsService permissionsService)
          : base(context, httpContextAccessor, notesHub)
        {
            _permissionsService = permissionsService;
        }




        #region GetAllPermission
        [HttpGet]
        [Route("GetAllPermission")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllPermission()
        {
            try
            {
               var result = await  _permissionsService.GetAllPermissionAsync();

                if(result == null)
                {
                    return NotFound();
                }

                return Ok(result);
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
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> InsertPermission([FromBody] string permissionName)
        {
            try
            {
                var result = await _permissionsService.InsertPermissionAsync(permissionName);

                if (result is string)
                {
                    return BadRequest(result);
                }
                else if (result is Permission)
                {
                    return Ok(result);
                }
                else
                {
                    return StatusCode(500, "Internal server error");
                }
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
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> DeletePermission([FromQuery] string PermissionId)
        {
            try
            {
                var result = await _permissionsService.DeletePermissionAsync(PermissionId);

                if (result is string)
                {
                    return BadRequest(result);
                }
                else if (result is Permission)
                {
                    var permission = result as Permission;
                    return Ok(permission.PermissionName);
                }
                else
                {
                    return StatusCode(500, "Internal server error");
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                Console.WriteLine($"An error occurred: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
        #endregion

        #region Allocate Permission To Role
        [HttpPost]
        [Route("AllocatePermissionToRole")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> AllocatePermissionToRole([FromBody] RolePermissionDto rolePermissionDto)
        {
            try
            {
                var result = await _permissionsService.AllocatePermissionToRoleAsync(rolePermissionDto);


                if (result == null || result.Contains("Assign Proper Role And Permission") || result.Contains("Already  Permssion"))
                {
                    return BadRequest(result);
                }


                // Return Ok if the allocation was successful
                return Ok(result);

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
                var result = await _permissionsService.GetAllPermissionByRoleId(roleId);

                // Check if the result is an error object
                if (result.GetType() == typeof(object) && result.GetType().GetProperty("Error") != null)
                {
                    // If it is, return an error response
                    var errorCode = result.GetType().GetProperty("ErrorCode").GetValue(result);
                    return StatusCode(400, result);
                }

                // If no errors, return the permissions list
                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return StatusCode(500, new { Error = "Internal server error", ErrorCode = "INTERNAL_SERVER_ERROR" });
            }
        }

        #endregion

        #region DeAllocate Permission To Role
        [HttpPost]
        [Route("DeAllocatePermissionToRole")]
        public async Task<IActionResult> DeAllocatePermissionToRole([FromBody] RolePermissionDto rolePermissionDto)
        {
            try
            {
                var result = await _permissionsService.DeAllocatePermissionToRole(rolePermissionDto);


                if (result == null || result.Contains("Assign Proper Role And Permission"))
                {
                    return BadRequest(result);

                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }

        }
        #endregion





    }
}
