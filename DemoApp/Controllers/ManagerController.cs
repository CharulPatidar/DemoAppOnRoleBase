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

     

    }

    #region BodyClasses
    public class DtoUserNotesDelete
    {
        public string UserId { get; set; }

        public string NoteId { get; set; }

    }
    #endregion
}
