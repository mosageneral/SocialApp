using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL.DTO
{
   public class UserRegisterDTO
    {
        [Required]
        public string UserName { get; set; }
        [DataType(DataType.Upload)]
        public IFormFile UserPic { get; set; }
        [Required]

        public string MobileNum { get; set; }
        [Required]

        public string Email { get; set; }
        [Required]
      
        public string Password { get; set; }
        [Required]

        public string Bio { get; set; }

        [DataType(DataType.Upload)]
        public IFormFile CovorPic { get; set; }

    }
}
