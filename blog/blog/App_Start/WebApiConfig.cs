using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using JsonPatch;

namespace blog
{
    public static class WebApiConfig
    {

        public static void Register(HttpConfiguration config)
        {
            HttpConfiguration confi = new HttpConfiguration();
            config.Filters.Add(new AuthorizeAttribute());
            config.Formatters.XmlFormatter.SupportedMediaTypes.Remove(appXmlType);
            config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            config.Formatters.Add(new JsonPatch.Formatting.JsonPatchFormatter());
            config.EnableSystemDiagnosticsTracing();
        }


        public static System.Net.Http.Headers.MediaTypeHeaderValue appXmlType { get; set; }
    }
}
