using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Security.Claims;
using WEB_153504_Klimkovich.IdentityServer.Data;
using WEB_153504_Klimkovich.IdentityServer.Models;

namespace WEB_153504_Klimkovich.IdentityServer
{
    public class SeedData
    {
        public static void EnsureSeedData(WebApplication app)
        {
            using (var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
                context.Database.Migrate();

                #region create role

                var roleMgr = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var role = roleMgr.FindByNameAsync("user").Result;
                if (role == null)
                {
                    role = new IdentityRole
                    {
                        Name = "user",
                        NormalizedName = "user",
                    };
                    var result = roleMgr.CreateAsync(role).Result;
                    if (!result.Succeeded)
                    {
                        throw new Exception(result.Errors.First().Description);
                    }
                    Log.Debug("role user created");
                }
                else
                {
                    Log.Debug("role user already exists");
                }

                role = roleMgr.FindByNameAsync("admin").Result;
                if (role == null)
                {
                    role = new IdentityRole
                    {
                        Name = "admin",
                        NormalizedName = "admin",
                    };
                    var result = roleMgr.CreateAsync(role).Result;
                    if (!result.Succeeded)
                    {
                        throw new Exception(result.Errors.First().Description);
                    }
                    Log.Debug("role admin created");
                }
                else
                {
                    Log.Debug("role admin already exists");
                }

                #endregion

                #region create users

                var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var user = userMgr.FindByNameAsync("user").Result;
                if (user == null)
                {
                    user = new ApplicationUser
                    {
                        UserName = "user",
                        Email = "user@gmail.com",
                        EmailConfirmed = true,
                    };
                    var result = userMgr.CreateAsync(user, "User1234_").Result;
                    if (!result.Succeeded)
                    {
                        throw new Exception(result.Errors.First().Description);
                    }

                    result = userMgr.AddToRoleAsync(user, "user").Result;
                    if (!result.Succeeded)
                    {
                        throw new Exception(result.Errors.First().Description);
                    }

                    result = userMgr.AddClaimsAsync(user, new Claim[]{
                            new Claim(JwtClaimTypes.Name, "Fake_user"),
                            new Claim(JwtClaimTypes.GivenName, "User"),
                            new Claim(JwtClaimTypes.FamilyName, "No"),
                            new Claim(JwtClaimTypes.WebSite, "http://user.com"),
                        }).Result;
                    if (!result.Succeeded)
                    {
                        throw new Exception(result.Errors.First().Description);
                    }
                    Log.Debug("user created");
                }
                else
                {
                    Log.Debug("user already exists");
                }

                user = userMgr.FindByNameAsync("admin").Result;
                if (user == null)
                {
                    user = new ApplicationUser
                    {
                        UserName = "admin",
                        Email = "admin@gmail.com",
                        EmailConfirmed = true,
                    };
                    var result = userMgr.CreateAsync(user, "Admin1234_").Result;
                    if (!result.Succeeded)
                    {
                        throw new Exception(result.Errors.First().Description);
                    }

                    result = userMgr.AddToRoleAsync(user, "admin").Result;
                    if (!result.Succeeded)
                    {
                        throw new Exception(result.Errors.First().Description);
                    }

                    result = userMgr.AddClaimsAsync(user, new Claim[]{
                            new Claim(JwtClaimTypes.Name, "Fake_admin"),
                            new Claim(JwtClaimTypes.GivenName, "Admin"),
                            new Claim(JwtClaimTypes.FamilyName, "No"),
                            new Claim(JwtClaimTypes.WebSite, "http://admin.com"),
                        }).Result;
                    if (!result.Succeeded)
                    {
                        throw new Exception(result.Errors.First().Description);
                    }
                    Log.Debug("admin created");
                }
                else
                {
                    Log.Debug("admin already exists");
                }

                #endregion
            }
        }
    }
}