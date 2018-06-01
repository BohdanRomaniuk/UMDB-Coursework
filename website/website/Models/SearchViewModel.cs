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
        public int[] SelectedGenres { get; set; }
        public int Country { get; set; }
        public string OrderBy { get; set; }
        public int YearFrom { get; set; }
        public int YearTo { get; set; }
        public string MovieName { get; set; }
        public PagingInfo Pagination { get; set; }
        public int Count { get; set; }

        public SearchViewModel(IQueryable<Genre> _genres, IQueryable<Country> _countries, int _count, IQueryable<Movie> _movies, int[] _selectedGenres, int _country, string _orderBy, int _yearFrom, int _yearTo,  string _movieName, PagingInfo _pagination)
        {
            Genres = _genres;
            Countries = _countries;
            Movies = _movies;
            SelectedGenres = _selectedGenres;
            Country = _country;
            OrderBy = _orderBy;
            YearFrom = _yearFrom;
            YearTo = _yearTo;
            MovieName = _movieName;
            Pagination = _pagination;
            Count = _count;
        }
    }
}
