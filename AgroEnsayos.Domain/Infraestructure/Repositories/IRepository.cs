using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AgroEnsayos.Domain.Infraestructure.Repositories
{
    interface IRepository<T> where T: class, new()
    {
        List<T> GetAll();
        List<T> GetAll(List<Expression<Func<T, object>>> includes);

        T Single(Expression<Func<T, bool>> predicate);
        T Single(Expression<Func<T, bool>> predicate, List<Expression<Func<T, object>>> includes);

        List<T> Get(Expression<Func<T, bool>> predicate);
        List<T> Get(Expression<Func<T, bool>> predicate, List<Expression<Func<T, object>>> includes);

        void Insert(T entity);
        void Update(T entity);

        void Delete(T entity);
        void Delete(Expression<Func<T, bool>> predicate);
    }
}
