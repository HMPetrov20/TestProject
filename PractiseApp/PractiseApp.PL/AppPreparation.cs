using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PractiseApp.BLL.Identity.Constants;
using PractiseApp.DAL;

namespace PractiseApp;

public static class AppPreparation
{
    public static async Task PrepareAsync(this IApplicationBuilder app)
    {
        try
        {
            using var scope = app.ApplicationServices.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<EntityContext>();
            
            await dbContext.Database.MigrateAsync();

            if (!await dbContext.Roles.AnyAsync())
            {
                using var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
                foreach (var role in DefaultRoles.List)
                {
                    await roleManager.CreateAsync(new ApplicationRole { Name = role });
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}