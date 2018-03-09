using System;
using System.Linq;
using System.Data.Entity;
using database.Models;

namespace database
{
    public class MoviesContext : DbContext
    {
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<MovieGenre> MovieGenres { get; set; }
        public DbSet<MovieCountry> MovieCountries { get; set; }
        public MoviesContext() :
            base("UMDB-1")
        {
        }
    }

    public class DbDataContext
    {
        static void Main(string[] args)
        {
            using (var db = new MoviesContext())
            {
                var result = from movie in db.Movies.Include("Genres").Include("Genres.Genre").Include("Countries").Include("Countries.Country")
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
