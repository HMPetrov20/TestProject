using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PractiseApp.DAL.Models;

namespace PractiseApp.DAL;

public class EntityContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
{
    public EntityContext(DbContextOptions<EntityContext> options)
        : base(options)
    {
    }
        
    public DbSet<TestModel> TestModels { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfiguration(new TestModel.Configuration());
    }
        
}