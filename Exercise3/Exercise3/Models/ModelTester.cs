using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Script.Serialization;
using Exercise3.Models.Interface;

namespace Exercise3.Models
{
    public class ModelTester
    {
        public static void Test()
        {
            var model = FlightSimulatorsModel.Instance;
            GetDataTests(model);
            SaveDataTests(model);
            LoadDataTests(model);


        }
        #region get-data-tests
        public static void GetDataTests(IFlightSimulatorsModel model)
        {
            TestConnectionAndValues(model);
            TestConnectionAndValues(model);
            TestSimulatorFewThreads(model);
            TestTwoSimulators(model);
            TestTwoSimulatorsThreads(model);
        }
        public static void GetDataTest(IFlightSimulatorsModel model, string ip, int port, string[] vals,string test)
        {
            var res = model.GetData(ip, port, vals);
            System.Diagnostics.Debug.WriteLine(test);
            foreach (var pair in res)
            {
                System.Diagnostics.Debug.WriteLine($"{pair.Key} = {pair.Value}");
            }
        }
        public static void TestConnectionAndValues(IFlightSimulatorsModel model)
        {
            GetDataTest(model, "127.0.0.1", 5402, new[] { "Lon", "Lat" }, "Test Connection and Values Sampling results:");
        }
        public static void TestTwoSimulators(IFlightSimulatorsModel model)
        {
            GetDataTest(model, "127.0.0.1", 5401, new[] { "Lon", "Lat" }, "Test two clients at once 1/2:");
            GetDataTest(model, "127.0.0.1", 5402, new[] { "Lon", "Lat" }, "Test two clients at once 2/2:");
        }
        public static void TestTwoSimulatorsThreads(IFlightSimulatorsModel model)
        {
            new Thread(() =>
            {
                GetDataTest(model, "127.0.0.1", 5401, new[] { "Lon", "Lat" }, "Test Two simulators thread1:");
            }).Start();

            new Thread(() =>
            {
                GetDataTest(model, "127.0.0.1", 5402, new[] { "Lon", "Lat" }, "Test two simulators thread2:");
            }).Start();
        }
        public static void TestSimulatorFewThreads(IFlightSimulatorsModel model)
        {
            new Thread(() =>
            {
                GetDataTest(model, "127.0.0.1", 5401, new[] { "Lon", "Lat" }, "Test Same simulator thread1:");
            }).Start();
            new Thread(() =>
            {
                GetDataTest(model, "127.0.0.1", 5401, new[] { "Lon", "Lat" }, "Test Same simulator thread2:");
            }).Start();
            new Thread(() =>
            {
                GetDataTest(model, "127.0.0.1", 5401, new[] { "Lon", "Lat" }, "Test Same simulator thread3:");
            }).Start();


        }
        #endregion

        #region save-data-tests
        public static void SaveDataTests(IFlightSimulatorsModel model)
        {
            SaveDataTest(model, "127.0.0.1", 5402, 4, 10, "saveTest1.txt", new[] { "Lon", "Lat" }, "simple save test:");
        }
        public static void SaveDataTest(IFlightSimulatorsModel model, string ip, int port, int freq
                                       , int duration, string file,string[] vals, string test)
        {
            System.Diagnostics.Debug.WriteLine(test);
            model.SaveData(ip, port,freq,duration, file, vals);
            System.Diagnostics.Debug.WriteLine("saved.");
        }
        #endregion
        #region load-data-tests
        public static void LoadDataTests(IFlightSimulatorsModel model)
        {
            LoadDataTest(model, "saveTest1.txt",  "simple load test:");
        }
        public static void LoadDataTest(IFlightSimulatorsModel model,string file, string test)
        {
            System.Diagnostics.Debug.WriteLine(test);
            var data = model.LoadData(file);
            System.Diagnostics.Debug.WriteLine(new JavaScriptSerializer().Serialize(data));
        }
        #endregion
    }
}