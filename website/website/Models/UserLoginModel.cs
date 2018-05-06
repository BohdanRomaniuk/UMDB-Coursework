using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace website.Models
{
    public class UserLoginModel
    {
        [Required(ErrorMessage = "Ви не вказала імя користувача(нікнейм)")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Ви не ввели пароль")]
        public string Password { get; set; }
    }
}
