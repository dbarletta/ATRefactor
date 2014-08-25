using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgroEnsayos.Domain.Entities;
using AgroEnsayos.Domain.Infraestructure.EF;
using RefactorThis.GraphDiff;
using System.Data.SqlClient;
using System.Data.Entity;

namespace AgroEnsayos.Domain.Infraestructure.Repositories
{
    public interface IAttributeRepository : IRepository<Entities.Attribute>
    {
        List<string> GetOriginalValues(int attributeId);
        void SaveGraph(Domain.Entities.Attribute attribute);

        List<AttributeMapping> GetFilters(int categoryId);
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
            using (var ctx = _factory.Create() as DbAgrotool)
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

        public List<AttributeMapping> GetFilters(int categoryId)
        {
            using (var ctx = _factory.Create() as DbAgrotool)
            {
                var query = ctx.AttributeMappings.Where(a => a.Attribute.Categories.Select(c => c.Id).Contains(categoryId)
                                                          && a.Attribute.IsFilter)
                                           .Include(a => a.Attribute)
                                           .OrderBy(a => a.Attribute.Family)
                                           .ThenBy(a => a.Attribute.Name);

                var result = query.ToList();

                result.ForEach(a => a.Attribute.Family = a.Attribute.Family ?? "Caracteristicas");

                return result;
            }
        }
    }
}
