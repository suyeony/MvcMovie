using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MvcMovie.Data;
using System;
using System.Linq;

namespace MvcMovie.Models
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new MvcMovieContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<MvcMovieContext>>()))
            {
                // Look for any movies.
                if (context.Movie.Any())
                {
                    return;   // DB has been seeded
                }

                context.Movie.AddRange(
                    new Movie
                    {
                        Title = "When Harry Met Sally",
                        ReleaseDate = DateTime.Parse("1989-2-12"),
                        GenreId = 1,
                        Price = 7.99M,
                        ImageUrl = "https://www.imdb.com/title/tt0098635/mediaviewer/rm1579924224",
                        Rating = "R"
                    },

                    new Movie
                    {
                        Title = "Ghostbusters",
                        ReleaseDate = DateTime.Parse("1984-3-13"),
                        GenreId = 2,
                        Price = 8.99M,
                        ImageUrl = "https://www.imdb.com/title/tt0087332/mediaviewer/rm1280169216",
                    },

                    new Movie
                    {
                        Title = "Ghostbusters 2",
                        ReleaseDate = DateTime.Parse("1986-2-23"),
                        GenreId = 2,
                        Price = 9.99M,
                        ImageUrl = "https://www.imdb.com/title/tt0097428/mediaviewer/rm4131968000",
                    },

                    new Movie
                    {
                        Title = "Rio Bravo",
                        ReleaseDate = DateTime.Parse("1959-4-15"),
                        GenreId = 3,
                        Price = 3.99M,
                        ImageUrl = "https://www.imdb.com/title/tt0053221/mediaviewer/rm2309951232",
                    }
                );
                context.SaveChanges();
            }
        }
    }
}