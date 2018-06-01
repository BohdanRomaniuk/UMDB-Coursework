using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace database.Models
{
    public class Movie
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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

        
        public List<MovieGenre> Genres { get; set; }
        public List<MovieCountry> Countries { get; set; }

        public List<Comment> Comments { get; set; }
    }
}
