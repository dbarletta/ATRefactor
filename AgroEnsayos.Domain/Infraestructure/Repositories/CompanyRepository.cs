using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgroEnsayos.Domain.Entities;

namespace AgroEnsayos.Domain.Infraestructure.Repositories
{
    public interface ICompanyRepository : IRepository<Company> { }

    public class CompanyRepository : RepositoryBase<Company>, ICompanyRepository
    {
        public CompanyRepository(IDataContextFactory factory)
            : base(factory)
        {

        }
    }
}
