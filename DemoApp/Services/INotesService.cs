using Microsoft.AspNetCore.Mvc;
using DemoApp.DTO;


namespace DemoApp.Services
{
    public interface INotesService 
    {

        public Task<IActionResult> GetAllNotes();

        public Task<IActionResult> GetNotesByUserId();

        public Task<IActionResult> AddNotes([FromBody] NotesDto notesDto);

        public Task<IActionResult> UpdateNoteById([FromBody] NotesDto notesDto);

        public Task<IActionResult> DeleteNoteById([FromBody] NotesDto notesDto);
    }
}
