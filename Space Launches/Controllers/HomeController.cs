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

        [AsyncTimeout(8000)]
        [HandleError(ExceptionType = typeof(TimeoutException), View = "Error")]
        public async Task<ActionResult> Index()
        {
            ViewBag.SyncOrAsync = "Asynchronous";
            ViewBag.Title = "Upcoming Launch List";
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

        /*
        public ActionResult Error(Exception exception)
        {
            var code = (exception is HttpException) ? (exception as HttpException).GetHttpCode() : 500;

//            var message = String.Format(Strings_Errors.Error500, code);
            var message = String.Format("Code {0}:", code);
            var subtitle = "";

//            var appSpecific = (exception is YourApplicationBaseException);
//            if (code == 404)
//                message = Strings_Errors.Error404;

            if (code == 500)
            {
//                if (appSpecific)
//                    message = exception.Message;
//                else
                    subtitle = exception.Message;
            }

//            var model = new ErrorViewModel(message, appSpecific)
//            {
//                ErrorOccurred = { StatusCode = code, Subtitle = subtitle }
//            };

            ViewBag.message = message;
            ViewBag.subtitle = subtitle;
            return View("Error");
        }
        */
    }
}