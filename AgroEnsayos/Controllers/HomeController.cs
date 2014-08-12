using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AgroEnsayos.Services;
using AgroEnsayos.Entities;

using AgroEnsayos.Models;

namespace AgroEnsayos.Controllers
{
    public class HomeController : Controller
    {
        [Authorize()]
        public ActionResult Index()
        {
            ViewBag.UserCategorias = AuthenticationService.GetUserCategorias(User.Identity.Name);
            ViewBag.Categorias = CategoriaService.Get().Where(x => x.PadreId.HasValue && (x.Padre.Equals("Semillas") || x.Padre.Equals("Fitosanitarios"))).ToList();

            return View();
        }

        [Authorize()]
        public ActionResult Busqueda(int id)
        {
            ViewBag.CategoriaId = id;
            return View();
        }


    }
}
