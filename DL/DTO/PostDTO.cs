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
        public int Id { get; set; }
        public string Content { get; set; }
      
        public IFormFile Attachment { get; set; }
       
        public int UserId { get; set; }
     
        public bool Liked { get; set; }

    }
    public class PostDTOback
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public string Attachment { get; set; }
        public bool Liked { get; set; }
        public DateTime Date { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public virtual IEnumerable<Comment> Comments { get; set; }
        public virtual IEnumerable<Like> Likes { get; set; }

    }

    public class CommentDTO
    {
        public int UserId { get; set; }
     
        public string Content { get; set; }
        public IFormFile Attachment { get; set; }
        public int PostId { get; set; }
      
      
    }
    public class CommentDTOback
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public string Content { get; set; }
        public string Attachment { get; set; }
        public int PostId { get; set; }
        public Post Post { get; set; }
        public DateTime Date { get; set; }


    }
    public class LikeDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
     
        public int PostId { get; set; }
     
       
    }
 }
