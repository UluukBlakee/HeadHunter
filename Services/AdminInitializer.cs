using HeadHunter.Models;
using Microsoft.AspNetCore.Identity;

namespace HeadHunter.Services
{
    public class AdminInitializer
    {
        public static async Task AddRoles(RoleManager<IdentityRole<int>> roleManager)
        {
            var roles = new[] { "applicant", "employer" };
            foreach (var r in roles)
            {
                if (await roleManager.FindByNameAsync(r) is null)
                    await roleManager.CreateAsync(new IdentityRole<int>(r));
            }
        }
    }
}
