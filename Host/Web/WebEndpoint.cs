using System;
using Host.Switches;
using Nancy;
using Nancy.ModelBinding;

namespace Host.Web
{
    public class FeatureResource
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool On { get; set; }
    }

    public class WebEndpoint : NancyModule
    {
        public WebEndpoint()
        {
            Get[@"/"] = parameters => Response.AsFile("Content/index.html", "text/html");

            Get["/features"] = parameters => Response.AsJson(Program.SwitchRegistry.All());

            Post["/features"] = p =>
            {
                FeatureResource featureResource = this.Bind();
                featureResource.Id = Guid.NewGuid().ToString();

                Program.cls.Send(new AddSwithCommand()
                {
                    Id = featureResource.Id,
                    Name = featureResource.Name, 
                    State = featureResource.On
                });

                return Response.AsJson(featureResource);
            };

            Put["/features/{id}"] = _ =>
            {
                FeatureResource patch = this.Bind();

                Program.cls.Send(new FlipSwitchCommand() { Id = patch.Id });


                return Response.AsJson(HttpStatusCode.OK);
            };
        }
    }
}