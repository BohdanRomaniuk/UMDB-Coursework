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
        private readonly UMDBContext db1;
        public SearchController(IUMDBRepository _db, UMDBContext _db1)
        {
            db = _db;
            db1 = _db1;
        }

        public IActionResult Query(int page=1)
        {
            IQueryable<Movie> allMovies = from movie in db.Movies.Include(m=>m.Countries).ThenInclude(c=>c.Country).Include(m=>m.Genres).ThenInclude(g=>g.Genre)
                         select movie;
            int perPage = 20;
            return View(allMovies.Skip(page*perPage-perPage).Take(perPage));
        }
    }
}