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
       
        public int UserId { get; set; }
      
      
    }

    public class CommentDTO
    {
        public int UserId { get; set; }
     
        public string Content { get; set; }
        public IFormFile Attachment { get; set; }
        public int PostId { get; set; }
      
      
    }
    public class LikeDTO
    {
        public int UserId { get; set; }
     
        public int PostId { get; set; }
     
       
    }
 }
