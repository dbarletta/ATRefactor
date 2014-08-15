using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgroEnsayos.Domain.Entities;
using AgroEnsayos.Domain.Infraestructure.EF;
using RefactorThis.GraphDiff;

namespace AgroEnsayos.Domain.Infraestructure.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        void SaveGraph(User product);
    }

    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(IDataContextFactory factory)
            : base(factory)
        {
        }

        public void SaveGraph(User user)
        {
            using (var ctx = _factory.Create() as DbAgrotool)
            {
                ctx.UpdateGraph<User>(user, map =>
                    map.OwnedCollection(x => x.CategoriesOfIntrest));

                ctx.SaveChanges();
            }
        }
    }
}
