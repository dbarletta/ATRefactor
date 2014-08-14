using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgroEnsayos.Domain.Infraestructure.Repositories
{
    public interface IDataContextFactory
    {
        DbContext Create();
    }
}
