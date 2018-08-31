using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebGaraioLogParser
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new
                { controller = "Upload", action = "UploadFile", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "MergeFilesUploadedIntoSingleFile",
                url: "{controller}/{action}/{id}",
                defaults: new
                { controller = "Upload", action = "MergeFilesUploadedIntoSingleFile", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "GetDataIntoJSON",
                url: "{controller}/{action}/{id}",
                defaults: new
                { controller = "Upload", action = "GetDataIntoJSON", id = UrlParameter.Optional }
            );
        }
    }
}
