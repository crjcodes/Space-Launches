using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Policy;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.ModelBinding;
using System.Web.UI.WebControls;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;

namespace Space_Launches.Models
{
    public class LaunchLibraryClient
    {
        private static readonly string ApiBaseURL = "https://launchlibrary.net/1.4/";
        private static readonly HttpClient _LaunchHttpClient;
        static LaunchLibraryClient()
        {
            _LaunchHttpClient = new HttpClient();
            _LaunchHttpClient.BaseAddress = new Uri(ApiBaseURL);
            _LaunchHttpClient.DefaultRequestHeaders.Accept.Clear();
            _LaunchHttpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        // TODO: hande cancellations in the middle of the submit

        /*
        public async Task<LaunchCollectionModel> GetLaunchCollectionAsync(
                                CancellationToken cancelToken = default(CancellationToken))
        {
            // TODO: exception handling

            LaunchCollectionModel launchCollectionModel = null;

            var response = await _LaunchHttpClient.GetAsync("launch", cancelToken);

            // throw exception if not successful
            response.EnsureSuccessStatusCode();
            //launchCollectionModel = await response.Content.ReadAsAsync<LaunchCollectionModel>();

            launchCollectionModel = 
                JsonConvert.DeserializeObject<LaunchCollectionModel>(
                        await response.Content.ReadAsStringAsync()
                        );

            // TODO: error case

            return launchCollectionModel;
        }
        */

        public async Task<LaunchCollectionModel> GetLaunchCollectionAsync(
                                                string queryParam = "", 
                                                CancellationToken cancelToken = default(CancellationToken))
        {
            // TODO: exception handling

            LaunchCollectionModel launchCollectionModel = null;

            var response = await _LaunchHttpClient.GetAsync("launch", cancelToken);

            if (cancelToken.IsCancellationRequested)
            {
                // right way to break?? Throw an exception? Use TaskCanceledException?
//                break;
            }

            // throw exception if not successful
            response.EnsureSuccessStatusCode();
            //launchCollectionModel = await response.Content.ReadAsAsync<LaunchCollectionModel>();

            launchCollectionModel =
                JsonConvert.DeserializeObject<LaunchCollectionModel>(
                        await response.Content.ReadAsStringAsync()
                        );

            // TODO: error case

            return launchCollectionModel;
        }
    }
}