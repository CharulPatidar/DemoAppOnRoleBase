using DemoApp.Hubs;
using DemoApp.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace DemoApp.ServicesImplement
{
    public class BaseServiceImplement
    {

        protected readonly ApplicationDbContext _context;
        protected readonly IHttpContextAccessor _httpContextAccessor;
        protected readonly IHubContext<NotesHub> _notesHub;

        public BaseServiceImplement(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, IHubContext<NotesHub> notesHub)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _notesHub = notesHub;


        }
    }
}
