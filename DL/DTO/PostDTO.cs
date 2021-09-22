using DL.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL.DTO
{
   public class PostDTO
    {
        public string Content { get; set; }
        public IFormFile Attachment { get; set; }
        public DateTime Date { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public virtual IEnumerable<CommentDTO> Comments { get; set; }
        public virtual IEnumerable<Like> Likes { get; set; }
    }

    public class CommentDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public string Content { get; set; }
        public IFormFile Attachment { get; set; }
        public int PostId { get; set; }
        public Post Post { get; set; }
        public DateTime Date { get; set; }
    }
}
