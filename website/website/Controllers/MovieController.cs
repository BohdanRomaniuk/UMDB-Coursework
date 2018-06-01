using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using database.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using website.Models;
using website.Models.Interfaces;

namespace website.Controllers
{
    public class MovieController : Controller
    {
        private UserManager<User> userManager;
        private readonly IUMDBRepository db;
        public MovieController(IUMDBRepository _db, UserManager<User> _userManager)
        {
            db = _db;
            userManager = _userManager;
        }

        public IActionResult ViewMovie(int id)
        {
            Movie current = db.Movies.Include(m => m.Countries).ThenInclude(c => c.Country)
                                     .Include(m => m.Genres).ThenInclude(g => g.Genre)
                                     .Include(m=>m.Comments).ThenInclude(c=>c.PostedBy)
                                     .Where(m => m.Id == id).FirstOrDefault();
            return View(current);
        }

        [HttpPost]
        public async Task<IActionResult> AddComment(string commentText, int movieId)
        {
            User postedBy = await userManager.FindByNameAsync(User.Identity.Name);
            db.AddComment(movieId,new Comment() { CommentText=commentText, PostedBy=postedBy, PostedDate=DateTime.Now });
            return RedirectToRoute(new { controller = "Movie", action = "ViewMovie", id = movieId });
        }
    }
}