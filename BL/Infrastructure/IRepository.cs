using System;
using System.Linq;
using System.Linq.Expressions;

namespace BL.Infrastructure
{
    public interface IRepository<TEntity>
    {
        IQueryable<TEntity> GetAll();
        IQueryable<TEntity> GetMany(Expression<Func<TEntity, bool>> where);
        TEntity Get(Expression<Func<TEntity, bool>> where);
        TEntity GetById(params object[] id);
        void Add(TEntity entity);
        void Update(TEntity entity);
        void Delete(params object[] id);
       
    }
}
