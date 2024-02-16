using DemoApp.DTO;
using DemoApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace DemoApp.ServicesImplement
{
    public class NoteServicesImplement : INotesService
    {
        Task<IActionResult> INotesService.AddNotes(NotesDto notesDto)
        {
            throw new NotImplementedException();
        }

        Task<IActionResult> INotesService.DeleteNoteById(NotesDto notesDto)
        {
            throw new NotImplementedException();
        }

        Task<IActionResult> INotesService.GetAllNotes()
        {
            throw new NotImplementedException();
        }

        Task<IActionResult> INotesService.GetNotesByUserId()
        {
            throw new NotImplementedException();
        }

        Task<IActionResult> INotesService.UpdateNoteById(NotesDto notesDto)
        {
            throw new NotImplementedException();
        }
    }
}
