using System;
using System.Collections.Generic;
using Host.Web;
using Nancy.Hosting.Self;

namespace Host
{
    class Program
    {
        public static List<FeatureResource> State = new List<FeatureResource>() {
            new FeatureResource() {Id = "1", Name = "Allow users in", On = false},
            new FeatureResource() {Id = "2", Name = "Show promo", On = true},
            new FeatureResource() {Id = "3", Name = "Offer discounts", On = false},
            new FeatureResource() {Id = "4", Name = "Bypass authentication", On = false},
        };

        static void Main(string[] args)
        {
            var nancyHost = new NancyHost(
                new Uri("http://localhost:8081/"),
                new NancyBootstrap(),
                new HostConfiguration() { AllowChunkedEncoding = false }
                );
            nancyHost.Start();

            Console.ReadKey();

            nancyHost.Stop();
        }
    }

    
}
