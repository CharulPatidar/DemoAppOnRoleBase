using DemoApp.Hubs;
using DemoApp.Models;
using DemoApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Security;

namespace DemoApp.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class AdminController : BaseController
    {

        public AdminController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, IHubContext<NotesHub> notesHub)
            : base(context, httpContextAccessor, notesHub)
        {
        }


        //#region GetAllUsers
        //[HttpGet]
        //[Route("GetAllUsers")]
        //public async Task<IActionResult> GetAllUsers()
        //{
        //    try
        //    {
        //        var users = _context.Users.ToList();
        //        return Ok(users);
        //    }
        //    catch (Exception ex)
        //    {
               
        //        Console.Error.WriteLine($"An error occurred while fetching users: {ex.Message}");
        //        return StatusCode(500, "Internal Server Error");
        //    }
        //}

        //#endregion


       
  
       
    }



    #region bodyClasses

    public class DtoUserRole
    {
        public string UserId { get; set; }

        public string RoleId { get; set; }

    }

    public class DtoRolePermission
    {
        public string RoleId { get; set; }
        public string PermissionId { get; set; }
    }

    #endregion
}
