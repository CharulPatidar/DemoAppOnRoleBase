using DemoApp.Hubs;
using DemoApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace DemoApp.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin, ManagerS")]

    public class ManagerController : BaseController
    {

        public ManagerController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, IHubContext<NotesHub> notesHub)
            : base(context, httpContextAccessor, notesHub)
        {
        }

        #region DeleteNoteById

        [HttpDelete]
        [Route("DeleteNoteById")]
        public async Task<IActionResult> DeleteNoteById([FromBody] DtoUserNotesDelete dtoUserNotesDelete)
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

            // Retrieve the value of the userId claim
            if (claimsDictionary.TryGetValue("userId", out string userId))
            {
                Console.WriteLine(userId);


                dtoUserNotesDelete.UserId = userId;
            }

            try
            {
                if (dtoUserNotesDelete != null)
                {
                    var note = _context.Notes.Where(n => n.Id.ToString().Equals(dtoUserNotesDelete.NoteId)).FirstOrDefault();
                    var user = _context.Users.Where(u => u.Id.ToString().Equals(dtoUserNotesDelete.UserId)).FirstOrDefault();

                    if (note != null && user != null)
                    {

                         

                        _context.Notes.Remove(note);
                        _context.SaveChanges();
                        return Ok($"Delete notes by {user.UserName}");
                    }

                    return BadRequest("User or Note not find");
                }

                return BadRequest("Send data Properly");
            }
            catch (Exception ex)
            {
                // Log the exception or handle it accordingly
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }

        }

        #endregion

    }

    #region BodyClasses
    public class DtoUserNotesDelete
    {
        public string UserId { get; set; }

        public string NoteId { get; set; }

    }
    #endregion
}
