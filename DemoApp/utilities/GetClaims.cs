namespace DemoApp.utilities
{
    public class GetClaims
    {


        GetClaims() { }

        public static Dictionary<string,string> GetClaimsByToken(IHttpContextAccessor _httpContextAccessor)
        {
            var httpContext = _httpContextAccessor.HttpContext;

            var token = httpContext.Request.Headers["Authorization"].FirstOrDefault()?.Replace("Bearer ", "");
            var key = Environment.GetEnvironmentVariable("SecretKey");

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
}
