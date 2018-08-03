using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace BabyStore
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");         
            routes.MapRoute(
                name: "ProductsCreate",
                url: "products/create",
                defaults: new { controller = "Products", action = "Create"}
            );
            routes.MapRoute(
                name: "ProductsbyCategorybyPage",
                url: "products/{category}/page{page}",
                defaults:  new { controller = "Products", action = "Index"}
            );
            routes.MapRoute(
                name: "productsbyPage",
                url: "products/page{page}",
                defaults: new { controller = "Products", action = "Index"}
            );
            routes.MapRoute(
                name: "ProductsbyCategory",
                url: "products/{category}",
                defaults: new { controller = "Products", action = "Index"}
            );
            routes.MapRoute(
                name: "ProductsIndex",
                url: "products",
                defaults: new { controller = "Products", action = "Index"}
            );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
