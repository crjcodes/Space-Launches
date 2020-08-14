using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Space_Launches
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        /*
                void Application_Error(object sender, EventArgs e)
                {
                    var error = Server.GetLastError();
                    var code = (error is HttpException) ? (error as HttpException).GetHttpCode() : 500;

                    if (code != 404)
                    {
                        // TBD: Options: email, log, etc
                    }

                    //Response.Clear();
                    //Server.ClearError();

                    // TBD: Options: Redirect or switch views
                    var httpContext = ((HttpApplication)sender).Context;
                    httpContext.Response.Clear();
                    httpContext.ClearError();
                    httpContext.Response.TrySkipIisCustomErrors = true;

                    InvokeErrorAction(httpContext, error);
                }

                void InvokeErrorAction(HttpContext httpContext, Exception exception)
                {
                    var routeData = new RouteData();
                    routeData.Values["controller"] = "home";
                    routeData.Values["action"] = "error";
                    routeData.Values["exception"] = exception;

                    using (var controller = new Space_Launches.Controllers.HomeController())
                    {
                        ((IController)controller).Execute(
                            new RequestContext(new HttpContextWrapper(httpContext), routeData));
                    }

                }
        */
    }
}
