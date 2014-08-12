namespace AgroEnsayos.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Security;

    using AgroEnsayos.Models;
    using AgroEnsayos.Services;

    public class AccountController : Controller
    {
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                AgroEnsayos.Entities.User user = AgroEnsayos.Services.AuthenticationService.ValidateUser(model.Name, model.Password);
                if (user != null)
                {
                    //Roles.AddUserToRoles(model.Name, user.Role.Split(','));
                    this.Session["EmpresaId"] = user.EmpresaId;
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
            ViewBag.Categorias = CategoriaService.Get().Where(x => x.PadreId.HasValue && (x.Padre.Equals("Semillas") || x.Padre.Equals("Fitosanitarios"))).ToList();

            return View();
        }

        [Authorize()]
        [AllowAnonymous]
        public ActionResult SaveUser(string name, string password, string nombre, string apellido, string localidad, string provincia, string empresa, string email, List<string> cultivos)
        {
            int newUserId;
            int itemId;
            newUserId=AuthenticationService.User_Insert(name, password, nombre, apellido, localidad, provincia, empresa, email, "cliente");

            foreach (string item in cultivos)
            {
                itemId = AuthenticationService.UserCategoria_Insert(newUserId, Convert.ToInt32(item));
            }

            return RedirectToAction("LogIn", "Account",null);
        }

        [Authorize()]
        [AllowAnonymous]
        public JsonResult GetLocalidades_x_prov(string provincia = "Capital Federal")
        {

            List<string> localidades = LugarService.GetLocalidades_x_prov(provincia);

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
            if (AuthenticationService.UserExists(name))
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
