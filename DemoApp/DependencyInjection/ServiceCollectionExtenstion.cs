﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace DemoApp.DependencyInjection
{
    public static class ServiceCollectionExtenstion
    {
        public static void AddJwtService(this IServiceCollection services,string secretKey)
        {

           //var key = Environment.GetEnvironmentVariable("SecretKey");
            var key = secretKey;
            if (key == null) { return; }


            services.AddAuthentication(options =>
{
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = false,
                        ValidateIssuerSigningKey = true,
                        ClockSkew = TimeSpan.Zero,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
                    };
                });

        }
    }
}
