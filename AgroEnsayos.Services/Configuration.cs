namespace AgroEnsayos.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Configuration;
    using System.Web.Configuration;
    using System.Web;

    public static class Configuration
    {

        public static string ConnectionString
        {
            get
            {
               
#if Debug
                return @"Server=.;Database=AgroEnsayos;Trusted_Connection=yes;";
#else
                //WINDOWS AZURE
                //return @"Server=f61mw4yvr9.database.windows.net,1433;Database=AgroEnsayos;Uid=lt_admin;Pwd=secure3001!;";
                //return @"Server=.;Database=AgroEnsayos;Trusted_Connection=yes;";
                return System.Configuration.ConfigurationManager.ConnectionStrings["connString"].ToString();
#endif
            }
        }

        public static string ImportersPath
        {
            get
            {
                return HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["ImportersPath"].ToString());                 
            }
        }
    }
}
