using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Exercise3.Models;
using Exercise3.Models.Interface;
using System.Net.Sockets;
using System.Net;
using System.Web.Script.Serialization;

namespace Exercise3.Controllers
{
    public class HomeController : Controller
    {


        // GET: /diaplay/
        public ActionResult Display(string param1, string param2, int freq = -1,
            double duration =-1, string fileName = "")
        {

            IFlightSimulatorsModel model = FlightSimulatorsModel.Instance; //remember to create the first instance (there is no settings this time)
            var vm = new DisplayDataContainerViewModel(param1, param2, freq ,fileName ,duration, new Parameter(), model);
            //if (Parameter.isIP(param1) && Parameter.isInt(param2))
                return View(vm);
            

            //if (Parameter.isFile(param1) && Parameter.isNum(param2))
              //  return Content("file");
            //return Content("Wrong url buddy.");
        }
        public ActionResult Save(string ip, int port, int freq, double duration, string file)
        {

            //IFlightSimulatorModel model = FlightSimulatorModel.Instance; //remember to create the first instance (there is no settings this time)

            return RedirectToAction("Display",new {param1 = ip,
                param2 = port.ToString(),freq = freq,
                duration = duration,fileName = file });
            //return View(model);
        }
        public ActionResult Location(string ip, int port, string file =  "")
        {

            if(string.IsNullOrEmpty(file))
            {
                return Content(new JavaScriptSerializer().Serialize(FlightSimulatorsModel.Instance.GetData(ip, port, new[] { "Lon", "Lat" })));
            }
            else
            {
                return Content(new JavaScriptSerializer().Serialize(FlightSimulatorsModel.Instance.SaveData(ip, port, file, new[] { "Lon", "Lat", "Rudder", "Throttle" })));
            }
        }
        public ActionResult Load(string file)
        {

            if (!string.IsNullOrEmpty(file))
            {
                return Content(new JavaScriptSerializer().Serialize(FlightSimulatorsModel.Instance.LoadData(file, new[] { "Lon", "Lat", "Rudder", "Throttle" })));
            }
            else
            {
                return Content("give file");
            }
        }
    }
}