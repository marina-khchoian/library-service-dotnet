using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using LibraryService.Entities;
namespace LibraryService.Data.Configurations
{
    public class LibraryCardConfiguration : IEntityTypeConfiguration<LibraryCard>
    {
        public void Configure(EntityTypeBuilder<LibraryCard> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.CardNumber).HasMaxLength(20).IsRequired();
            builder.HasIndex(c => c.CardNumber).IsUnique();
            builder.HasOne(c => c.User)
                .WithOne(u => u.LibraryCard)
                .HasForeignKey<LibraryCard>(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(c=>c.UserId).IsUnique();

        }
    }
}
