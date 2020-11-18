using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Leaderboard.BL.Interfaces.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        TEntity Get(int id);
        IEnumerable<TEntity> GetAll();
        TEntity Find(Expression<Func<TEntity, bool>> predicate);
        IEnumerable<TEntity> FindAll(Expression<Func<TEntity, bool>> predicate);
        TEntity Update(TEntity entity);
        TEntity Add(TEntity entity);
        void AddRange(IEnumerable<TEntity> entities);
        void Remove(TEntity entiry);
        void RemovRange(IEnumerable<TEntity> entities);
    }
}
