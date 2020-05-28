using Microsoft.AspNetCore.Identity;
using NeonMarket.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NeonMarket
{
    public class DatabaseInitializer
    {

        public static async System.Threading.Tasks.Task InitializeAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {

            string adminPhone = "0";
            string password = "qwertyasdfgh";
            string email = "admin@admin.com";

            // role creating
            if (await roleManager.FindByNameAsync(Constants.ROLE_ADMIN) == null)
            {
                await roleManager.CreateAsync(new IdentityRole(Constants.ROLE_ADMIN));
            }
            if (await roleManager.FindByNameAsync(Constants.ROLE_CUSTOMER) == null)
            {
                await roleManager.CreateAsync(new IdentityRole(Constants.ROLE_CUSTOMER));
            }
            if (await roleManager.FindByNameAsync(Constants.ROLE_MODERATOR) == null)
            {
                await roleManager.CreateAsync(new IdentityRole(Constants.ROLE_MODERATOR));
            }

            // connect admin role with admin user
            if (await userManager.FindByNameAsync(adminPhone) == null)
            {

                // create admin user
                User admin = new User { PhoneNumber = adminPhone, UserName = adminPhone, Email = email };
                IdentityResult result = await userManager.CreateAsync(admin, password);

                // connect to this user admin role
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, Constants.ROLE_ADMIN);
                }
            }

        }
    }
}
