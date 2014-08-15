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
    public interface IPlaceRepository : IRepository<Place> { }

    public class PlaceRepository : RepositoryBase<Place>, IPlaceRepository
    {
        public PlaceRepository(IDataContextFactory factory)
            : base(factory)
        {

        }

    }
}
