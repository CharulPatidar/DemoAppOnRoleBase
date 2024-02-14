using DemoApp.Hubs;
using DemoApp.Models;
using DemoApp.utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace DemoApp.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin, Manager, TeamLead")]

    public class TeamLeadController : BaseController
    {

        public TeamLeadController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor , IHubContext<NotesHub> notesHub)
            : base(context, httpContextAccessor, notesHub)
        {
        }


        #region update/EditNotesById
        [HttpPut]
        [Route("UpdateNoteById")]
        public async Task<IActionResult> UpdateNoteById([FromBody] DtoUserNotesUpdate dtoUserNotesUpdate)
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


                dtoUserNotesUpdate.UserId = userId;
            }
            try
            {
                if(dtoUserNotesUpdate != null) 
                {
                    var note = _context.Notes.Where(n => n.Id.ToString().Equals(dtoUserNotesUpdate.NoteId)).FirstOrDefault();
                    var user = _context.Users.Where(u => u.Id.ToString().Equals(dtoUserNotesUpdate.UserId)).FirstOrDefault();

                    if(note != null && user != null)
                    {

                        note.Topic = dtoUserNotesUpdate.NoteTopic;
                        note.Description = dtoUserNotesUpdate.NoteDescription;

                        _context.Notes.Update(note);
                        _context.SaveChanges();
                        return Ok($"update notes by {user.UserName}");
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

    public class DtoUserNotesUpdate
    {

        public string UserId { get; set; }

        public string NoteId { get; set; }

        public string NoteTopic { get; set; }

        public string NoteDescription { get; set; }
    }
    #endregion
}
