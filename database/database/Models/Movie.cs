using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace database.Models
{
    public class Movie
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int Year { get; set; }
        [Required]
        public string Poster { get; set; }
        [Required]
        public string Length { get; set; }
        [Required]
        public string ImdbLink { get; set; }
        [Required]
        public string Companies { get; set; }
        [Required]
        public string Director { get; set; }
        [Required]
        public string Actors { get; set; }
        [Required]
        public string Story { get; set; }

        //SpecRegion
        public List<Genre> Genres { get; set; }
        public List<Country> Countries { get; set; }
    }
}
