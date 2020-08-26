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
        private static readonly string ApiBaseURL = "https://ll.thespacedevs.com/2.0.0/launch/";

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

        // all of the GET request and deserialization are run asynchronously
        // TBD: add support for cancellation if it takes too long
        // TBD: exception handling, starting with the most-prone-to-fail single-point failures
        public async Task<LaunchCollectionModel> GetLaunchCollectionAsync(
                                                string queryParam = "?format=json", 
                                                CancellationToken cancelToken = default(CancellationToken))
        {

            // error paths
            //   1.  call cancelled
            //   2.  call times out
            //   3.  call throws an exception to handle in-app
            //   4.  call throws an exception to pass on
            //   5.  response is garbage
            //   6.  response success status code not there
            var response = await _LaunchHttpClient.GetAsync(queryParam, cancelToken);

            if (cancelToken.IsCancellationRequested)
            {
                // right way to break?? Throw an exception? Use TaskCanceledException?
//                break;
            }

            // throw exception if not successful
            response.EnsureSuccessStatusCode();


            // Error paths:
            //   1.  call throws an exception
            //      -- ones to handle
            //      -- unknown ones to pass on
            //   2.  call returns an error status code
            //   3.  call returns corrupted content
            //   4.  call returns partially corrupted content
            LaunchCollectionModel launchCollection = null;
            string result = null;
            result = await response.Content.ReadAsStringAsync();

            launchCollection = ConvertModel(result);

            // validate LaunchCollection here

            return launchCollection;
        }

        public LaunchCollectionModel ConvertModel(string responseContent)
        {
            // VALIDATION
            //   1. Overall, valid JSON format?
            //   2. 
            //

            // Error paths:
            //   1.  call throws an exception
            //      -- ones to handle
            //      -- unknown ones to pass on
            //   2.  call returns an error status code
            //   3.  call returns corrupted content
            //   4.  call returns partially corrupted content
            return JsonConvert.DeserializeObject<LaunchCollectionModel>(responseContent);
            
        }
    }
}