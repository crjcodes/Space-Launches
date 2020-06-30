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
        public async Task<ActionResult> Index()
        {
            ViewBag.SyncOrAsync = "Asynchronous";
            ViewBag.Title = "Asynchronous Index";
            ViewResult result = (ViewResult) await GetLaunchCollectionAsync();
            LaunchCollectionModel model = (LaunchCollectionModel) result.Model;
            return View(model);
        }

        /*
        public ActionResult Index()
        {
            ViewBag.Title = "Empty Index";
            return View();
        }
        */

        /*
                [HttpGet]
                [AsyncTimeout(8000)]
                [HandleError(ExceptionType = typeof(TimeoutException), View = "TimedOut")]
                public async Task<ActionResult> Index(CancellationToken cancellationToken)
                {
                    LaunchCollectionModel workingLaunchCollection = new LaunchCollectionModel();

                    workingLaunchCollection = await _LaunchApiClient.GetLaunchCollectionAsync("launch",cancellationToken);

                    return View(workingLaunchCollection);
                }
        */

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
    }
}