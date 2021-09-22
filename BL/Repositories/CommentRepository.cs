using BL.Infrastructure;
using DL.DBContext;
using DL.Entities;
using System.Collections.Generic;

namespace BL.Repositories
{
    public interface ICommentRepository
    { }

    public class CommentRepository : Repository<Comment>, ICommentRepository
    {
        public CommentRepository(DBContext ctx) : base(ctx)
        { }

       
    }
}
