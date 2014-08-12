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
    public class CategoriasController : ApiController
    {
        // GET api/categorias
        public List<Categoria> Get()
        {
            return CategoriaService.Get().Where(c => c.PadreId.HasValue && c.PadreId.Value == 1).ToList();
        }

        // GET api/categorias/5
        public string Get(int id)
        {
            throw new NotImplementedException();
        }

        // POST api/categorias
        public void Post([FromBody]string value)
        {
            throw new NotImplementedException();
        }

        // DELETE api/categorias/5
        public void Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
