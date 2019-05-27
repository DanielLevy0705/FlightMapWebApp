using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Exercise3
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            
            routes.MapRoute(
                "BasicDisplay",
                "display/{param1}/{param2}/{freq}",
                new {Controller = "Home", Action = "Display", freq = UrlParameter.Optional }
            );
            routes.MapRoute(
                "SaveTrack",
                "save/{ip}/{port}/{freq}/{duration}/{file}",
                new { Controller = "Home", Action = "Save" }
            );
            routes.MapRoute(
                "GetLocation",
                "info/location",
                new { Controller = "Info", Action = "location" }
            );




        }
    }
}
