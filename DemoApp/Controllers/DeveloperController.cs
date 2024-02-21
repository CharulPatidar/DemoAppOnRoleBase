using DemoApp.Hubs;
using DemoApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

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
