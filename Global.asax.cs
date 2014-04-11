using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using HRE.Models;

namespace HRE {

    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // Visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication {


        public static void RegisterGlobalFilters(GlobalFilterCollection filters) {
            filters.Add(new HandleErrorAttribute());
        }


        public static void RegisterRoutes(RouteCollection routes) {

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            // routes.IgnoreRoute("{resource}.aspx/{*pathInfo}");
            routes.IgnoreRoute("{resource}.html/{*pathInfo}");
            routes.IgnoreRoute("{resource}.php/{*pathInfo}");

            // routes.IgnoreRoute("index.html");
            // routes.IgnoreRoute("");

            /* routes.MapRoute(
                "StaticHtml", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Static", action = "Index", id = UrlParameter.Optional},     // Parameter defaults.
                new { controller = "Home|Meedoen|Newsletter|Inschrijvingen|Programma|Video" } // Route constraints.
            ); */

           routes.MapRoute(
                "Html", // Route name
                "{controller}/{action}", // URL with parameters
                new { controller = "Html", action = "GetHtml"}     // Parameter defaults.
            );

            routes.MapRoute(
                "Controllers", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional},     // Parameter defaults.
                new { controller = "Home|Meedoen|Newsletter|Inschrijvingen|Programma|Video|Account|emailaudits|Inschrijven" } // Route constraints.
            );
        }


        protected void Application_Start() {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            // Create the admin role and admin users if they don't exist yet.
            InschrijvingenRepository.CreateAdminRoleAndAdmins();
        }
    }
}