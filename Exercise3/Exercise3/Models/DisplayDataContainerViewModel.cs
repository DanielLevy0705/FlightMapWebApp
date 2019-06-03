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
        public int Freq { get; set; }
        public double Duration { get; set; }
        public string FileName { get; set; }
        public Parameter paramCheck;

        public IFlightSimulatorsModel model;
        public DisplayDataContainerViewModel(string p1, string p2,
            int p3, string file,double dur,Parameter pc, IFlightSimulatorsModel m)
        {
            Param1 = p1;
            Param2 = p2;
            Freq = p3;
            Duration = dur;
            FileName = file;
            paramCheck = pc;
            model = m;
        }
    }
}