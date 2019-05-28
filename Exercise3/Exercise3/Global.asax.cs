using Exercise3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Exercise3
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            FlightSimulatorsModel.Instance.Start(new TelnetClient(), 3000);
            ModelTester.Test();
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}
