using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace database.Models
{
    public class Comment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        public User PostedBy { get; set; }
        [Required]
        public string CommentText { get; set; }
        public DateTime PostedDate { get; set; }
    }
}
