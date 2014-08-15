using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using AgroEnsayos.Domain.Infraestructure.EF;
using AgroEnsayos.Domain.Infraestructure.Repositories;
using AgroEnsayos.Models;

namespace AgroEnsayos.Controllers
{
    public class AccountController : Controller
    {
        private IUserRepository _userRepository = null;
        private ICategoryRepository _categoryRepository = null;
        private IPlaceRepository _placeRepository = null;

        public AccountController()
        {
            var ctxFactory = new EFDataContextFactory();
            _userRepository = new UserRepository(ctxFactory);
            _categoryRepository = new CategoryRepository(ctxFactory);
            _placeRepository = new PlaceRepository(ctxFactory);
        }

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                Domain.Entities.User user = _userRepository.Single(x => x.Name == model.Name && x.Password == model.Password && !x.IsDisabled);
                if (user != null)
                {
                    //Roles.AddUserToRoles(model.Name, user.Role.Split(','));
                    this.Session["CompanyId"] = user.CompanyId;
                    FormsAuthentication.RedirectFromLoginPage(model.Name, model.RememberMe);
                    RedirectToLocal(returnUrl);
                }
                else
                {
                    ModelState.AddModelError("", "The user name or password provided is incorrect.");
                }
            }
            return View(model);
        }

        [Authorize()]
        [AllowAnonymous]
        public ActionResult NewUser()
        {
            ViewBag.Categorias = _categoryRepository.Get(x => 
                x.Parent.Name.Equals("Semillas", StringComparison.InvariantCultureIgnoreCase) ||
                x.Parent.Name.Equals("Fitosanitarios", StringComparison.InvariantCultureIgnoreCase))
                .ToList();

            return View();
        }

        //TODO: Ver de enviar el user completo con las categorias desde la UI
        [Authorize()]
        [AllowAnonymous]
        public ActionResult SaveUser(Domain.Entities.User user)
        {
            _userRepository.SaveGraph(user);

            return RedirectToAction("LogIn", "Account",null);
        }

        [Authorize()]
        [AllowAnonymous]
        public JsonResult GetLocalidades_x_prov(string provincia = "Capital Federal")
        {
            List<string> localidades = _placeRepository.Get(x => x.Province == provincia)
                                                       .Select(x => x.Locality)
                                                       .ToList();

            return Json(new AgroEnsayos.Helpers.JsonResultObject { haserror = false, message = provincia != "" ? "Existe" : "No Existe", result = localidades });

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }


        [Authorize()]
        [AllowAnonymous]
        public JsonResult UserExists(string name)
        {
            int res = 0;
            if (_userRepository.Any(x => x.Name == name))
            {
                res = 1;
            }

            return Json(new AgroEnsayos.Helpers.JsonResultObject { haserror = false, message = name != "" ? "Existe" : "No Existe", result = res });

        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
