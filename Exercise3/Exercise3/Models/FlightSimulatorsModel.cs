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
        public void SaveData(string ip, int port, int freq, int duration, string file, string[] vals)
        {
            int size = duration * freq;
            var data = new Dictionary<string, double>[size];
            new Thread(() =>
            {
                for (int i = 0; i < size; i ++)
                {
                    data[i] = GetData(ip, port, vals);
                    Thread.Sleep(1000 / freq);
                }

                var json = new JavaScriptSerializer().Serialize(data);
                string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), file);
                File.WriteAllText(filePath, json);

   
            }).Start();
        }
        public Dictionary<string, double>[] LoadData(string file)
        {
            string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), file);
            var json = File.ReadAllText(filePath);
            return new JavaScriptSerializer().Deserialize<Dictionary<string, double>[]>(json);
        }

    }
}
