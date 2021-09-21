using BL.Repositories;
using DL.DBContext;


namespace BL.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private DBContext _ctx;
        public UnitOfWork(DBContext ctx)
        {
            _ctx = ctx;
            _ctx.ChangeTracker.LazyLoadingEnabled = true;
        }
       

        public UserRepository UserRepository => new UserRepository(_ctx);

        public void Dispose()
        {
            _ctx.Dispose();
        }

      

        public int Save()
        {
            return _ctx.SaveChanges();
        }
    }
}
