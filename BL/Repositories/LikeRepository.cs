using BL.Infrastructure;
using DL.DBContext;
using DL.Entities;
using System.Collections.Generic;

namespace BL.Repositories
{
    public interface ILikeRepository
    { }

    public class LikeRepository : Repository<Like>, ILikeRepository
    {
        public LikeRepository(DBContext ctx) : base(ctx)
        { }

       
    }
}
