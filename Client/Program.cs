using System;
using System.Net;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;

namespace Client
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine(args[0]);
            Console.WriteLine(args[1]);
            
            var method = args[0] == "blocking" ? "getworkresultblocking" : "getworkresult";
            var iterations = int.Parse(args[1]) + 1;

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            var result = Parallel.For(1, iterations, id =>
            {
                using (var client = new WebClient())
                {
                    Console.WriteLine($"{DateTime.Now.ToString("hh:mm:ss")} # Client {id} starting a request");
                    
                    var response = client.DownloadString($"https://localhost:5001/work/{method}?clientId={id}");
                    
                    Console.WriteLine($"{DateTime.Now.ToString("hh:mm:ss")} # Client {id} gets result: {response}");
                }
            });

            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;

            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);
            Console.WriteLine("RunTime " + elapsedTime);
        }
    }
}