using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DemoApp.Models;
using Microsoft.AspNetCore.SignalR;
using DemoApp.Hubs;
using Microsoft.AspNetCore.Cors;

namespace DemoApp.Controllers
{

    //[EnableCors("AllowAll")] // Enable CORS for this /*controller*/

    public class BaseController : ControllerBase
    {

        protected readonly ApplicationDbContext _context;
        protected readonly IHttpContextAccessor _httpContextAccessor;
        protected readonly IHubContext<NotesHub> _notesHub;


        public BaseController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, IHubContext<NotesHub> notesHub)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _notesHub = notesHub;


        }


    }
}
