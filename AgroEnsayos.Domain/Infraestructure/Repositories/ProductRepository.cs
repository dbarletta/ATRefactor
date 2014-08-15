using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AgroEnsayos.Domain.Entities;
using AgroEnsayos.Domain.Infraestructure.EF;
using RefactorThis.GraphDiff;

namespace AgroEnsayos.Domain.Infraestructure.Repositories
{
    public interface IProductRepository : IRepository<Product> 
    {
        void SaveGraph(Product product);
    }

    public class ProductRepository : RepositoryBase<Product>, IProductRepository
    {
        public ProductRepository(IDataContextFactory factory)
            : base(factory)
        {

        }


        public void SaveGraph(Product product)
        {
            using(var ctx = _factory.Create() as DbAgrotool)
            {
                ctx.UpdateGraph<Product>(product, map => 
                    map.OwnedCollection(x => x.AttributeMappings)
                       .OwnedCollection(x => x.Places));
                ctx.SaveChanges();
            }
        }
    }
}
