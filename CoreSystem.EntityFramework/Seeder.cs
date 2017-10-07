using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreSystem.EntityFramework
{
    public class Seeder
    {
        private CoreSystemDbContext db { get; set; }

        private UserManager<CoreSystemUser> UM { get; set; }

        private RoleManager<CoreSystemRole> RM { get; set; }

        public Seeder(CoreSystemDbContext context, UserManager<CoreSystemUser> userManager,
            RoleManager<CoreSystemRole> roleManager)
        {
            db = context;
            UM = userManager;
            RM = roleManager;
        }

        public async Task CreateRolesAsync()
        {
            if (!(await RM.RoleExistsAsync("admin")))
                await RM.CreateAsync(new CoreSystemRole { Name = "admin" });

            if (!(await RM.RoleExistsAsync("member")))
                await RM.CreateAsync(new CoreSystemRole { Name = "member" });
        }

        private async Task CreateMemberUserAsync(string username, params string[] roleNames)
        {
            CoreSystemUser user = await UM.FindByNameAsync(username);

            if (user == null)
            {
                user = new CoreSystemUser
                {
                    UserName = username,
                    Email = $"{username}@coresystem.com",
                    EmailConfirmed = true
                };
                var res = await UM.CreateAsync(user, "Aaa000$");
                if (res.Succeeded)
                {
                    await UM.AddToRoleAsync(user, "member");
                }
            }
        }

        public async Task CreateUsersAsync()
        {
            // The Admin User
            CoreSystemUser admin = await UM.FindByNameAsync("admin");

            if (admin == null)
            {
                admin = new CoreSystemUser
                {
                    UserName = "admin",
                    Email = "webmaster@coresystem.net",
                    EmailConfirmed = true
                };
                var res = await UM.CreateAsync(admin, "Aaa000$");
                if (res.Succeeded)
                {
                    await UM.AddToRoleAsync(admin, "member");
                    await UM.AddToRoleAsync(admin, "admin");
                }
            }

            await CreateMemberUserAsync("user1");
            await CreateMemberUserAsync("user2");
            await CreateMemberUserAsync("user3");
            await CreateMemberUserAsync("user4");
            await CreateMemberUserAsync("user5");
        }

        public async Task InitializeDataAsync()
        {
            await CreateRolesAsync();
            await CreateUsersAsync();           
        }
    }
}
