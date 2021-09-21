using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL.Entities
{
  public  class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string UserPic { get; set; }
        public string MobileNum { get; set; }
        public string Password { get; set; }
        public string Bio { get; set; }
        public string CovorPic { get; set; }
        public bool IsActive { get; set; }
    }
}
