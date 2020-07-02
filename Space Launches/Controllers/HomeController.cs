using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Space_Launches.Models;

namespace Space_Launches.Controllers
{
    public class HomeController : Controller
    {
        // instantiate httpclient within this class just once to prevent
        // exhaustion of sockets under heavy loads; see MS docs on HttpClient

        private static readonly LaunchLibraryClient _LaunchApiClient;
        
        static HomeController()
        {
            _LaunchApiClient = new LaunchLibraryClient();
        }

        // some help from https://github.com/RickAndMSFT/Async-ASP.NET/
        // 

        public async Task<ActionResult> GetLaunchCollectionAsync()
        {
            ViewBag.SyncOrAsync = "Asynchronous";
            return View("Launches", await _LaunchApiClient.GetLaunchCollectionAsync());
        }

        /*
        [AsyncTimeout(50)]
        [HandleError(ExceptionType = typeof(TimeoutException), View = "TimeoutError")]
        public async Task<ActionResult> CancelGetLauncheCollectionAsync(CancellationToken cancellationToken)
        {
            ViewBag.SyncOrAsync = "Asynchronous";

            var launchesTask = _LaunchApiClient.GetLaunchCollectionAsync(cancellationToken);

            return View("Launches", await _LaunchApiClient.GetLaunchCollectionAsync(cancellationToken));
        }
        */

            /*
        public async Task<ActionResult> Index()
        {
            ViewBag.SyncOrAsync = "Asynchronous";
            ViewBag.Title = "Asynchronous Index";
            ViewResult result = (ViewResult) await GetLaunchCollectionAsync();
            LaunchCollectionModel model = (LaunchCollectionModel) result.Model;
            return View(model);
        }
        */

        [AsyncTimeout(8000)]
        [HandleError(ExceptionType = typeof(TimeoutException), View = "Error")]
        public async Task<ActionResult> Index()
        {
            ViewBag.SyncOrAsync = "Asynchronous";
            ViewBag.Title = "Launch List";
            ViewResult result = (ViewResult)await GetLaunchCollectionAsync();
            LaunchCollectionModel model = (LaunchCollectionModel)result.Model;
            return View(model);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            //base.OnException(filterContext);

            filterContext.ExceptionHandled = true;

            // TODO:_Logger.Error(filterContext.Exception);

            //Redirect or return a view, but not both.
            //            filterContext.Result = RedirectToAction("Index", "ErrorHandler");
            // OR 

            ViewResult vr = new ViewResult();
            vr.ViewName = "~/Views/Error/Error.cshtml";
            vr.ViewBag.Title = "Probably Timeout";
            filterContext.Result = vr;
        }
    }
}