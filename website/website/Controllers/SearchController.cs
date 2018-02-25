using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using website.Models;

namespace website.Controllers
{
    public class SearchController : Controller
    {
        [HttpGet]
        public ActionResult Query(int page=1)
        {
            MoviesContext mc = new MoviesContext();
            var selected =
                from movie in mc.Movies
                select movie;
            int perPage = 5;
            int skip = page * perPage - perPage;
            return View( selected.OrderBy(s => s.Id).Skip(skip).Take(perPage));
        }
    }
}