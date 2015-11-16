using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Martin
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "BlackDoor",
                url: "b",
                defaults: new { controller = "Home", action = "Login" }
            );

            routes.MapRoute(
                name: "FrontAlbum",
                url: "{albumName}/{albumId}",
                defaults: new { controller = "Home", action = "GetSlideByName"},
                constraints: new RouteValueDictionary { { "albumId", @"\d{1,6}" } }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}