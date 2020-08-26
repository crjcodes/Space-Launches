using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Space_Launches.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult Index()
        {
            //Server.ClearError();
            //Response.TrySkipIisCustomErrors = true;
            ViewBag.Title = "Unknown Error";
            return View();
        }

        public ActionResult NotFound()
        {
            //Server.ClearError();
            //Response.TrySkipIisCustomErrors = true;
            ViewBag.Title = "Invalid Request";
            return View();
        }
    }
}