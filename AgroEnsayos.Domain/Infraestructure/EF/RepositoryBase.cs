using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AgroEnsayos.Domain.Infraestructure.EF;

namespace AgroEnsayos.Domain.Infraestructure.Repositories
{
    public abstract class RepositoryBase<T> : IRepository<T> where T: class, new()
    {
        private IDataContextFactory _factory;

        public RepositoryBase(IDataContextFactory factory)
        {
            _factory = factory;
        }

        public List<T> GetAll()
        {
            using (var db = _factory.Create())
            {
                return (List<T>)db.Set<T>().ToList();
            }
        }

        public List<T> GetAll(List<Expression<Func<T, object>>> includes)
        {
            List<string> includelist = new List<string>();

            foreach (var item in includes)
            {
                MemberExpression body = item.Body as MemberExpression;
                if (body == null)
                    throw new ArgumentException("The body must be a member expression");

                includelist.Add(body.Member.Name);
            }

            using (var db = _factory.Create())
            {
                DbQuery<T> query = db.Set<T>();

                includelist.ForEach(x => query = query.Include(x));

                return (List<T>)query.ToList();
            }

        }


        public T Single(Expression<Func<T, bool>> predicate)
        {
            using (var db = _factory.Create())
            {
                return db.Set<T>().FirstOrDefault(predicate);
            }
        }

        public T Single(Expression<Func<T, bool>> predicate, List<Expression<Func<T, object>>> includes)
        {
            List<string> includelist = new List<string>();

            foreach (var item in includes)
            {
                MemberExpression body = item.Body as MemberExpression;
                if (body == null)
                    throw new ArgumentException("The body must be a member expression");

                includelist.Add(body.Member.Name);
            }

            using (var db = _factory.Create())
            {
                DbQuery<T> query = db.Set<T>();

                includelist.ForEach(x => query = query.Include(x));

                return query.FirstOrDefault(predicate);
            }
        }


        public List<T> Get(Expression<Func<T, bool>> predicate)
        {
            using (var db = _factory.Create())
            {
                return (List<T>)db.Set<T>().Where(predicate).ToList();
            }
        }

        public List<T> Get(Expression<Func<T, bool>> predicate, List<Expression<Func<T, object>>> includes)
        {
            List<string> includelist = new List<string>();

            foreach (var item in includes)
            {
                MemberExpression body = item.Body as MemberExpression;
                if (body == null)
                    throw new ArgumentException("The body must be a member expression");

                includelist.Add(body.Member.Name);
            }

            using (var db = _factory.Create())
            {
                DbQuery<T> query = db.Set<T>();

                includelist.ForEach(x => query = query.Include(x));

                return (List<T>)query.Where(predicate).ToList();
            }
        }


        public void Insert(T entity)
        {
            using (var db = _factory.Create())
            {
                db.Set<T>().Add(entity);
                db.SaveChanges();
            }
        }

        public void Update(T entity)
        {
            using (var db = _factory.Create())
            {
                db.Entry(entity).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        public void Delete(T entity)
        {
            using (var db = _factory.Create())
            {
                db.Entry(entity).State = EntityState.Deleted;
                db.SaveChanges();
            }
        }

        public void Delete(Expression<Func<T, bool>> predicate)
        {
            using (var db = _factory.Create())
            {
                var entities = db.Set<T>().Where(predicate).ToList();
                entities.ForEach(x => db.Entry(x).State = EntityState.Deleted);
                db.SaveChanges();
            }
        }
    }
}
