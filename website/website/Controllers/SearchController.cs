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

        public IActionResult Query(int country=1, string movieName="", int page = 1)
        {
            IQueryable<Movie> allMovies = db.Movies
                                          .Include(m => m.Countries).ThenInclude(c => c.Country)
                                          .Include(m => m.Genres).ThenInclude(g => g.Genre)
                                          .Where(c => c.Countries.Any(p => p.Country.Id==country))
                                          .Where(c=>c.Name.Contains(movieName));
            int perPage = 20;
            return View(new SearchViewModel(db.Genres, db.Countries, allMovies.Skip(page*perPage-perPage).Take(perPage), country, movieName));
        }
    }
}