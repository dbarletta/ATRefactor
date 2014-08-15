using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AgroEnsayos.Domain.Infraestructure.EF;
using AgroEnsayos.Domain.Infraestructure.Repositories;
using AgroEnsayos.Helpers;

namespace AgroEnsayos.Controllers
{
    public class AtributosController : Controller
    {
        private IAttributeRepository _attributeRepository = null;
        private ICategoryRepository _categoryRepository = null;

        public AtributosController()
        {
            var ctxFactory = new EFDataContextFactory();
            _attributeRepository = new AttributeRepository(ctxFactory);
            _categoryRepository = new CategoryRepository(ctxFactory);
        }

        [Authorize(Roles = "Administrador")]
        public ActionResult Admin(int id)
        {
            ViewBag.Atributos = _categoryRepository.Single(cat => cat.Id == id, c => c.Attributes).Attributes;
            return PartialView();
        }

        [Authorize(Roles="Administrador")]
        public ActionResult Equivalencias()
        {
            ViewBag.Categorias = _categoryRepository.Get(x => x.Parent.Name.Equals("Semillas", StringComparison.InvariantCultureIgnoreCase)).ToList();
            return PartialView();
        }

        [HttpGet()]
        public string GetAtributos(int categoriaId)
        {
            return _categoryRepository.Single(cat => cat.Id == categoriaId, c => c.Attributes)
                                      .Attributes
                                      .ToJson();
        }

        [HttpGet()]
        public string GetValores(int atributoId)
        {
            return _attributeRepository.GetOriginalValues(atributoId).ToJson();
        }

        [HttpGet()]
        public string GetEquivalencias(int atributoId)
        {
            return _attributeRepository.Single(x => x.Id == atributoId, y => y.AttributeMappings).ToJson();
        }

        [HttpPost()]
        public void SaveEquivalencia(Domain.Entities.Attribute attribute)
        {
            _attributeRepository.SaveGraph(attribute);
        }
    }
}
