﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Exercise3.Models;
using Exercise3.Models.Interface;
using System.Net.Sockets;
using System.Net;

namespace Exercise3.Controllers
{
    public class HomeController : Controller
    {


        // GET: /diaplay/
        public ActionResult Display(string param1, string param2, int freq = -1)
        {

            //IFlightSimulatorModel model = FlightSimulatorModel.Instance; //remember to create the first instance (there is no settings this time)
       
            if (Parameter.isIP(param1) && Parameter.isInt(param2))
                return Content("hello:");

            if (Parameter.isFile(param1) && Parameter.isNum(param2))
                return Content("file");
            return Content("wrong format");
            //return View(model);
        }
        public ActionResult Save(string ip, int port, int freq, double duration, string file)
        {

            //IFlightSimulatorModel model = FlightSimulatorModel.Instance; //remember to create the first instance (there is no settings this time)

            return Content("hello:" + ip);
            //return View(model);
        }
    }
}