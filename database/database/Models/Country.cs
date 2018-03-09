using System.ComponentModel.DataAnnotations;

namespace database.Models
{
    public class Country
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int Name { get; set; }
    }
}
