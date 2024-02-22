using DemoApp.Hubs;
using DemoApp.Models;
using DemoApp.Services;
using DemoApp.ServicesImplement;
using DemoApp.utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using DemoApp.DTO;


namespace DemoApp.Controllers
{
    //[EnableCors("AllowAll")] // Enable CORS for this controller

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : BaseController
    {
        private readonly IUsersService _userService;


        public UserController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, IHubContext<NotesHub> notesHub, IUsersService userService)
            : base(context, httpContextAccessor, notesHub)
        {
            _userService = userService;
        }

        #region GetAllUsers

        [HttpGet]
        [Route("GetAllUsers")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _userService.GetAllUsersAsync();
                return Ok(users);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                Console.WriteLine($"An error occurred: {ex.Message}");
                return StatusCode(500, "An error occurred during while getting all Users");
            }
        }

        #endregion

        #region Login
        [HttpPost]
        [Route("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] UserDto user)
        {
            try
            {
                if ( string.IsNullOrEmpty(user.UserEmail) || string.IsNullOrEmpty(user.UserPassword))
                {
                    return BadRequest("Invalid user data");
                }

                var token = await _userService.LoginAsync(user);

                if (token == null)
                {
                    return Unauthorized("Invalid email or password");
                }

                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                Console.WriteLine($"An error occurred during login: {ex.Message}");
                return StatusCode(500, "An error occurred during login");
            }
           
        }
        #endregion

        #region Register

        [HttpPost]
        [Route("Register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] UserDto user)
        {

            try
            {
                if (user == null || string.IsNullOrEmpty(user.UserName) || string.IsNullOrEmpty(user.UserEmail) || string.IsNullOrEmpty(user.UserPassword))
                {
                    return BadRequest("Invalid user data");
                }

                var result = await _userService.RegisterAsync(user);

                if (result == "Registration successful")
                {
                    return Ok("Registration successful");
                }
                else if (result == "User already registered")
                {
                    return Conflict("User already registered");
                }
                else
                {
                    return StatusCode(500, "An error occurred during registration");
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                Console.WriteLine($"An error occurred during registration: {ex.Message}");
                return StatusCode(500, "An error occurred during registration");
            }
           
        }

        #endregion

        #region DeleteUser
        [HttpDelete]
        [Route("DeleteUser")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser([FromQuery] string userId)
        {

            try
            {
                var result = await _userService.DeleteUserAsync(userId);
                return Ok(result); // Return the result from the service method
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }


           
        }
        #endregion

        #region GetAllUserByRoleId
        [HttpGet]
        [Route("GetAllUserByRoleId")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> GetAllUserByRoleId([FromQuery] string roleId)
        {
            try
            {
                var result = await _userService.GetAllUserByRoleIdAsync(roleId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return StatusCode(500, new { Error = "Internal server error", ErrorCode = "INTERNAL_SERVER_ERROR" });
            }
            
        }
        #endregion

        #region getUserData

        [HttpGet]
        [Route("GetUserData")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> GetUserData()
        {


            try
            {
                var httpContext = _httpContextAccessor.HttpContext;

                var userData = await _userService.GetUserDataAsync(httpContext);

                if (userData != null)
                {
                    return Ok(userData);
                }
                else
                {
                    return NotFound("User not found or userId not found in claims");
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                Console.WriteLine($"An error occurred: {ex.Message}");
                return StatusCode(500, "An error occurred while retrieving user data");
            }
          
        }

        #endregion

    }



}
