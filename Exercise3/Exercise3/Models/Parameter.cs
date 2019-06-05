using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.IO;

namespace Exercise3.Models
{
    public class Parameter
    {
        
        public int isIP(string str)
        {
            IPAddress address;
            return IPAddress.TryParse(str, out address)?1:0;
        }
        public int isFile(string str)
        {
            string path = HttpContext.Current.Server.MapPath(String.Format(
                FlightSimulatorsModel.SCENARIO_FILE, str));
            return File.Exists(path)?1:0;
        }
        public int isNum(string str)
        {
            double res;
            return double.TryParse(str, out res)?1:0;
        }
        public int isInt(string str)
        {
            int res;
            return int.TryParse(str, out res)?1:0;
        }
    }
}