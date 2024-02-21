using Microsoft.AspNetCore.Mvc;
using DemoApp.DTO;


namespace DemoApp.Services
{
    public interface INotesService 
    {

        public Task<object> GetAllNotes();

        public Task<object> GetNotesByUserId(string UserId);

        public Task<string> AddNotes(NotesDto notesDto, string UserId);

        public Task<string> UpdateNoteById(NotesDto notesDto, string UserId);

        public Task<string> DeleteNoteById(NotesDto notesDto, string UserId);
    }
}
