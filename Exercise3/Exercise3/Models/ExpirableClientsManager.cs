using Exercise3.Models.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading;

namespace Exercise3.Models
{
    public class ExpirableClientsManager : IClientsManager
    {
        public class ManagableClient
        {
            public Mutex mutex;
            public ITelnetClient Client { set; get;}
            public DateTime Expirey { set; get; }
            public ManagableClient(ITelnetClient client, double timeout)
            {
                mutex = new Mutex(false);
                Client = client;
                setTimeout(timeout);
            }
            public void setTimeout(double timeout)
            {
                Expirey = DateTime.Now.AddMilliseconds(timeout);
            }
            public bool Expired
            {
                get { return Expirey < DateTime.Now; }
            }
        }
        int Timeout { set; get; }
        Dictionary<Tuple<String, int>, ManagableClient> clients;
        Mutex mutex;
        bool active;
        ITelnetClientFactory clientsFactory;
        
        public ExpirableClientsManager(int timeout)
        {
            mutex = new Mutex(false);
            clients = new Dictionary<Tuple<string, int>, ManagableClient>();
            Timeout = timeout;
            active = false;
        }
        public void addClient(Tuple<String, int> key)
        {
            mutex.WaitOne();
            try
            {
                ITelnetClient client = clientsFactory.New();
                client.Connect(key.Item1, key.Item2);
                clients[key] = new ManagableClient(client, Timeout);
            }
            finally
            {
                mutex.ReleaseMutex();
            }
        }
        public void Add(Tuple<String, int> key)
        {
            if(active)
                checkClient(key);
        }
        public void Start(ITelnetClientFactory factory)
        {
            clientsFactory = factory;
            active = true; 
            new Thread(() =>
            {
                while (active)
                {
                    Thread.Sleep(Timeout);
                    //delete from the servers list every server which is not necesarry;
                    var expireds = new List<Tuple<string, int>>();
                    
                    foreach (var key in clients.Keys)
                    {
                        if (clients[key].Expired)
                            expireds.Add(key);
                        else
                            clients[key].setTimeout(Timeout);
                    }
                    foreach (var expired in expireds)
                    {
                        mutex.WaitOne();
                        try
                        {
                            var client = clients[expired];
                            if (client.Expired)
                            {
                                client.Client.Disconnect();
                                clients.Remove(expired);
                            }
                        }
                        finally
                        {
                            mutex.ReleaseMutex();
                        }
                        
                    }
                }
                

            }).Start();
        }
        public void Stop()
        {
            mutex.WaitOne();
            active = false;
            try
            {
                foreach (var pair in clients)
                {
                    pair.Value.Client.Disconnect();
                }
                clients.Clear();
            }
            finally
            {
                mutex.ReleaseMutex();
            }
    }
        private void checkClient(Tuple<String, int> key)
        {
            mutex.WaitOne();
            if (clients.TryGetValue(key, out var exclient))
            {
                exclient.setTimeout(Timeout);
                mutex.ReleaseMutex();
            }
            else
            {
                mutex.ReleaseMutex();
                addClient(key);
            }

        }
        public string Read(Tuple<String, int> key)
        {
            if (active)
            {
                checkClient(key);
                return clients[key].Client.Read();
            }
            return null;
        }
        public void Write(Tuple<String, int> key, string msg)
        {
            if (active)
            {
                checkClient(key);
                clients[key].Client.Write(msg);
            } 
        }
        public void Lock(Tuple<String, int> key)
        {
            if (active)
            {
                  checkClient(key);
                clients[key].mutex.WaitOne();
                    
            }
        }
        public void Unlock(Tuple<String, int> key)
        {
            if (active)
            {
                checkClient(key);
                try
                {
                    clients[key].mutex.ReleaseMutex();
                } catch(Exception e)
                {
                    System.Diagnostics.Debug.WriteLine("error on mutex release");
                }
                
            }
        }
    }
}