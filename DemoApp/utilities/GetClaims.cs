

namespace DemoApp.utilities
{
    public class GetClaims
    {
      


        public GetClaims() 
        {
            

        }

        public static Dictionary<string,string> GetClaimsByToken(IHttpContextAccessor _httpContextAccessor)
        {
            try
            {

                var httpContext = _httpContextAccessor.HttpContext;

                if (httpContext == null)
                {
                    return null;
                }
                else
                {
                    // var token = httpContext.Request.Headers["Authorization"].FirstOrDefault()?.Replace("Bearer ", "") ?? "";
                    var token = httpContext.Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "") ?? "";

                    var key = Environment.GetEnvironmentVariable("SecretKey") ?? "";
                  //  var key = _configuration["Jwt:SecretKey"];

                    JwtTokenGenerator.VerifyJwtToken(token, key, httpContext); //using this we set claims in  httpContext

                    // Retrieve all claims from the HttpContext
                    var allClaims = httpContext.User.Claims.ToList();

                    // Create a dictionary to store claim types and values
                    var claimsDictionary = new Dictionary<string, string>();

                    // Populate the claims dictionary
                    foreach (var claim in allClaims)
                    {
                        claimsDictionary.Add(claim.Type, claim.Value);
                    }

                    return claimsDictionary;

                }
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.ToString());
                throw;
            }
            
            
            
            
        }
    }
}
