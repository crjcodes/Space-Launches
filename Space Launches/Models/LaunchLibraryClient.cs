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
        private static readonly string ApiBaseURL = "https://ll.thespacedevs.com/2.0.0/launch";
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
            /*
            // TODO: exception handling
            //  -- formatting problem in query
            //  -- cancellation
            //  -- if GET fails for whatever reason
            //  -- if the result in error 
            //  -- if the result is in a bad format
            //  -- timeout?
            //  -- other possible exceptions

            var response = await _LaunchHttpClient.GetAsync(queryParam, cancelToken);

            if (cancelToken.IsCancellationRequested)
            {
                // right way to break?? Throw an exception? Use TaskCanceledException?
//                break;
            }

            // throw exception if not successful
            response.EnsureSuccessStatusCode();
            */

            string result = null;
            //string result = await response.Content.ReadAsStringAsync();
            LaunchCollectionModel launchCollection = null;

            // TBD: move to a unit test

            result = @"{
                'count': 1820,
                'next': 'https://ll.thespacedevs.com/2.0.0/launch/?format=json&limit=10&offset=10',
                'previous': null,
                'results': [
                {
                    'id': '9279744e-46b2-4eca-adea-f1379672ec81',
                    'url': 'https://ll.thespacedevs.com/2.0.0/launch/9279744e-46b2-4eca-adea-f1379672ec81/?format=json',
                    'launch_library_id': 1829,
                    'slug': 'atlas-lv-3a-samos-2',
                    'name': 'Atlas LV-3A | Samos 2',
                }
                ]
            }";


            launchCollection = ConvertModel(result);

            return launchCollection;
        }

        public LaunchCollectionModel ConvertModel(string responseContent)
        {
            return JsonConvert.DeserializeObject<LaunchCollectionModel>(responseContent);
            
        }
    }
}