using DemoApp.Hubs;
using DemoApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Globalization;

namespace DemoApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin, Developer, Intern, Manager, TeamLead")]

    public class DeveloperController : BaseController
    {

        public DeveloperController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, IHubContext<NotesHub> notesHub)
            : base(context, httpContextAccessor, notesHub)
        {
        }


        #region AddNotes

        [HttpPost]
        [Route("AddNotes")]
        public async Task<IActionResult> AddNotes([FromBody]DtoUserNotes dtoUserNotes)
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


                dtoUserNotes.UserId = userId;
            }

            try
            {
                if (dtoUserNotes != null && !String.IsNullOrEmpty(dtoUserNotes.UserId) && !String.IsNullOrEmpty(dtoUserNotes.NoteTopic) && !String.IsNullOrEmpty(dtoUserNotes.NoteDescription))
                {
                    var user = _context.Users.Where(u => u.Id.ToString().Equals(dtoUserNotes.UserId)).FirstOrDefault();

                    if (user == null)
                    {
                        // User not found
                        return NotFound("User not found");
                    }

                    Notes newNotes = new Notes
                    {
                        Topic = dtoUserNotes.NoteTopic,
                        Description = dtoUserNotes.NoteDescription
                    };

                    _context.Notes.Add(newNotes);
                    _context.SaveChanges();

                    var noteId = newNotes.Id;

                    var note = _context.Notes.Where(u => u.Id.Equals(noteId)).FirstOrDefault();

                    if (note == null)
                    {
                        // Note not found
                        return NotFound("Note not found");
                    }

                    UserNotes userNotes = new UserNotes();

                    userNotes.NoteId = noteId;
                    userNotes.UserId = user.Id;
                    userNotes.User = user;
                    userNotes.Note = note;


                    _context.UserNotes.Add(userNotes);
                    _context.SaveChanges();

                    await _notesHub.Clients.All.SendAsync("ReceiveMsg", "HIIIIIII");

                    return Ok($"Note {newNotes.Topic} Created By User {user.UserName} ");


                }
                return Ok("Some Problem while adding add proper note data");
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

    public class DtoUserNotes
    {

        public string UserId { get; set; }

        public string NoteTopic { get; set; }

        public string NoteDescription { get; set; }
    }
    #endregion
}
