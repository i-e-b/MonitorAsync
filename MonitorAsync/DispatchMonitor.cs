using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace MonitorAsync
{
    public static class DispatchMonitor
    {
        // This keeps a list of incoming jobs, and can continue the tasks it has when external data comes in
        // ReSharper disable once AnnotationRedundancyAtValueType
        [NotNull, ItemNotNull]private static readonly Dictionary<int, TaskCompletionSource<object>> Threads = new Dictionary<int,TaskCompletionSource<object>>();

        /// <summary>
        /// Simulates sending a message to another system, and waiting for a result
        /// </summary>
        /// <param name="correlationId">Reply ID for the other system to use</param>
        /// <param name="inputData">Message data to send</param>
        /// <returns>A task that will contain the result</returns>
        [NotNull]public static Task<object> SendAndWait(int correlationId, object inputData)
        {
            Console.WriteLine("        (SendAndWait)");
            var tcs = new TaskCompletionSource<object>();
            Threads.Add(correlationId, tcs);
            return tcs.Task ?? throw new InvalidOperationException();
        }

        /// <summary>
        /// This is how you can make an await throw an exception
        /// </summary>
        public static void FailResponse(int correlationId)
        {
            Threads[correlationId].SetException(new Exception("This request never got a response"));
        }

        /// <summary>
        /// Simulates the response
        /// </summary>
        /// <param name="correlationId"></param>
        /// <param name="responseData"></param>
        public static void HandleResponse(int correlationId, object responseData)
        {
            try
            {
                Console.WriteLine("        (HandleResponse)");
                Threads[correlationId].SetResult(responseData); // <--- this will cause the async method to continue
                Console.WriteLine("Program exited");            // <--- this happens when the async method returns
            }
            catch
            {
                Console.WriteLine(" * BOOM *)");
                //ignore
            }
        }
    }
}