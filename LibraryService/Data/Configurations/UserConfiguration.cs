using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using LibraryService.Entities;
namespace LibraryService.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
      public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);
            builder.Property(u=>u.FullName).HasMaxLength(100).IsRequired();
            builder.Property(u => u.Email).HasMaxLength(256).IsRequired();
            builder.HasIndex(u => u.Email).IsUnique();
            builder.Property(u => u.PasswordHash).IsRequired().HasMaxLength(100);
            builder.Property(u => u.Role).HasConversion<string>().HasMaxLength(20).IsRequired();
        }
    }
}
