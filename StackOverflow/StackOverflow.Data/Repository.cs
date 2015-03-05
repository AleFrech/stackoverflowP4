using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace StackOverflow.Data
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity:class
    {
        internal StackOverflowContext Context;
        internal DbSet<TEntity> DbSet;
         public Repository(StackOverflowContext context)
        {
            Context = context;
            DbSet = context.Set<TEntity>();
        }
        public IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = "")
        {
            IQueryable<TEntity> query = DbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }
        }

        public TEntity GetById(object id)
        {
            return DbSet.Find(id);
        }

        public void Insert(TEntity entity)
        {
            DbSet.Add(entity);
        }

        public void Delete(object id)
        {
            TEntity entityToDelete = DbSet.Find(id);
            Delete(entityToDelete);
        }

        public void Update(TEntity entityToUpdate)
        {
            DbSet.Attach(entityToUpdate);
            Context.Entry(entityToUpdate).State = EntityState.Modified;
        }
    }

    public interface IRepository<T>
    {
        IEnumerable<T> Get(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = "");
        T GetById(object id);
        void Insert(T entity);
        void Delete(object id);
        void Update(T entityToUpdate);
    }
}
