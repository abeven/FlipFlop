using System;
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

            Get["/features"] = parameters => Response.AsJson(Program.State);

            Post["/features"] = p =>
            {
                FeatureResource featureResource = this.Bind();
                featureResource.Id = Guid.NewGuid().ToString();

                Program.State.Add(featureResource);

                return Response.AsJson(featureResource);
            };

            Put["/features/{id}"] = _ =>
            {
                FeatureResource patch = this.Bind();

                var ft = Program.State.Find(x => x.Id == _.id);
                ft.On = patch.On;

                return Response.AsJson(HttpStatusCode.OK);
            };
        }
    }
}