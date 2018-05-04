using System.ComponentModel.DataAnnotations;

namespace database.Models
{
    public class MovieCountry
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int MovieId { get; set; }
        [Required]
        public Country Country { get; set; }
    }
}
