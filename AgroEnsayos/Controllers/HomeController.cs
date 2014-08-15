using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AgroEnsayos.Domain.Infraestructure.EF;
using AgroEnsayos.Domain.Infraestructure.Repositories;
using AgroEnsayos.Models;

namespace AgroEnsayos.Controllers
{
    public class HomeController : Controller
    {
        private ICategoryRepository _categoryRepository = null;

        public HomeController()
        {
            var EFfactory = new EFDataContextFactory();
            _categoryRepository = new CategoryRepository(EFfactory);
        }

        [Authorize()]
        public ActionResult Index()
        {
            ViewBag.UserCategorias = _categoryRepository.Get(c => c.Users.Select(u => u.Name).Contains(User.Identity.Name))
                                                        .Select(c => c.Id);

            ViewBag.Categorias = _categoryRepository.Get(c => c.Parent.Name.Equals("Semillas", StringComparison.InvariantCultureIgnoreCase) 
                                                           || c.Parent.Name.Equals("Fitosanitarios", StringComparison.InvariantCultureIgnoreCase))
                                                    .ToList();

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
