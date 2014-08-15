using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AgroEnsayos.Domain.Entities;
using AgroEnsayos.Domain.Infraestructure.EF;
using AgroEnsayos.Domain.Infraestructure.Repositories;

namespace AgroEnsayos.Controllers.WebApi
{
    public class EmpresasController : ApiController
    {
        private ICompanyRepository _companyRepository = null;

        public EmpresasController()
        {
            var ctxFactory = new EFDataContextFactory();
            _companyRepository = new CompanyRepository(ctxFactory);
        }
        // GET api/empresas
        public List<Company> Get()
        {
            return _companyRepository.Get(c => !c.IsDisabled);
        }

        // GET api/empresas/5
        public string Get(int id)
        {
            throw new NotImplementedException();
        }

        // POST api/empresas
        public void Post([FromBody]string value)
        {
            throw new NotImplementedException();
        }

        // PUT api/empresas/5
        public void Put(int id, [FromBody]string value)
        {
            throw new NotImplementedException();
        }

        // DELETE api/empresas/5
        public void Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
