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
    public interface IAttributeRepository : IRepository<Entities.Attribute> 
    {
        List<string> GetOriginalValues(int attributeId);
        void SaveGraph(Domain.Entities.Attribute attribute);

        List<Entities.Attribute> GetFilters(int categoryId);
    }

    public class AttributeRepository : RepositoryBase<Entities.Attribute>, IAttributeRepository
    {
        public AttributeRepository(IDataContextFactory factory)
            : base(factory)
        {
            
        }

        //TODO: Chequear que devuelva valores que aun no tienen mappings.
        public List<string> GetOriginalValues(int attributeId)
        {
            using(var ctx = _factory.Create() as DbAgrotool)
            {
                var query = ctx.Attributes.Single(x => x.Id == attributeId)
                                          .AttributeMappings
                                          .Where(m => string.IsNullOrEmpty(m.MappedValue))
                                          .Select(m => m.OriginalValue);

                return query.ToList();
            }
        }

        public void SaveGraph(Entities.Attribute attribute)
        {
            using (var ctx = _factory.Create() as DbAgrotool)
            {
                ctx.UpdateGraph<Domain.Entities.Attribute>(attribute, map =>
                    map.OwnedCollection(x => x.AttributeMappings)
                       .OwnedCollection(x => x.Categories));
                ctx.SaveChanges();
            }
        }

        //TODO: Chequear los resultados de este query.
        public List<Entities.Attribute> GetFilters(int categoryId)
        {
            using (var ctx = _factory.Create() as DbAgrotool)
            {
                var query = ctx.Categories.Single(c => c.Id == categoryId)
                                .Attributes.Where(a => a.IsFilter)
                                .OrderByDescending(a => new { a.Family, a.Name });
                
                    return query.ToList();
            }
        }
    }
}
