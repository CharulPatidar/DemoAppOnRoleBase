using DemoApp.Hubs;
using DemoApp.Models;
using DemoApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

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


    }
}
