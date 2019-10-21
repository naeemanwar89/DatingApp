using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.API.Dtos
{
    public class UserRegisterDto
    {
        [Required]
        public string username { get; set; }
        [Required]
        [MaxLength(8,ErrorMessage ="You must specify password between 4 and 8")]
        [MinLength(4,ErrorMessage = "You must specify password between 4 and 8")]
        public string password { get; set; }
    }
}
