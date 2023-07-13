using DataAccessEF.Services.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebAPI.Extensions
{
    public static class ServicesExtenstions
    {
        public static void ConfigureCors(this IServiceCollection services, IConfiguration configuration) =>
    services.AddCors(options =>
    {
        var originsConfig = configuration.GetSection("AllowedCors").Value;
        var orginsArr = originsConfig?.Split(',', StringSplitOptions.RemoveEmptyEntries);

        if (originsConfig != "*" && (orginsArr?.Any() ?? false))
        {
            options.AddPolicy("AllowSpecificOrigin",
                builder => builder.AllowAnyHeader().AllowAnyMethod().AllowCredentials().WithOrigins(orginsArr));
        }
        else
        {
            options.AddPolicy("AllowSpecificOrigin",
                builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
        }
    });
       
        public static void ConfigureAuthentication(this IServiceCollection services, AuthConfiguration authConfig) =>
          services.AddAuthentication(options =>
          {
              options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
              options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
              options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
          })
          .AddJwtBearer(options =>
          {
              options.SaveToken = true;
              options.RequireHttpsMetadata = false;
              options.TokenValidationParameters = new TokenValidationParameters()
              {
                  ValidateIssuer = true,
                  ValidateAudience = true,
                  ValidateLifetime = true,
                  ValidateIssuerSigningKey = true,

                  ValidAudience = authConfig.JwtValidAudience,
                  ValidIssuer = authConfig.JwtValidIssuer,
                  IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authConfig.JwtSecretKey)),
                  TokenDecryptionKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authConfig.JwtEncryptionKey))
              };
          });
        public static void ConfigureSwaggerGen(this IServiceCollection services) =>
    services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "API.PortalApp", Version = "v1" });
                #region JWT Token
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer"
        });

        c.AddSecurityRequirement(new OpenApiSecurityRequirement()
          {
                    {
                      new OpenApiSecurityScheme
                      {
                        Reference = new OpenApiReference
                          {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                          },
                          Scheme = "oauth2",
                          Name = "Bearer",
                          In = ParameterLocation.Header,

                        },
                        new List<string>()
                      }
            });

                #endregion
    });

    }
}
