using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgroEnsayos.Domain.Infraestructure.Repositories;

namespace AgroEnsayos.Domain.Infraestructure.EF
{
    public class EFDataContextFactory : IDataContextFactory
    {
        public DbContext Create()
        {
            return new DbAgrotool();
        }
    }
}
