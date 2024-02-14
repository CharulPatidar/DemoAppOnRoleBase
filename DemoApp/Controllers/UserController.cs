using DemoApp.Hubs;
using DemoApp.Models;
using DemoApp.utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text;


namespace DemoApp.Controllers
{
    //[EnableCors("AllowAll")] // Enable CORS for this controller

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : BaseController
    {

        public UserController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, IHubContext<NotesHub> notesHub)
            : base(context, httpContextAccessor, notesHub)
        {
        }

        #region GetAllUsers

        [HttpGet]
        [Route("GetAllUsers")]
        
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _context.Users.ToListAsync();
                return Ok(users);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                Console.WriteLine($"An error occurred: {ex.Message}");
                return Ok(null);
            }
        }

        #endregion


        #region Login
        [HttpPost]
        [Route("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest user)
        {
            try
            {
                user.UserPassword = EncrypteDecrypte.Encrypt(user.UserPassword);

                var existingUser = await _context.Users
                    .FirstOrDefaultAsync(u => u.UserEmail == user.UserEmail && u.UserPassword == user.UserPassword);

                if (existingUser == null)
                {
                    return Unauthorized();
                }


                var userRoles = await (
                                          from User in _context.Users
                                          join userRole in _context.UserRoles on User.Id equals userRole.UserId
                                          join role in _context.Roles on userRole.RoleId equals role.Id
                                          where User.Id == existingUser.Id
                                          select role.RoleName
                                      ).ToListAsync();
                
                var userRolesString = string.Join(", ", userRoles);
                var userid = existingUser.Id.ToString();
                var username = existingUser.UserName; // Replace with the actual username
                var userrole = string.Join(", ", userRoles);

                var key = Environment.GetEnvironmentVariable("SecretKey");

                if (key == null) { return BadRequest("null key "); }


                // Generate JWT token
                //var token = JwtTokenGenerator.GenerateJwtToken(username,userrole, userid, key);
                var token = JwtTokenGenerator.createToken(existingUser, userRoles, key);


                // Return the token as part of the authentication response
                return Ok(new { Token = token });

            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                Console.WriteLine($"An error occurred during registration: {ex.Message}");
                return Ok("Error during Login :  " +  ex.Message );
            }
        }
        #endregion

        #region Register

        [HttpPost]
        [Route("Register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] UserRegisterRequest user)
        {
            try
            {
                user.UserPassword = EncrypteDecrypte.Encrypt(user.UserPassword);



                // Check if the user already exists
                var existingUser = await _context.Users
                    .FirstOrDefaultAsync(u => u.UserEmail == user.UserEmail && u.UserPassword == user.UserPassword);

                if (existingUser != null)
                {
                    return Ok("User already registered");
                }

                // Create a new user
                User newUser = new User
                {
                    UserName = user.UserName,
                    UserEmail = user.UserEmail,
                    UserPassword = user.UserPassword
                };

                // Add the user to the context
                _context.Users.Add(newUser);

                // Save changes to the database
                await _context.SaveChangesAsync();

                return Ok("Registration successful");
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                Console.WriteLine($"An error occurred during registration: {ex.Message}");
                return Ok("Error during registration");
            }
        }

        #endregion

        #region GetAllNotes

        [HttpGet]
        [Route("GetAllNotes")]
        public async Task<IActionResult> GetAllNotes()
        {

            try
            {
                var allNotes = _context.Notes.ToList();

                if (allNotes != null)
                {
                    return Ok(allNotes);
                }
                else
                {
                    // Handle the case where allNotes is null
                    return NotFound("No notes found.");
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it accordingly
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }

        }



        #endregion


        #region getLoginUserData

        [HttpGet]
        [Route("GetUserData")]
        public async Task<IActionResult> GetUserData()
        {
            try
            {
                var httpContext = _httpContextAccessor.HttpContext;

                var token = httpContext.Request.Headers["Authorization"].FirstOrDefault()?.Replace("Bearer ", "");
                var key = Environment.GetEnvironmentVariable("SecretKey");

                JwtTokenGenerator.VerifyJwtToken(token, key, httpContext); //using this we set claims in  httpContext

                // Retrieve all claims from the HttpContext
                var allClaims = httpContext.User.Claims.ToList();

                // Create a dictionary to store claim types and values
                var claimsDictionary = new Dictionary<string, string>();

                // Populate the claims dictionary
                foreach (var claim in allClaims)
                {
                    claimsDictionary.Add(claim.Type, claim.Value);
                }

                

                // Retrieve userId from the dictionary
                if (claimsDictionary.ContainsKey("userId"))
                {
                    var userId = claimsDictionary["userId"];
                    var existingUser = await _context.Users
                        .FirstOrDefaultAsync(u => u.Id.ToString().Equals(userId));
                }


                var userData = new
                {
                    userName = claimsDictionary["userName"],
                    userId = claimsDictionary["userId"],
                    Roles = new[] { claimsDictionary["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"] },
                    userEmail = claimsDictionary["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress"],
                };

                


                return Ok(userData);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                Console.WriteLine($"An error occurred: {ex.Message}");
                return Ok(null);
            }
        }

        #endregion


    }


    #region BodyClasses

    public class UserLoginRequest
        {
            public string UserEmail { get; set; }
            public string UserPassword { get; set; }
        }

        public class UserRegisterRequest
        {
            public string UserName { get; set; }

            public string UserEmail { get; set; }

            public string UserPassword { get; set; }
        }

    #endregion
}
