using DemoApp.Hubs;
using DemoApp.DTO;
using DemoApp.Models;
using DemoApp.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Text.Json;
using Microsoft.Identity.Client;
using DemoApp.utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Rewrite;

namespace DemoApp.Controllers
{
    [Route("api/[controller]")]
    public class NoteController : BaseController
    {
        private readonly INotesService _notesService;

        public NoteController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, IHubContext<NotesHub> notesHub, INotesService notesService)
            : base(context, httpContextAccessor, notesHub)
        {
            _notesService = notesService;
        }


        #region GetAllNotes

        [HttpGet]
        [Route("GetAllNotes")]
        public async Task<IActionResult> GetAllNotes()
        {

            try
            {

                var result = await _notesService.GetAllNotes();


                if (result != null)
                {
                    return Ok(result);
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

        #region AddNotes

        [HttpPost]
        [Route("AddNotes")]
        [Authorize(Roles = "Admin, Developer, Manager, TeamLead")]

        public async Task<IActionResult> AddNotes([FromBody] NotesDto noteDto)
        {

            try
            {
                var claimsDictionary = GetClaims.GetClaimsByToken(_httpContextAccessor);

                var UserId = "id";
                // Retrieve the value of the userId claim

                if (claimsDictionary.TryGetValue("userId", out string userId))
                {
                    Console.WriteLine(userId);


                    UserId = userId;
                }

                var result = await _notesService.AddNotes(noteDto, UserId);

                if (result.Contains("Note not found") || result.Contains("Some Problem while adding add proper note data") || result == null)
                {
                    return BadRequest(result);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it accordingly
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }


        #endregion

        #region DeleteNoteById

        [HttpDelete]
        [Route("DeleteNoteById")]
        [Authorize(Roles = "Admin, Manager")]

        public async Task<IActionResult> DeleteNoteById([FromBody] NotesDto notesDto)
        {
           

            try
            {
                var claimsDictionary = GetClaims.GetClaimsByToken(_httpContextAccessor);

                var UserId = "id";
                // Retrieve the value of the userId claim

                if (claimsDictionary.TryGetValue("userId", out string userId))
                {
                    Console.WriteLine(userId);


                    UserId = userId;
                }

                var result = await _notesService.DeleteNoteById(notesDto, UserId);


                if ( result.Contains("User or Note not find") || result.Contains("Send data Properly") || result == null )
                {
                    return BadRequest(result);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it accordingly
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }

        }

        #endregion

        #region update/EditNotesById
        [HttpPut]
        [Route("UpdateNoteById")]
        [Authorize(Roles = "Admin, Manager, TeamLead")]

        public async Task<IActionResult> UpdateNoteById([FromBody] NotesDto notesDto)
        {
           
            try
            {

                var claimsDictionary = GetClaims.GetClaimsByToken(_httpContextAccessor);

                var UserId = "id";
                // Retrieve the value of the userId claim

                if (claimsDictionary.TryGetValue("userId", out string userId))
                {
                    Console.WriteLine(userId);


                    UserId = userId;
                }

                var result = await _notesService.UpdateNoteById(notesDto, UserId);


                if (result.Contains("User or Note not find") || result.Contains("Send data Properly") || result == null)
                {
                    return BadRequest(result);
                }

                return Ok(result);

               
            }
            catch (Exception ex)
            {
                // Log the exception or handle it accordingly
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }

        }

        #endregion

        #region GetNotesByUserId
        [HttpGet]
        [Route("GetNotesByUserId")]
        public async Task<IActionResult> GetNotesByUserId()
        {
            try
            {
                var claimsDictionary = GetClaims.GetClaimsByToken(_httpContextAccessor);

                var UserId = "id";
                // Retrieve the value of the userId claim

                if (claimsDictionary.TryGetValue("userId", out string userId))
                {
                    Console.WriteLine(userId);


                    UserId = userId;
                }


                var result = await _notesService.GetNotesByUserId(userId);

                if (result != null)
                {
                  

                    return Ok(result);
                }
                else
                {
                    return BadRequest(result);
                }



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
}
