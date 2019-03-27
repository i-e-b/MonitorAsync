using System;
using System.Threading;
using System.Threading.Tasks;

namespace MonitorAsync
{
    class Program
    {
        static async Task Main()
        {
            var correlationID = 123;
            var inputData = new object();
            BackgroundWorker(correlationID);

            Console.WriteLine("Calling...");

            // this is the general requirement
            var result = await DispatchMonitor.SendAndWait(correlationID, inputData);
            
            Console.WriteLine("...done");
            Console.WriteLine(result);
            Console.WriteLine("Press [enter]");
            Console.ReadLine();
        }

        // there is a background thread polling for completions from external sources
        private static void BackgroundWorker(int correlationID)
        {
            var t = new Thread(() =>
            {
                Console.WriteLine("    [Waiting]");
                for (int i = 0; i < 10; i++)
                {
                    Thread.Sleep(300);
                    Console.Write(".");
                }

                Console.WriteLine();
                Console.WriteLine("    [Triggering Dispatch]");
                DispatchMonitor.HandleResponse(correlationID, new {IAm = "The result"});
                Console.WriteLine("    [Dispatch done]");
            });
            t.Start();
        }
    }
}
