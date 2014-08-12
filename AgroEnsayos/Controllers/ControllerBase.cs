using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace AgroEnsayos.Controllers
{
    public class ControllerBase : Controller
    {
        protected override void OnException(ExceptionContext filterContext)
        {
            //Handle Error
            if (filterContext.HttpContext.IsCustomErrorEnabled)
            {
                filterContext.ExceptionHandled = true;
                if (filterContext.RequestContext.HttpContext.Request.IsAjaxRequest())
                {
                    this.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    this.Json(filterContext.Exception).ExecuteResult(this.ControllerContext);
                }
                else
                {
                    this.ViewBag.Exception = filterContext.Exception;
                    this.View("Error").ExecuteResult(this.ControllerContext);
                }
            }
        }

    }
}
