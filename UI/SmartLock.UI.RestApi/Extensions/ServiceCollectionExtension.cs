using System.IdentityModel.Tokens.Jwt;
using System.Text;

using FluentValidation;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using SmartLock.Application.Configurations.Models;
using SmartLock.Application.Consts;
using SmartLock.Application.Services;
using SmartLock.Shared.Abstraction;
using SmartLock.UI.RestApi.Configuration;

namespace SmartLock.UI.RestApi.Extensions {
    public static class ServiceCollectionExtension {

        public static IServiceCollection AddSmartLockAuthentication(this IServiceCollection services, Audience audience) {
            services.AddAuthentication(opt => {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(option => {
                option.Events = new JwtBearerEvents {
                    OnTokenValidated = async (context) => {
                        IUserManagment userManagement = context.HttpContext.RequestServices.GetRequiredService<IUserManagment>();
                        JwtSecurityToken accessToken = context.SecurityToken as JwtSecurityToken;

                        if (!ulong.TryParse(accessToken.Claims.FirstOrDefault(x => x.Type == ClaimNames.ClaimNames_UserId)?.Value, out ulong userId) ||
                            !await userManagement.Authenticate(userId))
                            context.Fail("Unauthorized");
                    }
                };
                SymmetricSecurityKey signingKey = new(Encoding.ASCII.GetBytes(audience.Secret));
                option.RequireHttpsMetadata = false;
                option.SaveToken = true;
                option.RefreshOnIssuerKeyNotFound = false;
                option.TokenValidationParameters = new TokenValidationParameters {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = signingKey,
                    ValidateIssuer = false,
                    ValidIssuer = audience.Iss,
                    ValidateAudience = true,
                    ValidAudience = audience.Aud,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    RequireExpirationTime = true,
                };
            });           
            

            return services;
        }

        public static IServiceCollection AddSmartLockAuthorization(this IServiceCollection services) {
            services.AddAuthorization(opt => {
                opt.AddPolicy("SmartLockAuth", o => o.RequireRole(RoleKeys.Admin, RoleKeys.User, RoleKeys.System));
            });
            return services;
        }
    }
}
