using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PractiseApp.DAL.Models;

public class TestModel
{
    [Required]
    public Guid Id { get; set; }
    
    [Required]
    public string FirstName { get; set; } = string.Empty;
    
    [Required]
    public string LastName { get; set; } = string.Empty;
    
    [Required]
    public string Email { get; set; } = string.Empty;
    
    [Required]
    public int Age { get; set; }
    
    public int TestNumber { get; set; }
    
    public double TestDouble { get; set; }
    public string AddingInfo { get; set; } = string.Empty;
    
    [Required]
    public bool IsDeleted { get; set; } = false;

    internal class Configuration : IEntityTypeConfiguration<TestModel>
    {
        public void Configure(EntityTypeBuilder<TestModel> builder)
        {
            builder.Property(x => x.FirstName).HasMaxLength(100);
            builder.Property(x => x.LastName).HasMaxLength(100);
            builder.Property(x => x.Email).HasMaxLength(200);
            builder.Property(x => x.AddingInfo).HasMaxLength(500);
        }
    }
}