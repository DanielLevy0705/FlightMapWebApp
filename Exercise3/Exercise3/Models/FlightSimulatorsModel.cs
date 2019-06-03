using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;
using System.Threading;
using Exercise3.Models.Interface;
using System.Web.Script.Serialization;
using System.IO;
using System.Web;

namespace Exercise3.Models
{
    
    //A class that will function as a model.
    public class FlightSimulatorsModel : IFlightSimulatorsModel
    {
        #region Singleton
        private static IFlightSimulatorsModel m_Instance = null;
        public static IFlightSimulatorsModel Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    m_Instance = new FlightSimulatorsModel();
                }
                return m_Instance;
            }

        }
        #endregion
        IClientsManager connections;
        Dictionary<string, string> path;
        private bool active;
               
        //constructor
        public FlightSimulatorsModel(int timeout = 2000)
        {
            active = false;
            path = new Dictionary<string, string>();
            path["Lon"] = "/position/longitude-deg";
            path["Lat"] = "/position/latitude-deg";
            path["Rudder"] = "/controls/flight/rudder";
            path["Throttle"] = "/controls/engines/current-engine/throttle";
        }
        public void Start(ITelnetClientFactory factory, int timeout = 3000)
        {
            if (!active)
            {
                connections = new ExpirableClientsManager(timeout);
                connections.Start(factory);
                active = true;
            }
        }
        public void Stop()
        {
            active = false;
            connections.Stop();
        }
        private double ParseInsureSimulatorValue(string res, string path)
        {
            string[] words = res.Split(new[] {' ','=','(','\'',')' }).Where(x => !string.IsNullOrEmpty(x) && !x.Equals("/>")).ToArray(); 
            if (words.Length > 1)
            {
                bool cond1 = path.Substring(1).Equals(words[0]) || path.Equals(words[0]);
                bool cond2 = double.TryParse(words[1], out double value);
                if (cond1 && cond2) 
                    return value;
            }
            throw new Exception("Error: wrong value from simulator");
        }
        public double getValue(Tuple<string, int>address, string val)
        {
            if (!path.ContainsKey(val))
                throw new Exception("Error: getValue() error - val not exist");
            
            connections.Lock(address);
            connections.Write(address, "get " + path[val]);
            double res = ParseInsureSimulatorValue(connections.Read(address), path[val]);
            connections.Unlock(address);
            return res;
        }
        public Dictionary<string, double> GetData(string ip, int port, string[] vals)
        {
            var address = new Tuple<string, int>(ip, port);
            var res = new Dictionary<string, double>();
            foreach( var val in vals)
            {
                res[val] = getValue(address, val);
            }
            return res;
        }
        public const string SCENARIO_FILE = "~/App_Data/{0}.txt";           // The Path of the Secnario
                                                                            // changes: removed freq and duration as parameters.
        public Dictionary<string, double> SaveData(string ip, int port, string file, string[] vals)
        {
            string path = HttpContext.Current.Server.MapPath(String.Format(SCENARIO_FILE, file));
            var data = GetData(ip, port, vals);
            using (System.IO.StreamWriter newFile = new System.IO.StreamWriter(path))
            {
                newFile.WriteLine(String.Format("{0},{1},{2},{3}",data["Lon"],
                    data["Lat"],data["Rudder"],data["Throttle"]));
            }
            /////////////////////////////////////////////////
            //int size = duration * freq;
            //var data = new Dictionary<string, double>[size];
            //new Thread(() =>
            //{
            //  for (int i = 0; i < size; i ++)
            //{
            //  data[i] = GetData(ip, port, vals);
            //Thread.Sleep(1000 / freq);
            //}
            ///////////////////////////////////////////////////

            //var json = new JavaScriptSerializer().Serialize(data);
            //string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), file);
            //File.WriteAllText(filePath, json);
            return data;

            //}).Start();
        }
                                                                                  //changes in load: returning string[] instead of dictionary<string,double>[].
        public Dictionary<string,double>[] LoadData(string file)
        {
            string path = HttpContext.Current.Server.MapPath(String.Format(SCENARIO_FILE, file));
            string[] lines = System.IO.File.ReadAllLines(path);
            Dictionary<string,double>[] dict = new Dictionary<string,double>[lines.Length];
            for(int i=0;i<lines.Length;i++)
            {
                string[] line = lines[i].Split(',');
                dict[i]["Lon"] = double.Parse(line[0]);
                dict[i]["Lat"] = double.Parse(line[1]);
                dict[i]["Rudder"] = double.Parse(line[2]);
                dict[i]["Throttle"] = double.Parse(line[3]);
            }
            //string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), file);
            //var json = File.ReadAllText(filePath);
            //return new JavaScriptSerializer().Deserialize<Dictionary<string, double>[]>(json);
            return dict;
        }

    }
}
