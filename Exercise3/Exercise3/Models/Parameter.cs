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
        
        public bool isIP(string str)
        {
            IPAddress address;
            return IPAddress.TryParse(str, out address);
        }
        public bool isFile(string str)
        {
            return File.Exists(str);
        }
        public bool isNum(string str)
        {
            double res;
            return double.TryParse(str, out res);
        }
        public bool isInt(string str)
        {
            int res;
            return int.TryParse(str, out res);
        }
    }
}