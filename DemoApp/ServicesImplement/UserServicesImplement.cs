using DemoApp.DTO;
using DemoApp.Hubs;
using DemoApp.Models;
using DemoApp.Services;
using DemoApp.utilities;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Collections;
using System.Security.Cryptography;
using System.Text;


namespace DemoApp.ServicesImplement
{



    public class UserServicesImplement : BaseServiceImplement, IUsersService
    {
        private readonly IConfiguration _configuration;

      
        public UserServicesImplement(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, IHubContext<NotesHub> notesHub, IConfiguration configuration)
          : base(context, httpContextAccessor, notesHub)

        {
            _configuration = configuration;
        }


        public async Task<object> DeleteUserAsync(string userId)
        {
            try
            {
                if (string.IsNullOrEmpty(userId))
                {
                    return ("UserId  cannot be empty");
                }

                var user = _context.Users
                            .AsEnumerable()  //AsEnumerable or ToList to bring the data into memory before applying the case-insensitive comparison
                            .FirstOrDefault(u => u.Id.ToString().Equals(userId, StringComparison.OrdinalIgnoreCase));

                if (user != null)
                {
                    _context.Users.Remove(user);
                    _context.SaveChanges();

                    return ($"User With Name '{user.UserName}' Deleted");
                }

                 return($"User '{user.UserName}' Not Found ");
                
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                Console.WriteLine($"An error occurred: {ex.Message}");
                throw;
            }
        }

        public async Task<object> GetAllUserByRoleIdAsync(string roleId)
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


                return (userNamesAndIds);


            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                throw;
            }
        }

        public async Task<object> GetAllUsersAsync()
        {
            try
            {
                var users = await _context.Users.Select(u => new
                {
                    u.Id,
                    u.UserName,
                    u.UserEmail
                }).ToListAsync();

                return (users);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                Console.WriteLine($"An error occurred: {ex.Message}");
                throw;
            }
        }

        public async Task<object> GetUserDataAsync(HttpContext httpContext)
        {
            try
            {
                var token = httpContext.Request.Headers["Authorization"].FirstOrDefault()?.Replace("Bearer ", "");
                //  var key = Environment.GetEnvironmentVariable("SecretKey");
                var key = _configuration["Jwt:SecretKey"];

                JwtTokenGenerator.VerifyJwtToken(token, key, httpContext); // Verify and set claims in HttpContext

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

                    if (existingUser != null)
                    {
                        var userData = new
                        {
                            userName = claimsDictionary["userName"],
                            userId = claimsDictionary["userId"],
                            Roles = new[] { claimsDictionary["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"] },
                            userEmail = claimsDictionary["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress"],
                        };

                        return userData;
                    }
                }

                return null; // Return null if user not found or userId not found in claims
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                Console.WriteLine($"An error occurred: {ex.Message}");
                throw; // Rethrow the exception to be handled by the controller
            }
        }

        public async Task<string> LoginAsync(UserDto userDto)
        {
            
           try
            {
                string encryptedPassword = EncrypteDecrypte.Encrypt(userDto.UserPassword);
                //hashedpassword

                //var userSalt = await _context.Users
                //    .Where(u => u.UserEmail == userDto.UserEmail)
                //    .Select(u => u.UserSalt)
                //    .FirstOrDefaultAsync();

                //byte[] saltBytes = Convert.FromBase64String(userSalt); // Convert userSalt to byte array


                //string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                //   password: encryptedPassword,
                //   salt: saltBytes,
                //   prf: KeyDerivationPrf.HMACSHA256,
                //   iterationCount: 100000,
                //   numBytesRequested: 256 / 8));

               // var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.UserEmail == userDto.UserEmail && u.UserHashedPassword == hashed);

                //hashedpassword-------
                var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.UserEmail == userDto.UserEmail && u.UserPassword == encryptedPassword);

                if (existingUser == null)
                {
                    return null;
                }

                var userRoles = await(from u in _context.Users
                                      join ur in _context.UserRoles on u.Id equals ur.UserId
                                      join r in _context.Roles on ur.RoleId equals r.Id
                                      where u.Id == existingUser.Id
                select r.RoleName).ToListAsync();



                //var key = Environment.GetEnvironmentVariable("SecretKey");
                var key = _configuration["Jwt:SecretKey"];

                if (key == null) { return ("null key "); }

                var token = JwtTokenGenerator.createJwtToken(existingUser, userRoles, key);

                Console.WriteLine("server side - >  ", token);

                return token;
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                Console.WriteLine($"An error occurred during login: {ex.Message}");
                return ("An Ex occurred during login");
            }

        }

        public async Task<string> RegisterAsync(UserDto userDto)
        {
            try
            {
                if (userDto.UserName.IsNullOrEmpty() || userDto.UserEmail.IsNullOrEmpty() || userDto.UserPassword.IsNullOrEmpty())
                {
                    return "null data";
                }

                userDto.UserPassword = EncrypteDecrypte.Encrypt(userDto.UserPassword);

                // Check if the user already exists
                var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.UserEmail == userDto.UserEmail);

                if (existingUser != null)
                {
                    return "User already registered";
                }
                // hashed password --

                byte[] salt = RandomNumberGenerator.GetBytes(128 / 8); // divide by 8 to convert bits to bytes
                var userSalt = Convert.ToBase64String(salt);
                Console.WriteLine($"Salt: {userSalt}");

                // derive a 256-bit subkey (use HMACSHA256 with 100,000 iterations)
                string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: userDto.UserPassword,
                    salt: salt,
                    prf: KeyDerivationPrf.HMACSHA256,
                    iterationCount: 100000,
                    numBytesRequested: 256 / 8));

                Console.WriteLine($"Hashed: {hashed}");

                 // return null;
                //-- hashed password
                // Create a new user
                User newUser = new User
                {
                    UserName = userDto.UserName,
                    UserEmail = userDto.UserEmail,
                    UserPassword = userDto.UserPassword,
                    UserSalt = userSalt,
                    UserHashedPassword = hashed
                };

                // Add the user to the context
                _context.Users.Add(newUser);

                // Save changes to the database
                await _context.SaveChangesAsync();

                return "Registration successful";
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                Console.WriteLine($"An error occurred during registration: {ex.Message}");
                return "Error during registration";
            }
        }
    }
}
