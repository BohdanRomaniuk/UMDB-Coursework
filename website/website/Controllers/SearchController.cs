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

        public IActionResult Query(int country=0, string movieName="", int[] genres=null, int page = 1)
        {
            /*IQueryable<Movie> allMovies = db.Movies
                                          .Include(m => m.Countries).ThenInclude(c => c.Country)
                                          .Include(m => m.Genres).ThenInclude(g => g.Genre)
                                          .Where(m => m.Countries.Any(p => p.Country.Id == country))
                                          .Where(m => m.Name.Contains(movieName))
                                          .Where(m => m.Genres.Where(g=>genres.Contains(g.Genre.Id)).Count()!=0);*/
            IQueryable<Movie> allMovies = db.Movies
                                          .Include(m => m.Countries).ThenInclude(c => c.Country)
                                          .Include(m => m.Genres).ThenInclude(g => g.Genre);
            if(country!=0)
            {
                allMovies = allMovies.Where(m => m.Countries.Any(p => p.Country.Id == country));
            }
            if(movieName!="")
            {
                allMovies = allMovies.Where(m => m.Name.Contains(movieName));
            }
            if(genres!=null && genres.Length!=0)
            {
                allMovies = allMovies.Where(m => m.Genres.Where(g => genres.Contains(g.Genre.Id)).Count() != 0);
            }
            int perPage = 20;
            return View(new SearchViewModel(db.Genres, db.Countries, allMovies.Skip(page*perPage-perPage).Take(perPage), country, movieName));
        }
    }
}