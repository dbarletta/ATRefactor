using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgroEnsayos.Domain.Infraestructure.Repositories
{
    public class AttributeRepository : RepositoryBase<Entities.Attribute>
    {
        public AttributeRepository(IDataContextFactory factory)
            : base(factory)
        {

        }
    }
}
