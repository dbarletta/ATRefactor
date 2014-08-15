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

        List<Product> Lookup(int categoriaId, string searchTerm, string empresa = "", string antiguedad = "", string region = "", List<string> cond_atributo = null, int limit_ini = 0, int limit_fin = 0);
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

        public List<Product> Lookup(int categoriaId, string searchTerm, string empresa = "", string antiguedad = "", string region = "", List<string> cond_atributo = null, int limit_ini = 0, int limit_fin = 0)
        {
            throw new NotImplementedException();
        }
    }
}
