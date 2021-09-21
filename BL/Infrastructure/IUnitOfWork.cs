using BL.Repositories;
using System;

namespace BL.Infrastructure
{
    public interface IUnitOfWork : IDisposable
    {
        
        UserRepository UserRepository { get; }
       



        int Save();

    }
}
