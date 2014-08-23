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
    public interface IPlaceRepository : IRepository<Place> 
    {
        IList<string> GetProvincesWithTests(int categoryId);
        IList<string> GetLocalitiesWithTests(int categoryId);
    }

    public class PlaceRepository : RepositoryBase<Place>, IPlaceRepository
    {
        public PlaceRepository(IDataContextFactory factory)
            : base(factory)
        {

        }

        public IList<string> GetProvincesWithTests(int categoryId)
        {
            using(var ctx = _factory.Create() as DbAgrotool)
            {
                var query = ctx.Places.Where(p => !string.IsNullOrEmpty(p.Province)
                                               && p.Tests.Where(x => x.Product.CategoryId == categoryId)
                                                         .Select(x => x.PlaceId)
                                                         .Contains(p.Id))
                                      .Select(p => p.Province)
                                      .Distinct()
                                      .ToList();

                return query;
            }
            
        }


        public IList<string> GetLocalitiesWithTests(int categoryId)
        {
            using (var ctx = _factory.Create() as DbAgrotool)
            {
                var query = ctx.Places.Where(p => !string.IsNullOrEmpty(p.Locality)
                                               && p.Tests.Where(x => x.Product.CategoryId == categoryId)
                                                         .Select(x => x.PlaceId)
                                                         .Contains(p.Id))
                                      .Select(p => p.Locality)
                                      .Distinct()
                                      .ToList();

                return query;
            }
        }
    }
}
