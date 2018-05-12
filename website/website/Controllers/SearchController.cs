using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using database.Models;
using website.Models.Interfaces;
using Microsoft.EntityFrameworkCore;
using website.Models;

namespace website.Controllers
{
    public class SearchController : Controller
    {
        private readonly IUMDBRepository db;
        public SearchController(IUMDBRepository _db)
        {
            db = _db;
        }

        public IActionResult Query(int country=0, int[] genres = null, string orderBy="year-desc", int yearFrom=1920, int yearTo=2018, string movieName="", int page = 1)
        {
            IQueryable<Movie> allMovies = db.Movies
                                          .Include(m => m.Countries).ThenInclude(c => c.Country)
                                          .Include(m => m.Genres).ThenInclude(g => g.Genre);
            int perPage = 20;
            if (country!=0)
            {
                allMovies = allMovies.Where(m => m.Countries.Any(p => p.Country.Id == country));
            }
            if(movieName!=null && movieName!="")
            {
                allMovies = allMovies.Where(m => m.Name.Contains(movieName));
            }
            if(genres!=null && genres.Length!=0)
            {
                allMovies = allMovies.Where(m => m.Genres.Where(g => genres.Contains(g.Genre.Id)).Count() != 0);
            }
            if(orderBy == "year-desc")
            {
                allMovies = allMovies.OrderByDescending(m => m.Year);
            }
            else if(orderBy == "year-asc")
            {
                allMovies = allMovies.OrderBy(m => m.Year);
            }
            if(yearFrom>1920)
            {
                allMovies = allMovies.Where(m => m.Year >= yearFrom);
            }
            if(yearTo<2018)
            {
                allMovies = allMovies.Where(m => m.Year <= yearTo);
            }
            return View(new SearchViewModel(db.Genres, 
                                            db.Countries,
                                            allMovies.Skip(page*perPage-perPage).Take(perPage),
                                            genres,
                                            country,
                                            orderBy,
                                            yearFrom,
                                            yearTo,
                                            movieName,
                                            new PagingInfo
                                            {
                                                CurrentPage = page,
                                                ItemsPerPage = perPage,
                                                TotalItems = allMovies.Count()
                                            }));
        }
    }
}