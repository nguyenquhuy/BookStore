using BTLWEB.Models;
using Microsoft.AspNetCore.Identity;

namespace BTLWEB.ViewModel
{
    public class UserRoleInitializer
    {
        public static async Task InitializaeAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<DefaultUser>>();
            string[] roleNames = { "Admin", "User" };
            IdentityResult roleResult;

            foreach(var role in roleNames)
            {
                var roleExists = await roleManager.RoleExistsAsync(role);
                if(!roleExists)
                {
                    roleResult = await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            var email = "admin@site.com";
            var password = "Huy@123";
            if(userManager.FindByEmailAsync(email).Result == null) {
                DefaultUser user = new()
                {
                    Email = email,
                    UserName = email,
                    Name = "Huy Nguyen",
                    Adress = "Ha Noi"
                };
                IdentityResult result = userManager.CreateAsync(user, password).Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Admin").Wait();
                }
            }
        }
    }
}
