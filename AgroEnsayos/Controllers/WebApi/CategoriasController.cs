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
    public class CategoriasController : ApiController
    {
        private ICategoryRepository _categoryRepository = null;

        public CategoriasController()
        {
            var ctxFactory = new EFDataContextFactory();
            _categoryRepository = new CategoryRepository(ctxFactory);
        }
        // GET api/categorias
        public List<Category> Get()
        {
            return _categoryRepository.Get(c => c.ParentId == 1).ToList();
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
