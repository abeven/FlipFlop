using System;
using FlipFlop;
using FlipFlop.Dashboard.Web;
using Nancy.Hosting.Self;
using NanoCluster;

namespace FlipFlop.Dashboard
{
    class Program
    {
        public static DistributedFeaturesRegistry FeaturesRegistry = new DistributedFeaturesRegistry();
        public static NanoClusterEngine cls;

        static void Main(string[] args)
        {
            var nancyHost = new NancyHost(new Uri("http://localhost:8081/"),new NancyBootstrap(),new HostConfiguration() { AllowChunkedEncoding = false });
            nancyHost.Start();

            using (cls = new NanoClusterEngine(cfg =>
            {
                cfg.DiscoverByClusterKey("FlipFlop");
                cfg.DistributedTransactions = FeaturesRegistry;
            }))
            {
                Console.WriteLine("Cluster stable " + cls.IsLeadingProcess);

                cls.Send(new AddFeatureCommand()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Show users", 
                    Enabled = true
                });

                Console.ReadKey();
            }

            nancyHost.Stop();
        }
    }
}
