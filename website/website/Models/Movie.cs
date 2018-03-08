using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace website.Models
{
    [Table("Movies")]
    public class Movie
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Year { get; set; }
    }
    //public string Length { get; set; }
    //public string Imdb { get; set; }
    //public string Companies { get; set; }
    //public string Director { get; set; }
    //public string Actors { get; set; }
    //public string Story { get; set; }
    //public string Poster { get; set; }
}