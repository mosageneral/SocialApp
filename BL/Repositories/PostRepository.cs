using BL.Infrastructure;
using DL.DBContext;
using DL.Entities;
using System.Collections.Generic;

namespace BL.Repositories
{
    public interface IPostRepository
    { }

    public class PostRepository : Repository<Post>, IPostRepository
    {
        public PostRepository(DBContext ctx) : base(ctx)
        { }

       
    }
}
