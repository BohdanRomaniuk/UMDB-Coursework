using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace website.Models
{
    public class UserRegisterModel
    {
        [Required(ErrorMessage = "Ви не вказали імені користувача (нікнейму)")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Ви не ввели пароль")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Ви не вказали свого імені")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Ви не вказали свою email адресу")]
        public string Email { get; set; }
    }
}
