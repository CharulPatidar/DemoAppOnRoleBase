using DemoApp.DTO;
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
    [Authorize]
    public class RoleController : BaseController
    {
        private readonly IRolesService _rolesService;

        public RoleController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, IHubContext<NotesHub> notesHub, IRolesService rolesService)
            : base(context, httpContextAccessor, notesHub)
        {
            _rolesService = rolesService;
        }



        #region GetAllRoles
        [HttpGet]
        [Route("GetAllRoles")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> GetAllRoles()
        {
            try
            {
                var roles = await _rolesService.GetAllRolesAsync();
                return Ok(roles);
            }
            catch (Exception ex)
            {

                Console.Error.WriteLine($"An error occurred while fetching roles: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        #endregion

        #region addRole

        [HttpPost]
        [Route("InsertRole")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> InsertRole([FromBody] string roleName)
        {
            try
            {

                var result = await _rolesService.InsertRoleAsync(roleName);

                if (result is string)
                {
                    // If the result is a string, return it as a BadRequest
                    return BadRequest(result);
                }
                else if (result is Role)
                {
                    // If the result is a Role object, return it as an OK response
                    return Ok(result);
                }
                else
                {
                    // If the result is neither string nor Role, return a 500 Internal Server Error
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

        #region deleteRole
        [HttpDelete]
        [Route("DeleteRole")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> DeleteRole([FromQuery] string RoleId)
        {
            try
            {
                var result =  await _rolesService.DeleteRoleAsync(RoleId);

                if (result is string)
                {
                    // If the result is a string, return it as a BadRequest
                    return BadRequest(result);
                }
                else if (result is Role)
                {
                    var role = result as Role;

                    // If the result is a Role object, return it as an OK response
                    return Ok($"Role With Name '{role.RoleName}' Deleted");
                }
                else
                {
                    // If the result is neither string nor Role, return a 500 Internal Server Error
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

        #region Allocate Role To User
        [HttpPost]
        [Route("AllocateRoleToUser")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> AllocateRoleToUser([FromBody] UserRoleDto userRoleDto)
        {
            try
            {

                var result = await _rolesService.AllocateRoleToUserAsync(userRoleDto);

                if (result == null || result.Contains("Assign Proper Role And User") || result.Contains("Already role") )
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


        #region DeAllocate Role To User
        [HttpPost]
        [Route("DeAllocateRoleToUser")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> DeAllocateRoleToUser([FromBody] UserRoleDto userRoleDto)
        {
            try
            {

                var result = await _rolesService.DeAllocateRoleToUserAsync(userRoleDto);

                if (result == null || result.Contains("Assign Proper Role And user") || result.Contains("Already role"))
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


    }
}
