using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Exercise3.Models.Interface
{
    //A FlightSimulatorModel interface.
    public interface IFlightSimulatorModel 
    {
        double Lon { get; set; }
        double Lat { get; set; }
        void ConnectInfoServer(string FlightServerIP, int FlightInfoPort);
        void DisconnectInfoServer();
        void StartInfoServer();
        void StopInfoServer();
        IFlightSimulatorModel SetTelnetClient(ITelnetClient telnetClient);
        void NotifyPropertySet(object s, PropertyChangedEventArgs e);
    }
}
