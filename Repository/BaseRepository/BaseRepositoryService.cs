using AppEnvironment;
using Context;
using Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repository.NewFolder
{
    public abstract class BaseRepository<T> : IBaseRepositoryService<T>
    where T : BaseEntity
    {
        private ApplicationDbContext _context;
        protected BaseRepository(ApplicationDbContext ctx)
        {
            _context = ctx;

        }
        public Result<T> Add(T entity, int? operatorUserId)
        {
            try
            {
                entity.CreatedAt = DateTime.Now;
                entity.CreatedBy = operatorUserId;
                var addedEntity = _context.Entry(entity);
                addedEntity.State = EntityState.Added;
                _context.SaveChanges();
                return new Result<T>(entity);
            }
            catch (Exception e)
            {
                return new Result<T>(e);
            }
        }

        public Result<T> Update(T entity, int operatorUserId)
        {
            try
            {

                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedBy = operatorUserId;
                var updatedEntity = _context.Entry(entity);
                updatedEntity.State = EntityState.Modified;
                _context.SaveChanges();

                return new Result<T>(entity);
            }
            catch (Exception e)
            {
                return new Result<T>(e);
            }
        }

        public Result Delete(T entity, int operatorUserId)
        {
            try
            {
                var ent = GetDbSet().FirstOrDefault(u => u.Id == entity.Id);
                ent.DeletedAt = DateTime.Now;
                ent.DeletedBy = operatorUserId;

                var res = Update(ent, operatorUserId);
                _context.SaveChanges();
                if (res.IsSuccess)
                {
                    return new Result(MessageType.DeleteSuccess);
                }

                return new Result
                {
                    Exception = res.Exception,
                    IsSuccess = false,
                    Message = res.Message,
                    MessageType = res.MessageType
                };
            }
            catch (Exception e)
            {
                return new Result(e);
            }
        }

        protected IQueryable<T> GetDbSet(Expression<Func<DbSet<T>, IQueryable<T>>>? makeJoins = null, bool includeDeleted = true)
        {

            IQueryable<T> dbSet = _context.Set<T>();
            if (makeJoins != null) // todo test this
            {
                Expression expr = Expression.Invoke(makeJoins, Expression.Constant(dbSet));
                var lambda = Expression.Lambda<Func<IQueryable<T>>>(expr);
                var compiled = lambda.Compile();
                dbSet = compiled.Invoke();
            }

            if (includeDeleted)
            {
                dbSet = dbSet.Where(u => u.DeletedAt == null);
            }

            return dbSet;
        }

        public Result<T> GetFirst(Expression<Func<T, bool>> whereCondition, Expression<Func<DbSet<T>, IQueryable<T>>>? makeJoins = null)
        {
            try
            {
                var res = GetDbSet(makeJoins).FirstOrDefault(whereCondition);
                return res == null ? new Result<T>(MessageType.RecordNotFound) : new Result<T>(res);
            }
            catch (Exception e)
            {
                return new Result<T>(e);
            }
        }

        public IQueryable<T> Get(Expression<Func<T, bool>> whereCondition, Expression<Func<DbSet<T>, IQueryable<T>>>? makeJoins = null, bool pullFromDb = false)
        {
            //todo we can manage pagination/sorting here
            var q = GetDbSet(makeJoins).Where(whereCondition);
            return pullFromDb ? q.Select(u => u) : q;
        }
        public void AddRange(IEnumerable<T> entities)
        {
            _context.Set<T>().AddRange(entities);
            _context.SaveChanges();
        }

    }
}
