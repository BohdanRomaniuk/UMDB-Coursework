using database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace website.Models
{
    public class MovieViewModel
    {
        public Movie Movie { get; set; }
        public IQueryable<Comment> Comments { get; set; }

        public MovieViewModel(Movie _movie, IQueryable<Comment> _comments)
        {
            Movie = _movie;
            Comments = _comments;
        }
    }
}
