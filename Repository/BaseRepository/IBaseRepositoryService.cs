using AppEnvironment;
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
    public interface IBaseRepositoryService<T> where T : BaseEntity
    {
        Result<T> Add(T user, int? operatorUserId);
        Result<T> Update(T user, int operatorUserId);
        Result Delete(T user, int operatorUserId);
        Result<T> GetFirst(Expression<Func<T, bool>> whereCondition, Expression<Func<DbSet<T>, IQueryable<T>>>? makeJoins = null);
        IQueryable<T> Get(Expression<Func<T, bool>> whereCondition, Expression<Func<DbSet<T>, IQueryable<T>>>? makeJoins = null, bool pullFromDb = false);
        public void AddRange(IEnumerable<T> entities);
    }
}
