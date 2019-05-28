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
        
        public static bool isIP(string str)
        {
            IPAddress address;
            return IPAddress.TryParse(str, out address);
        }
        public static bool isFile(string str)
        {
            return File.Exists(str);
        }
        public static bool isNum(string str)
        {
            double res;
            return double.TryParse(str, out res);
        }
        public static bool isInt(string str)
        {
            int res;
            return int.TryParse(str, out res);
        }
    }
}