using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace MyMusicStore
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            Configure(config);
            config.Routes.MapHttpRoute(
                name: "Albums",
                routeTemplate: "api/albums/{name}",
                defaults: new { controller="albums", name = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "Songs",
                routeTemplate: "api/albums/{albumId}/songs/{id}",
                defaults: new { controller = "songs", id = RouteParameter.Optional }
            );

            config.EnableSystemDiagnosticsTracing();
        }

        private static void Configure(HttpConfiguration config)
        {
            config.Formatters.Remove(config.Formatters.XmlFormatter);
            var json = config.Formatters.JsonFormatter;
            json.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
            json.SerializerSettings.ContractResolver =
                new CamelCasePropertyNamesContractResolver();
        }
    }
}
