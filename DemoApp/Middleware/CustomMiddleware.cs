using System.IdentityModel.Tokens.Jwt;

namespace DemoApp.Middleware
{
    public class CustomMiddleware
    {
        private readonly RequestDelegate _next;

        private readonly HashSet<string> excludedPaths = new HashSet<string> { "/api/User/Login", "/api/User/Register", "/index.html","/js", "/html" ,"/css", "/image" };

        public CustomMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Check if the current path is in the excluded paths
            if (excludedPaths.Contains(context.Request.Path)  )
            {
                // If the path is excluded, just pass the request to the next middleware
                await _next(context);
                return;
            }

            // Your middleware logic goes here
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Replace("Bearer ", "");
            var key = Environment.GetEnvironmentVariable("SecretKey");

            //if (key == null || token == null) { return; }

            // Your middleware logic goes here
            if (!JwtTokenGenerator.VerifyJwtToken(token, key, context))
            {
                // Token verification failed
                context.Response.StatusCode = 401; // Unauthorized
                return;
            }

            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token) as JwtSecurityToken;

            // Log or inspect claims here
            foreach (var claim in jsonToken?.Claims)
            {
                Console.WriteLine($"{claim.Type}: {claim.Value}");
            }



            await _next(context); // Call the next middleware in the pipeline
        }
    }

}
