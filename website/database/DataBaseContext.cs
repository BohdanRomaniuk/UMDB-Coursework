using System;
using database.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace database
{
    public class MoviesContext : IdentityDbContext<User>
    {
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<MovieGenre> MovieGenres { get; set; }
        public DbSet<MovieCountry> MovieCountries { get; set; }
        public MoviesContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"data source=(LocalDb)\MSSQLLocalDB;initial catalog=UMDB;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework");
        }
    }

    class DataBaseContext
    {
        static void Main(string[] args)
        {
            using (var db = new MoviesContext())
            {
                var result = from movie in db.Movies.Include(m => m.Genres).Include(m => m.Countries)
                             select movie;
                foreach (Movie elem in result)
                {
                    Console.WriteLine("{0} ({1})", elem.Name, elem.Year);
                    Console.Write("Жанри: ");
                    foreach (var genre in elem.Genres)
                    {
                        Console.Write("{0} ", genre.Genre.Name);
                    }
                    Console.Write("\n");
                    Console.Write("Країна: ");
                    foreach (var genre in elem.Countries)
                    {
                        Console.Write("{0} ", genre.Country.Name);
                    }
                    Console.Write("\n");
                }
                Console.ReadKey();
            }
        }
    }
}
