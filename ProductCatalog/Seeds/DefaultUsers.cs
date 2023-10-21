using Microsoft.AspNetCore.Identity;
using ProductCatalog.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;

namespace ProductCatalog.Seeds
{
    public static class DefaultUsers
    {
        public static async Task SeedBasicUserAsync(UserManager<IdentityUser> userManager)
        {
            var defaultUser = new IdentityUser
            {
                UserName = "user@abc.com",
                Email = "user@abc.com"
            };

            var user = await userManager.FindByEmailAsync(defaultUser.Email);

            if (user == null)
            {
                await userManager.CreateAsync(defaultUser, "Abc@123");
                await userManager.AddToRoleAsync(defaultUser, Roles.Basic.ToString());
            }
        }

        public static async Task SeedAdminUserAsync(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManger)
        {
            var defaultUser = new IdentityUser
            {
                UserName = "admin@abc.com",
                Email = "admin@abc.com",
            };

            var user = await userManager.FindByEmailAsync(defaultUser.Email);

            if (user == null)
            {
                await userManager.CreateAsync(defaultUser, "Abc@123");
                await userManager.AddToRolesAsync(defaultUser, new List<string> { Roles.Basic.ToString(), Roles.Admin.ToString() });
            }
        }
    }
}
