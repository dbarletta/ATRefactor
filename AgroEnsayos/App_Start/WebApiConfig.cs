using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Controllers;
using AgroEnsayos.Modules;
using Newtonsoft.Json.Serialization;

namespace AgroEnsayos
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            //config.Services.Replace(typeof(IHttpActionSelector), new HybridActionSelector());


            /* ORIGINAL */
            config.Routes.MapHttpRoute(
                name: "ApiById",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            /* RUSITO */
            //config.Routes.MapHttpRoute(
            //    name: "ApiById",
            //    routeTemplate: "api/{controller}/{id}",
            //    defaults: new { id = RouteParameter.Optional },
            //    constraints: new { id = @"^[0-9]+$" }
            //);

            //config.Routes.MapHttpRoute(
            //    name: "ApiByAction",
            //    routeTemplate: "api/{controller}/{action}",
            //    defaults: new { action = "Get" }
            //);

            /* MAGICAL */
            //config.Routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    routeTemplate: "api/{controller}/{id}/{action}/{actionid}/{subaction}/{subactionid}",
            //    defaults: new 
            //    { 
            //        id = RouteParameter.Optional, 
            //        action = RouteParameter.Optional, 
            //        actionid = RouteParameter.Optional, 
            //        subaction = RouteParameter.Optional, 
            //        subactionid = RouteParameter.Optional 
            //    }
            //);

        }
    }
}
