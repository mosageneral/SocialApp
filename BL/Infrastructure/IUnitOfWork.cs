using BL.Repositories;
using System;

namespace BL.Infrastructure
{
    public interface IUnitOfWork : IDisposable
    {
        
        UserRepository UserRepository { get; }
        PostRepository PostRepository { get; }
        CommentRepository CommentRepository { get; }
        LikeRepository LikeRepository { get; }




        int Save();

    }
}
