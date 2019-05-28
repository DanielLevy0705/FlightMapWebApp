using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercise3.Models.Interface
{
    interface IClientsManager
    {
        void Start(ITelnetClientFactory factory);
        void Stop();
        void Add(Tuple<String, int> address);
        string Read(Tuple<String, int> address);
        void Write(Tuple<String, int> address, string msg);
        void Lock(Tuple<String, int> address);
        void Unlock(Tuple<String, int> address);
    }
}
