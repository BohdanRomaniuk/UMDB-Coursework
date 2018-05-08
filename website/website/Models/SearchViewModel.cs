using database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace website.Models
{
    public class SearchViewModel
    {
        public IQueryable<Genre> Genres { get; set; }
        public IQueryable<Country> Countries { get; set; }
        public IQueryable<Movie> Movies { get; set; }
        public int Country { get; set; }
        public string MovieName { get; set; }

        public SearchViewModel(IQueryable<Genre> _genres, IQueryable<Country> _countries, IQueryable<Movie> _movies, int _country, string _movieName)
        {
            Genres = _genres;
            Countries = _countries;
            Movies = _movies;
            Country = _country;
            MovieName = _movieName;
        }
    }
}
