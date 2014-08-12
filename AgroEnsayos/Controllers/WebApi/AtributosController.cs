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
    public class AtributosController : ApiController
    {
        // GET api/Atributos
        public IEnumerable<Atributo> Get()
        {
            return AtributoService.Get(0);
        }

        // GET api/Atributos/5
        public Atributo Get(int id)
        {
            return AtributoService.GetById(id);
        }

        // POST api/Atributos
        public Atributo Post([FromBody]Atributo attr)
        {
            AtributoService.Save(attr);
            return attr;
        }

        // DELETE api/Atributos/5
        public void Delete(int id)
        {
            AtributoService.DisableAttributte(id);
        }
    }
}