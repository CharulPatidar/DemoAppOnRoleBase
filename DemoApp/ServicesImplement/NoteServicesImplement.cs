using DemoApp.Controllers;
using DemoApp.DTO;
using DemoApp.Hubs;
using DemoApp.Models;
using DemoApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace DemoApp.ServicesImplement
{
    public class NoteServicesImplement : BaseServiceImplement , INotesService 
    {

        public NoteServicesImplement(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, IHubContext<NotesHub> notesHub)
            : base(context, httpContextAccessor, notesHub)
        {

        }


        public async Task<string> AddNotes(NotesDto notesDto, string UserId)
        {
           
                try
                {
                    if (notesDto != null && !String.IsNullOrEmpty(notesDto.NoteTopic) && !String.IsNullOrEmpty(notesDto.NoteDescription))
                    {
                        var user = _context.Users.Where(u => u.Id.ToString().Equals(UserId)).FirstOrDefault();

                        if (user == null)
                        {
                            // User not found
                            return ("User not found");
                        }

                        Notes newNotes = new Notes
                        {
                            Topic = notesDto.NoteTopic,
                            Description = notesDto.NoteDescription
                        };

                        _context.Notes.Add(newNotes);
                        _context.SaveChanges();

                        var noteId = newNotes.Id;

                        var note = _context.Notes.Where(u => u.Id.Equals(noteId)).FirstOrDefault();

                        if (note == null)
                        {
                            // Note not found
                            return ("Note not found");
                        }

                        UserNotes userNotes = new UserNotes();

                        userNotes.NoteId = noteId;
                        userNotes.UserId = user.Id;
                        userNotes.User = user;
                        userNotes.Note = note;


                        _context.UserNotes.Add(userNotes);
                        _context.SaveChanges();


                        var noteData = new
                        {
                            id = newNotes.Id.ToString(),
                            topic = newNotes.Topic,
                            description = newNotes.Description,
                            userNotes = (object)null // or set it to null if needed
                        };



                        string jsonString = JsonSerializer.Serialize(noteData);
                        await _notesHub.Clients.All.SendAsync("ReceiveMsg", jsonString);

                      // return (noteData);
                        return ($"Note {newNotes.Topic} Created By User {user.UserName} ");


                    }
                    return ("Some Problem while adding add proper note data");
                }
                catch (Exception ex)
                {
                    // Log the exception or handle it accordingly
                    // return StatusCode(500, $"Internal Server Error: {ex.Message}");
                    throw;
                }
           

        }

        public async Task<string> DeleteNoteById(NotesDto notesDto, string UserId)
        {
            try
            {

                if (notesDto != null)
                {
                    var note = _context.Notes.Where(n => n.Id.ToString().Equals(notesDto.NoteId)).FirstOrDefault();
                    var user = _context.Users.Where(u => u.Id.ToString().Equals(UserId)).FirstOrDefault();

                    if (note != null && user != null)
                    {
                        _context.Notes.Remove(note);
                        _context.SaveChanges();
                        return ($"Delete notes by {user.UserName}");
                    }

                    return ("User or Note not find");
                }

                return ("Send data Properly");
            }
            catch (Exception ex)
            {
                // Log the exception or handle it accordingly
                // return StatusCode(500, $"Internal Server Error: {ex.Message}");
                throw;
            }
        }

        public  async Task<object> GetAllNotes()
        {
            try
            {
                var allNotes = _context.Notes.ToList();

                if (allNotes != null)
                {
                    return (allNotes);
                }
                else
                {
                    // Handle the case where allNotes is null
                    return ("No notes found.");
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it accordingly
                throw;
            }
        }

        public async Task<object> GetNotesByUserId(string UserId)
        {
            try
            {
                var existingUser = await _context.Users
                         .FirstOrDefaultAsync(u => u.Id.ToString().Equals(UserId));

                if (existingUser != null)
                {
                    var userNotes = await _context.UserNotes
                                                     .Where(u => u.UserId.Equals(existingUser.Id))
                                                     .Join(_context.Notes,
                                                           userNote => userNote.NoteId,
                                                           note => note.Id,
                                                           (userNote, note) => new
                                                           {
                                                               id = note.Id,
                                                               topic = note.Topic,
                                                               description = note.Description
                                                           })
                                                     .ToListAsync();


                    // Now you have the existing user and their associated notes in the userNotes variable
                    return (userNotes);
                }

                return ("User Not found");
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<string> UpdateNoteById(NotesDto notesDto, string UserId)
        {
            if (notesDto != null)
            {
                var note = _context.Notes.Where(n => n.Id.ToString().Equals(notesDto.NoteId)).FirstOrDefault();
                var user = _context.Users.Where(u => u.Id.ToString().Equals(UserId)).FirstOrDefault();

                if (note != null && user != null)
                {

                    note.Topic = notesDto.NoteTopic;
                    note.Description = notesDto.NoteDescription;

                    _context.Notes.Update(note);
                    _context.SaveChanges();
                    return ($"update notes by {user.UserName}");
                }

                return ("User or Note not find");
            }

            return ("Send data Properly");
        }
    }
}
