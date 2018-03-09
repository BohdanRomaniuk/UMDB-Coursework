using System;
using System.Collections.Generic;
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
        public MoviesContext():
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
                db.SaveChanges();
                Console.WriteLine("Database succesfully created!!!");
                Console.ReadKey();
            }
        }
    }
}
