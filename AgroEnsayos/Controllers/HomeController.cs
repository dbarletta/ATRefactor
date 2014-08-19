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
        private IUserRepository _userRepository = null;

        public HomeController()
        {
            var EFfactory = new EFDataContextFactory();
            _categoryRepository = new CategoryRepository(EFfactory);
            _userRepository = new UserRepository(EFfactory);
        }

        [Authorize()]
        public ActionResult Index()
        {
            ViewBag.UserCategorias = _userRepository.Single(u => u.Name == User.Identity.Name, inc => inc.CategoriesOfIntrest)
                                                    .CategoriesOfIntrest
                                                    .Select(c => c.Id)
                                                    .ToList();

            ViewBag.Categorias = _categoryRepository.Get(c => c.Parent.Name.Equals("Semillas", StringComparison.InvariantCultureIgnoreCase) 
                                                           || c.Parent.Name.Equals("Fitosanitarios", StringComparison.InvariantCultureIgnoreCase), inc => inc.Parent)
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
