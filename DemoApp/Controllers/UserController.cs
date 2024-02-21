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
            #region Comment


            //try
            //{

            //    //-------------
            //   if ( user.UserEmail.IsNullOrEmpty() || user.UserPassword.IsNullOrEmpty() ) { return BadRequest("null data "); }

            //    user.UserPassword = EncrypteDecrypte.Encrypt(user.UserPassword);

            //    var existingUser = await _context.Users
            //        .FirstOrDefaultAsync(u => u.UserEmail == user.UserEmail && u.UserPassword == user.UserPassword);

            //    if (existingUser == null)
            //    {
            //        return Unauthorized();
            //    }


            //    var userRoles = await (
            //                              from User in _context.Users
            //                              join userRole in _context.UserRoles on User.Id equals userRole.UserId
            //                              join role in _context.Roles on userRole.RoleId equals role.Id
            //                              where User.Id == existingUser.Id
            //                              select role.RoleName
            //                          ).ToListAsync();

            //    var userRolesString = string.Join(", ", userRoles);
            //    var userid = existingUser.Id.ToString();
            //    var username = existingUser.UserName; // Replace with the actual username
            //    var userrole = string.Join(", ", userRoles);

            //    var key = Environment.GetEnvironmentVariable("SecretKey");

            //    if (key == null) { return BadRequest("null key "); }


            //    // Generate JWT token
            //    //var token = JwtTokenGenerator.GenerateJwtToken(username,userrole, userid, key);
            //    var token = JwtTokenGenerator.createToken(existingUser, userRoles, key);


            //    // Return the token as part of the authentication response
            //    return Ok(new { Token = token });

            //}
            //catch (Exception ex)
            //{
            //    // Log the exception or handle it as needed
            //    Console.WriteLine($"An error occurred during registration: {ex.Message}");
            //    return Ok("Error during Login :  " +  ex.Message );
            //}
            #endregion
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
            #region Comment


            //try
            //{
            //    if (user.UserName.IsNullOrEmpty() || user.UserEmail.IsNullOrEmpty() || user.UserPassword.IsNullOrEmpty()) { return BadRequest("null data "); }


            //    user.UserPassword = EncrypteDecrypte.Encrypt(user.UserPassword);



            //    // Check if the user already exists
            //    var existingUser = await _context.Users
            //        .FirstOrDefaultAsync(u => u.UserEmail == user.UserEmail);

            //    if (existingUser != null)
            //    {
            //        return BadRequest("User already registered");
            //    }

            //    // Create a new user
            //    User newUser = new User
            //    {
            //        UserName = user.UserName,
            //        UserEmail = user.UserEmail,
            //        UserPassword = user.UserPassword
            //    };

            //    // Add the user to the context
            //    _context.Users.Add(newUser);

            //    // Save changes to the database
            //    await _context.SaveChangesAsync();

            //    return Ok("Registration successful");
            //}
            //catch (Exception ex)
            //{
            //    // Log the exception or handle it as needed
            //    Console.WriteLine($"An error occurred during registration: {ex.Message}");
            //    return Ok("Error during registration");
            //}
            #endregion
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


            //try
            //{
            //    if (string.IsNullOrEmpty(userId))
            //    {
            //        return BadRequest("UserId  cannot be empty");
            //    }

            //    var user = _context.Users
            //                .AsEnumerable()  //AsEnumerable or ToList to bring the data into memory before applying the case-insensitive comparison
            //                .FirstOrDefault(u => u.Id.ToString().Equals(userId, StringComparison.OrdinalIgnoreCase));

            //    if (user != null)
            //    {
            //        _context.Users.Remove(user);
            //        _context.SaveChanges();

            //        return Ok($"User With Name '{user.UserName}' Deleted");
            //    }

            //    return BadRequest($"User '{user.UserName}' Not Found ");
            //}
            //catch (Exception ex)
            //{
            //    // Log the exception or handle it as needed
            //    Console.WriteLine($"An error occurred: {ex.Message}");
            //    return StatusCode(500, "Internal server error");
            //}
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
            //try
            //{
            //    if (!Guid.TryParse(roleId, out Guid parsedRoleId))
            //    {
            //        return BadRequest(new { Error = "Invalid role ID format.", ErrorCode = "INVALID_ROLE_ID" });
            //    }

            //    var role = _context.Roles.AsEnumerable().FirstOrDefault(r => r.Id == parsedRoleId);

            //    if (role == null)
            //    {
            //        return NotFound(new { Error = $"Role with ID '{roleId}' not found.", ErrorCode = "ROLE_NOT_FOUND" });
            //    }

            //    // Retrieve users associated with the role, adjust as needed based on your entity relationships.
            //    var userRoles = _context.UserRoles
            //        .Include(rp => rp.User)
            //        .Where(rp => rp.RoleId == role.Id)
            //        .ToList();


            //    var userNames = userRoles.Select(rp => rp.User.UserName).Distinct().ToList();
            //    var userNamesAndIds = userRoles
            //                            .Select(rp => new { UserName = rp.User.UserName, UserId = rp.User.Id })  // Select user name and user ID
            //                            .Distinct()  // Ensure distinct combinations of user name and user ID
            //                            .ToList();


            //    return Ok(userNamesAndIds);


            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine($"An error occurred: {ex.Message}");
            //    return StatusCode(500, new { Error = "Internal server error", ErrorCode = "INTERNAL_SERVER_ERROR" });
            //}
        }
        #endregion


        #region getUserData

        [HttpGet]
        [Route("GetUserData")]
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
            #region comment


            //try
            //{
            //    var httpContext = _httpContextAccessor.HttpContext;

            //    var token = httpContext.Request.Headers["Authorization"].FirstOrDefault()?.Replace("Bearer ", "");
            //    var key = Environment.GetEnvironmentVariable("SecretKey");

            //    JwtTokenGenerator.VerifyJwtToken(token, key, httpContext); //using this we set claims in  httpContext

            //    // Retrieve all claims from the HttpContext
            //    var allClaims = httpContext.User.Claims.ToList();

            //    // Create a dictionary to store claim types and values
            //    var claimsDictionary = new Dictionary<string, string>();

            //    // Populate the claims dictionary
            //    foreach (var claim in allClaims)
            //    {
            //        claimsDictionary.Add(claim.Type, claim.Value);
            //    }



            //    // Retrieve userId from the dictionary
            //    if (claimsDictionary.ContainsKey("userId"))
            //    {
            //        var userId = claimsDictionary["userId"];
            //        var existingUser = await _context.Users
            //            .FirstOrDefaultAsync(u => u.Id.ToString().Equals(userId));
            //    }


            //    var userData = new
            //    {
            //        userName = claimsDictionary["userName"],
            //        userId = claimsDictionary["userId"],
            //        Roles = new[] { claimsDictionary["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"] },
            //        userEmail = claimsDictionary["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress"],
            //    };




            //    return Ok(userData);
            //}
            //catch (Exception ex)
            //{
            //    // Log the exception or handle it as needed
            //    Console.WriteLine($"An error occurred: {ex.Message}");
            //    return Ok(null);
            //}
            #endregion
        }

        #endregion

    }



}
