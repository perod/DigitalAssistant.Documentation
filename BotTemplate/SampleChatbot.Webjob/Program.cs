using Microsoft.Azure.WebJobs;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SampleChatbot.Webjob
{
    public class Program
    {
        static void Main()
        {
            var host = new JobHost();
            // note: JobHost.Call automatically injects its CancellationToken
            host.Call(typeof(Program).GetMethod("StartAgent"));

            host.RunAndBlock();
            // note: RunAndBlock returns immediately when the token is cancelled (e.g. because of a Webjob restart)

            Console.WriteLine("Delaying shutdown by 5 seconds to allow a graceful shutdown of the chatbot.");
            Task.Delay(5000).Wait();    // note: do not use the cancellation token overload
        }

        [NoAutomaticTrigger]
        public void StartAgent(CancellationToken cancellationToken)
        {
            Agent.Start(cancellationToken);
        }
    }
}
