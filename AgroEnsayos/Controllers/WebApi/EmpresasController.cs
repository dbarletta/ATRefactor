using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AgroEnsayos.Entities;
using AgroEnsayos.Services;

namespace AgroEnsayos.Controllers.WebApi
{
    public class EmpresasController : ApiController
    {
        // GET api/empresas
        public List<Empresa> Get()
        {
            return EmpresaService.Get();
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
