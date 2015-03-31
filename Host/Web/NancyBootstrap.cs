using System;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.Conventions;
using Nancy.Json;
using Nancy.TinyIoc;

namespace Host.Web
{
    public class NancyBootstrap : DefaultNancyBootstrapper
    {
        protected override void ConfigureConventions(NancyConventions cfg)
        {
            base.ConfigureConventions(cfg);

            cfg.StaticContentsConventions.Add(StaticContentConventionBuilder.AddDirectory("/css", @"/Content/css"));
            cfg.StaticContentsConventions.Add(StaticContentConventionBuilder.AddDirectory("/js", @"/Content/js"));
            cfg.StaticContentsConventions.Add(StaticContentConventionBuilder.AddDirectory("Content"));

            JsonSettings.MaxJsonLength = Int32.MaxValue;
        }
        
        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            pipelines.OnError += (ctx, ex) =>
            {
                Console.WriteLine(ex);
                return null;
            };
        }
    }
}