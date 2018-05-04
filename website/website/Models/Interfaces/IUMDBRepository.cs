using database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace website.Models.Interfaces
{
    public interface IUMDBRepository
    {
        IQueryable<Movie> Movies { get; }
        IQueryable<Genre> Genres { get; }
        IQueryable<Country> Countries { get; }
        IQueryable<MovieGenre> MovieGenres { get; }
        IQueryable<MovieCountry> MovieCountries { get; }
        IQueryable<User> Users { get; }
        IQueryable<Comment> Comments { get; }
    }
}
