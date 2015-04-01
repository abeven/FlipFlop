using System;
using FlipFlop;
using Nancy;
using Nancy.ModelBinding;

namespace FlipFlop.Dashboard.Web
{
    public class WebEndpoint : NancyModule
    {
        public WebEndpoint()
        {
            Get[@"/"] = parameters => Response.AsFile("Content/index.html", "text/html");

            Get["/features"] = parameters => Response.AsJson(Program.FeaturesRegistry.All());

            Post["/features"] = p =>
            {
                Feature featureResource = this.Bind();
                featureResource.Id = Guid.NewGuid().ToString();

                Program.cls.Send(new AddFeatureCommand()
                {
                    Id = featureResource.Id,
                    Name = featureResource.Name, 
                    Enabled = featureResource.Enabled
                });

                return Response.AsJson(featureResource);
            };

            Put["/features/{id}"] = _ =>
            {
                Feature patch = this.Bind();

                Program.cls.Send(new FlipFeatureCommand() { Id = patch.Id });

                return Response.AsJson(HttpStatusCode.OK);
            };
        }
    }
}