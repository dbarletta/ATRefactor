using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AgroEnsayos.Domain;
using AgroEnsayos.Domain.Infraestructure.EF;
using AgroEnsayos.Domain.Infraestructure.Repositories;

namespace AgroEnsayos.Controllers.WebApi
{
    public class AtributosController : ApiController
    {
        private IAttributeRepository _attributeRepository = null;

        public AtributosController()
        {
            var ctxFactory = new EFDataContextFactory();
            _attributeRepository = new AttributeRepository(ctxFactory);
        }

        // GET api/Atributos
        public IEnumerable<Domain.Entities.Attribute> Get()
        {
            return _attributeRepository.Get(a => !a.IsDisabled);
        }

        // GET api/Atributos/5
        public Domain.Entities.Attribute Get(int id)
        {
            return _attributeRepository.Single(a => a.Id == id && !a.IsDisabled);
        }

        // POST api/Atributos
        public Domain.Entities.Attribute Post([FromBody]Domain.Entities.Attribute attr)
        {
            _attributeRepository.SaveGraph(attr);
            return attr;
        }

        // DELETE api/Atributos/5
        public void Delete(int id)
        {
            var attr = _attributeRepository.Single(a => a.Id == id);
            attr.Disable();
        }
    }
}