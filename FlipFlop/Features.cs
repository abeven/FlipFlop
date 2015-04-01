using System;
using NanoCluster;

namespace FlipFlop
{
    public class Features
    {
        public static DistributedFeaturesRegistry FeaturesRegistry = new DistributedFeaturesRegistry();
        static readonly NanoClusterEngine Cls;

        static Features()
        {
            Cls = new NanoClusterEngine(cfg =>
            {
                cfg.DiscoverByClusterKey("FlipFlop");
                cfg.DistributedTransactions = FeaturesRegistry;
            });
        }

        public static void Initialize()
        {
            Console.WriteLine("Cluster stable " + Cls.IsLeadingProcess);
        }

        public static bool IsEnabled(string feature)
        {
            return FeaturesRegistry.Get(feature);
        }
    }
}