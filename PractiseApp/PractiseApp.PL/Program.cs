using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.EntityFrameworkCore;
using PractiseApp.BLL.Identity.Constants;
using PractiseApp.DAL;

namespace PractiseApp;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllersWithViews();
        builder.Services.AddMvc();
        builder.Services.AddAuthentication(NegotiateDefaults.AuthenticationScheme)
            .AddNegotiate();
        
        // Add roles
        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy(DefaultPolicies.AdminPolicy, policybuilder =>
            {
                policybuilder.RequireAuthenticatedUser();
                policybuilder.AddAuthenticationSchemes(CookieAuthenticationDefaults.AuthenticationScheme);
                policybuilder.RequireRole(DefaultRoles.Admin);
            });
            options.AddPolicy(DefaultPolicies.ModeratorPolicy, policybuilder =>
            {
                policybuilder.RequireAuthenticatedUser();
                policybuilder.AddAuthenticationSchemes(CookieAuthenticationDefaults.AuthenticationScheme);
                policybuilder.RequireRole(DefaultRoles.Moderator);
            });
            options.AddPolicy(DefaultPolicies.UserPolicy, policybuilder =>
            {
                policybuilder.RequireAuthenticatedUser();
                policybuilder.AddAuthenticationSchemes(CookieAuthenticationDefaults.AuthenticationScheme);
                policybuilder.RequireRole(DefaultRoles.User);
            });
        });
        builder.Services.AddRazorPages();

        // Register DbContext
        builder.Services.AddDbContext<EntityContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

        var app = builder.Build();

        await app.PrepareAsync();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.Run();
    }
}