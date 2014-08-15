using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgroEnsayos.Domain.Entities;

namespace AgroEnsayos.Domain.Infraestructure.Repositories
{
    public interface ICategoryRepository : IRepository<Category> { }

    public class CategoryRepository : RepositoryBase<Category>, ICategoryRepository
    {
        public CategoryRepository(IDataContextFactory factory)
            : base(factory)
        {

        }
    }
}
