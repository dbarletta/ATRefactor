using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AgroEnsayos.Services;
using AgroEnsayos.Entities;
using AgroEnsayos.Helpers;

namespace AgroEnsayos.Controllers
{
    public class AtributosController : Controller
    {
        [Authorize(Roles = "Administrador")]
        public ActionResult Admin(int id)
        {
            ViewBag.Atributos = AtributoService.Get(id);
            return PartialView();
        }

        [Authorize(Roles="Administrador")]
        public ActionResult Equivalencias()
        {
            ViewBag.Categorias = CategoriaService.Get().Where(x => x.PadreId.HasValue && x.Padre.Equals("Semillas")).ToList();

            return PartialView();
        }

        [HttpGet()]
        public string GetAtributos(int categoriaId)
        {
            return AtributoService.Get(categoriaId).ToJson();
        }

        [HttpGet()]
        public string GetValores(int atributoId)
        {
            return AtributoService.GetValores(atributoId).ToJson();
        }

        [HttpGet()]
        public string GetEquivalencias(int atributoId)
        {
            return AtributoService.GetEquivalencias(atributoId).ToJson();
        }

        [HttpPost()]
        public void SaveEquivalencia(Models.EquivalenciaModel model)
        {
            AtributoService.SaveEquivalencia(model.Equivalencias);
        }
    }
}
