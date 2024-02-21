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
