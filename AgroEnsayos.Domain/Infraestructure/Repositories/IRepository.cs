using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AgroEnsayos.Domain.Infraestructure.Repositories
{
    public interface IRepository<T> where T: class, new()
    {
        bool Any(Expression<Func<T, bool>> predicate);

        T Single(Expression<Func<T, bool>> predicate);
        T Single(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);

        List<T> GetAll();
        List<T> GetAll(int page, int pagesize);
        List<T> GetAll(params Expression<Func<T, object>>[] includes);
        List<T> GetAll(int page, int pagesize, params Expression<Func<T, object>>[] includes);

        List<T> Get(Expression<Func<T, bool>> predicate);
        List<T> Get(int page, int pagesize, Expression<Func<T, bool>> predicate);
        List<T> Get(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);
        List<T> Get(int page, int pagesize, Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);

        void Insert(T entity);
        void Update(T entity);

        void Delete(T entity);
        void Delete(Expression<Func<T, bool>> predicate);
    }
}
