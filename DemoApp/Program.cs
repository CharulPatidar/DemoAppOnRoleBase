using DemoApp.Controllers;
using DemoApp.DependencyInjection;
using DemoApp.Hubs;
using DemoApp.Middleware;
using DemoApp.Models;
using DemoApp.Services;
using DemoApp.ServicesImplement;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;



var builder = WebApplication.CreateBuilder(args);



// Configure the db.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection") + ";TrustServerCertificate=true" );

});

Environment.SetEnvironmentVariable("SecretKey", "677C2E5962533A1C3D449C99F2AD76168aefw");
var secretKey1 = builder.Configuration["Jwt:SecretKey"];


//The circular reference occurs when serializing an object that references itself (or forms a loop with other objects). To Preventing WE Do This

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
});


builder.Services.AddScoped<IUsersService, UserServicesImplement>();
builder.Services.AddScoped<IRolesService, RoleServicesImplement>();
builder.Services.AddScoped<IPermissionsService, PermissionServicesImplement>();
builder.Services.AddScoped<INotesService, NoteServicesImplement>();



// Add services to the container.
//builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

//jwt  with extension method 
builder.Services.AddJwtService();

//builder.Services.AddSwaggerGen();

builder.Services.AddHttpContextAccessor();

builder.Services.AddSignalR();        // ->>>>>>>>>>  Assing Signalr AS Service

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
            builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            });
});


var app = builder.Build();


// Add this in the Configure method
//app.UseAuthentication();
//app.UseAuthorization();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.UseSwagger();
    //app.UseSwaggerUI();
}

// Your custom middleware comes here
//app.UseMiddleware<CustomMiddleware>();


app.UseHttpsRedirection();


app.UseFileServer();
app.UseDefaultFiles();
app.UseStaticFiles();

app.UseRouting();

// Enable CORS middleware
//app.UseCors("AllowAll");



app.UseAuthentication();
app.UseAuthorization();


app.MapHub<NotesHub>("/NotesHub"); // Map the SignalR hub



app.MapControllers();



app.Run();
