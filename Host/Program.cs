using System;
using System.Collections.Generic;
using Host.Switches;
using Host.Web;
using Nancy.Hosting.Self;
using NanoCluster;

namespace Host
{
    class Program
    {
        public static DistributedSwitchRegistry SwitchRegistry = new DistributedSwitchRegistry();
        public static NanoClusterEngine cls;

        static void Main(string[] args)
        {
            var nancyHost = new NancyHost(new Uri("http://localhost:8081/"),new NancyBootstrap(),new HostConfiguration() { AllowChunkedEncoding = false });
            nancyHost.Start();

            using (cls = new NanoClusterEngine(cfg =>
            {
                cfg.DiscoverByClusterKey("FlipFlop");
                cfg.DistributedTransactions = SwitchRegistry;
            }))
            {
                Console.WriteLine("Cluster stable " + cls.IsLeadingProcess);

                cls.Send(new AddSwithCommand()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Show users", 
                    State = true
                });

                Console.ReadKey();
            }

            nancyHost.Stop();
        }
    }
}
