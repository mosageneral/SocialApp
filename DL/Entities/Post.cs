using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL.Entities
{
   public class Post
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public string Attachment { get; set; }
        public DateTime Date { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public virtual IEnumerable<Comment> Comments { get; set; }
        public virtual IEnumerable<Like> Likes { get; set; }
    }

    public class Like
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int PostId { get; set; }
        public Post Post { get; set; }
        public DateTime Date { get; set; }
    }

    public class Comment
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public string Content { get; set; }
        public string Attachment { get; set; }
        public int PostId { get; set; }
        public Post Post { get; set; }
        public DateTime  Date { get; set; }

    }
}
