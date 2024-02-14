using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DemoApp.Models;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.SwaggerGen;

public class JwtTokenGenerator
{
    public static string createToken(User userInfo, List<string> roleslist,string key)
    {
        Console.WriteLine(roleslist);
        var claims = new List<Claim> {
                            new Claim(JwtRegisteredClaimNames.Sub, Guid.NewGuid().ToString()),
                            new Claim(JwtRegisteredClaimNames.Jti, userInfo.Id.ToString()),
                            new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
                            new Claim("userId", userInfo.Id.ToString()),
                            new Claim("userName", userInfo.UserName),
                            new Claim("email", userInfo.UserEmail),
                            };
        foreach (var roles in roleslist)
        {
            claims.Add(new Claim(ClaimTypes.Role, roles));
        }

            
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
                                          issuer : "https://localhost:7128",
                                          audience: "*",
                                          claims: claims,
                                          expires: DateTime.UtcNow.AddHours(1),
                                          signingCredentials: credentials
                                          
                                         );

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        return tokenString;
    }




    public static string GenerateJwtToken(string username, string userrole,string userid, string secretKey)
    {
        var tokenHandler = new JwtSecurityTokenHandler(); // creating handler intance


        var key = Encoding.UTF8.GetBytes(secretKey); // Replace with your secret key

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, username), // Add claims as needed
                new Claim(ClaimTypes.Role, userrole),
                new Claim(ClaimTypes.NameIdentifier, userid)


              //   Add other claims...
            }),

            Expires = DateTime.UtcNow.AddHours(1), // Set token expiration time
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        var tokenString = tokenHandler.WriteToken(token);

        return tokenString;
    }

    public static bool VerifyJwtToken(string token, string secretKey, HttpContext context)
    {
        var key = Encoding.UTF8.GetBytes(secretKey); // Replace with your secret key

        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();

           var validationParameter = new TokenValidationParameters
                                    {
                                        ValidateIssuerSigningKey = true,
                                        IssuerSigningKey = new SymmetricSecurityKey(key),
                                        ValidateIssuer = false, // Set to true if you have an issuer to validate
                                        ValidateAudience = false, // Set to true if you have an audience to validate
                                        ClockSkew = TimeSpan.FromMinutes(5), // Set the clock skew to zero for better accuracy
                                        ValidateLifetime = true,
                                    };

            var principle = tokenHandler.ValidateToken(token, validationParameter, out SecurityToken securityToken);
            context.User = principle;

            return true;
        }
        catch
        {
            return false;
        }


    }
}

