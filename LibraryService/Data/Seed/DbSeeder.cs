using LibraryService.Entities;
using LibraryService.Enums;
using Microsoft.EntityFrameworkCore;

namespace LibraryService.Data.Seed
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(AppDbContext db)
        {
            // თითო ცხრილი ცალკე მოწმდება — თუ უკვე აქვს მონაცემები, აღარ ვამატებთ.
            // ასე seeder ყოველ გაშვებაზე უსაფრთხოდ ეშვება და დუბლიკატებს არ ქმნის.

            // 1. ჟანრები
            if (!await db.Genres.AnyAsync())
            {
                db.Genres.AddRange(
                    new Genre { Name = "Fiction" },
                    new Genre { Name = "Poetry" },
                    new Genre { Name = "Classic" },
                    new Genre { Name = "Dystopian" },
                    new Genre { Name = "History" },
                    new Genre { Name = "Science Fiction" });
                await db.SaveChangesAsync();
            }

            // 2. ავტორები
            if (!await db.Authors.AnyAsync())
            {
                db.Authors.AddRange(
                    new Author { FirstName = "Ilia", LastName = "Chavchavadze", BirthYear = 1837, Country = "Georgia" },
                    new Author { FirstName = "Vazha", LastName = "Pshavela", BirthYear = 1861, Country = "Georgia" },
                    new Author { FirstName = "George", LastName = "Orwell", BirthYear = 1903, Country = "United Kingdom" });
                await db.SaveChangesAsync();
            }

            // 3. წიგნები — ავტორებს და ჟანრებს სახელით ვპოულობთ, Id-ებს არ ვაჰარდკოდებთ
            if (!await db.Books.AnyAsync())
            {
                var genres = await db.Genres.ToDictionaryAsync(g => g.Name);
                var ilia = await db.Authors.FirstAsync(a => a.LastName == "Chavchavadze");
                var vazha = await db.Authors.FirstAsync(a => a.LastName == "Pshavela");
                var orwell = await db.Authors.FirstAsync(a => a.LastName == "Orwell");

                db.Books.AddRange(
                    NewBook("Otaraant Kvrivi", "9780000000017", 1888, 3, ilia,
                        genres["Fiction"], genres["Classic"]),
                    NewBook("Aluda Ketelauri", "9780000000024", 1888, 2, vazha,
                        genres["Poetry"], genres["Classic"]),
                    NewBook("Nineteen Eighty-Four", "9780000000031", 1949, 5, orwell,
                        genres["Fiction"], genres["Dystopian"], genres["Science Fiction"]),
                    NewBook("Animal Farm", "9780000000048", 1945, 1, orwell,
                        genres["Fiction"], genres["Classic"]));
                await db.SaveChangesAsync();
            }

            // 4. მომხმარებლები — პაროლები მხოლოდ ჰეშად
            if (!await db.Users.AnyAsync())
            {
                db.Users.AddRange(
                    new User
                    {
                        Email = "librarian@library.ge",
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword("Librarian1"),
                        FullName = "Main Librarian",
                        Role = Role.Librarian,
                        CreatedAt = DateTime.UtcNow
                    },
                    new User
                    {
                        Email = "reader@library.ge",
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword("Reader123"),
                        FullName = "Demo Reader",
                        Role = Role.Reader,
                        CreatedAt = DateTime.UtcNow
                    });
                await db.SaveChangesAsync();
            }
        }

        private static Book NewBook(string title, string isbn, int year, int copies, Author author, params Genre[] genres)
        {
            var book = new Book
            {
                Title = title,
                ISBN = isbn,
                PublishedYear = year,
                TotalCopies = copies,
                AvailableCopies = copies,
                Author = author
            };
            foreach (var genre in genres)
                book.Genres.Add(genre);
            return book;
        }
    }
}