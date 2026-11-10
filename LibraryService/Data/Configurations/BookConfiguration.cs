using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using LibraryService.Entities;

namespace LibraryService.Data.Configurations
{
    public class BookConfiguration : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.HasKey(b => b.Id);
            builder.Property(b=>b.Title).HasMaxLength(200).IsRequired();
            builder.Property(b => b.ISBN).HasMaxLength(13).IsRequired();
            builder.HasIndex(b => b.ISBN).IsUnique();

            builder.HasOne(b=>b.Author)
                .WithMany(a => a.Books)
                .HasForeignKey(b => b.AuthorId)
                .OnDelete(DeleteBehavior.Restrict); //ავტორი წიგნებით არ იშლება
            builder.HasMany(b=>b.Genres)
                .WithMany(g=>g.Books)
                .UsingEntity(j=>j.ToTable("BookGenres"));
        }
    }
}
