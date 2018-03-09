using System.ComponentModel.DataAnnotations;

namespace database.Models
{
    public class MovieGenre
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int MovieId { get; set; }
        [Required]
        public Genre Genre { get; set; }
    }
}
