using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgroEnsayos.Domain.Entities;

namespace AgroEnsayos.Domain.Infraestructure.Repositories
{
    public class CampaignRepository : RepositoryBase<Campaign>
    {
        public CampaignRepository(IDataContextFactory factory)
            : base(factory)
        {

        }
    }
}
