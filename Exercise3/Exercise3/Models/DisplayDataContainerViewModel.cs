using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Exercise3.Models.Interface;

namespace Exercise3.Models
{
    public class DisplayDataContainerViewModel
    {
        public string Param1 { get; set; }
        public string Param2 { get; set; }
        public int Param3 { get; set; }
        public IFlightSimulatorsModel model;
        public DisplayDataContainerViewModel(string p1, string p2, int p3, IFlightSimulatorsModel m)
        {
            Param1 = p1;
            Param2 = p2;
            Param3 = p3;
            model = m;
        }
    }
}