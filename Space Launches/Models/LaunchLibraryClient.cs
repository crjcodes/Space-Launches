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
//        private static readonly string ApiBaseURL = "https://ll.thespacedevs.com/2.0.0/launch/upcoming";
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

            // TBD: move to a unit test
            // TEST 1
            /*
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
            */

            /*
            // TEST 2
            result = @"{
 {
  'count': 201,
  'next': 'https://ll.thespacedevs.com/2.0.0/launch/upcoming/?format=json&limit=10&offset=10',
  'previous': null,
  'results': [
    {
      'id': 'a03444e3-c1b7-426a-b48f-4a18c60c5f28',
      'url': 'https://ll.thespacedevs.com/2.0.0/launch/a03444e3-c1b7-426a-b48f-4a18c60c5f28/?format=json',
      'launch_library_id': 1461,
      'slug': 'ariane-5-eca-galaxy-30-mev-2-bsat-4b',
      'name': 'Ariane 5 ECA+ | Galaxy 30, MEV-2 & BSAT-4B',
      'status': {
        'id': 1,
        'name': 'Go'
      },
      'net': '2020-08-15T21:33:00Z',
      'window_end': '2020-08-15T22:20:00Z',
      'window_start': '2020-08-15T21:33:00Z',
      'inhold': false,
      'tbdtime': false,
      'tbddate': false,
      'probability': -1,
      'holdreason': '',
      'failreason': null,
      'hashtag': null,
      'launch_service_provider': {
        'id': 115,
        'url': 'https://ll.thespacedevs.com/2.0.0/agencies/115/?format=json',
        'name': 'Arianespace',
        'type': 'Commercial'
      },
      'rocket': {
        'id': 2734,
        'configuration': {
          'id': 215,
          'launch_library_id': 246,
          'url': 'https://ll.thespacedevs.com/2.0.0/config/launcher/215/?format=json',
          'name': 'Ariane 5 ECA+',
          'family': 'Ariane 5',
          'full_name': 'Ariane 5 ECA+',
          'variant': 'ECA+'
        }
      },
      'mission': {
        'id': 1133,
        'launch_library_id': 1327,
        'name': 'Galaxy 30, MEV-2 & BSAT-4B',
        'description': 'Galaxy-30 is a geostationary communications satellite for Intelsat. Satellite is built by Northrop Grumman Innovation Systems (NGIS) and is planned to provide video distribution and broadcast services to customers in North America. \nGalaxy 30 satellite is launched in tandem with MEV-2 vehicle. MEV-2, which stands for Mission Extension Vehicle-2, is the second servicing mission by NGIS. MEV-2 will rendezvous and dock with the Intelsat 1002 satellite in early 2021. Then, MEV-2 will use its own thrusters and fuel supply to control the satellite’s orbit, thereby extending its useful lifetime. \nAnother passenger of the flight is the BSAT-4b satellite for the Japanese operator BSAT. BSAT-4b will serve as a back-up for BSAT-4a satellite, launched in 2017. BSAT-4b will provide Direct-to-Home television services and is expected to operate for at least 15 years.',
        'launch_designator': null,
        'type': 'Communications',
        'orbit': null
      },
      'pad': {
        'id': 77,
        'url': 'https://ll.thespacedevs.com/2.0.0/pad/77/?format=json',
        'agency_id': null,
        'name': 'Ariane Launch Area 3',
        'info_url': null,
        'wiki_url': 'http://en.wikipedia.org/wiki/ELA-3',
        'map_url': 'https://www.google.com/maps/?q=5.239,-52.769',
        'latitude': '5.239',
        'longitude': '-52.768',
        'location': {
          'id': 13,
          'url': 'https://ll.thespacedevs.com/2.0.0/location/13/?format=json',
          'name': 'Kourou, French Guiana',
          'country_code': 'GUF',
          'map_image': 'https://spacelaunchnow-prod-east.nyc3.digitaloceanspaces.com/media/launch_images/location_13_20200803142412.jpg',
          'total_launch_count': 146,
          'total_landing_count': 0
        },
        'map_image': 'https://spacelaunchnow-prod-east.nyc3.digitaloceanspaces.com/media/launch_images/pad_77_20200803143458.jpg',
        'total_launch_count': 109
      },
      'webcast_live': true,
      'image': 'https://spacelaunchnow-prod-east.nyc3.digitaloceanspaces.com/media/launcher_images/ariane_5_eca25_image_20200220090552.jpeg',
      'infographic': 'https://spacelaunchnow-prod-east.nyc3.digitaloceanspaces.com/media/launch_images/ariane2520525_infographic_20200803120232.png'
    },
    {
      'id': '98a5da32-4719-495b-9713-fcb5c058f31c',
      'url': 'https://ll.thespacedevs.com/2.0.0/launch/98a5da32-4719-495b-9713-fcb5c058f31c/?format=json',
      'launch_library_id': 1359,
      'slug': 'falcon-9-block-5-saocom-1b',
      'name': 'Falcon 9 Block 5 | SAOCOM 1B',
      'status': {
        'id': 2,
        'name': 'TBD'
      },
      'net': '2020-08-31T00:00:00Z',
      'window_end': '2020-08-31T00:00:00Z',
      'window_start': '2020-08-31T00:00:00Z',
      'inhold': false,
      'tbdtime': true,
      'tbddate': true,
      'probability': -1,
      'holdreason': null,
      'failreason': null,
      'hashtag': null,
      'launch_service_provider': {
        'id': 121,
        'url': 'https://ll.thespacedevs.com/2.0.0/agencies/121/?format=json',
        'name': 'SpaceX',
        'type': 'Commercial'
      },
      'rocket': {
        'id': 71,
        'configuration': {
          'id': 164,
          'launch_library_id': 188,
          'url': 'https://ll.thespacedevs.com/2.0.0/config/launcher/164/?format=json',
          'name': 'Falcon 9 Block 5',
          'family': 'Falcon',
          'full_name': 'Falcon 9 Block 5',
          'variant': 'Block 5'
        }
      },
      'mission': {
        'id': 1096,
        'launch_library_id': 1294,
        'name': 'SAOCOM 1B',
        'description': 'The SAOCOM 1B spacecraft is the second of the two SAOCOM constellation satellites. It is tasked with hydrology and land observaion, and will also operate jointly with the Italian COSMO-SkyMed constellation in X-band to provide frequent information relevant for emergency management.',
        'launch_designator': null,
        'type': 'Communications',
        'orbit': null
      },
      'pad': {
        'id': 80,
        'url': 'https://ll.thespacedevs.com/2.0.0/pad/80/?format=json',
        'agency_id': 121,
        'name': 'Space Launch Complex 40',
        'info_url': null,
        'wiki_url': 'https://en.wikipedia.org/wiki/Cape_Canaveral_Air_Force_Station_Space_Launch_Complex_40',
        'map_url': 'http://maps.google.com/maps?q=28.56194122,-80.57735736',
        'latitude': '28.56194122',
        'longitude': '-80.57735736',
        'location': {
          'id': 12,
          'url': 'https://ll.thespacedevs.com/2.0.0/location/12/?format=json',
          'name': 'Cape Canaveral, FL, USA',
          'country_code': 'USA',
          'map_image': 'https://spacelaunchnow-prod-east.nyc3.digitaloceanspaces.com/media/launch_images/location_12_20200803142519.jpg',
          'total_launch_count': 200,
          'total_landing_count': 0
        },
        'map_image': 'https://spacelaunchnow-prod-east.nyc3.digitaloceanspaces.com/media/launch_images/pad_80_20200803143323.jpg',
        'total_launch_count': 57
      },
      'webcast_live': false,
      'image': 'https://spacelaunchnow-prod-east.nyc3.digitaloceanspaces.com/media/launcher_images/falcon25209_image_20190224025007.jpeg',
      'infographic': null
    },
    {
      'id': 'c179a24e-416b-4077-af8a-4670dfdb880a',
      'url': 'https://ll.thespacedevs.com/2.0.0/launch/c179a24e-416b-4077-af8a-4670dfdb880a/?format=json',
      'launch_library_id': 1595,
      'slug': 'soyuz-21a-bars-m-no-3',
      'name': 'Soyuz 2.1a | Bars-M No. 3',
      'status': {
        'id': 2,
        'name': 'TBD'
      },
      'net': '2020-08-31T00:00:00Z',
      'window_end': '2020-08-31T00:00:00Z',
      'window_start': '2020-08-31T00:00:00Z',
      'inhold': false,
      'tbdtime': true,
      'tbddate': true,
      'probability': -1,
      'holdreason': null,
      'failreason': null,
      'hashtag': null,
      'launch_service_provider': {
        'id': 193,
        'url': 'https://ll.thespacedevs.com/2.0.0/agencies/193/?format=json',
        'name': 'Russian Space Forces',
        'type': 'Government'
      },
      'rocket': {
        'id': 78,
        'configuration': {
          'id': 24,
          'launch_library_id': 49,
          'url': 'https://ll.thespacedevs.com/2.0.0/config/launcher/24/?format=json',
          'name': 'Soyuz 2.1A',
          'family': 'Soyuz',
          'full_name': 'Soyuz 2.1A',
          'variant': '2.1A'
        }
      },
      'mission': {
        'id': 836,
        'launch_library_id': 842,
        'name': 'Bars-M No. 3',
        'description': 'Bars-M is the second incarnation of the Bars project, which was started in the mid 1990ies to develop a successor for the Komtea class of area surveillance satellites. The original Bars project was halted in the early 2000s. In 2007, TsSKB-Progress was contracted for Bars-M, for which reportedly the Yantar-based service module was replaced by a new developed advanced service module.\n\nThe Bars-M satellites feature an electro-optical camera system called Karat, which is developed and built by the Leningrad Optical Mechanical Association (LOMO), and a dual laser altimeter instrument to deliver topographic imagery, stereo images, altimeter data and high-resolution images with a ground resolution around 1 meter.',
        'launch_designator': null,
        'type': 'Government/Top Secret',
        'orbit': {
          'id': 17,
          'name': 'Sun-Synchronous Orbit',
          'abbrev': 'SSO'
        }
      },
      'pad': {
        'id': 85,
        'url': 'https://ll.thespacedevs.com/2.0.0/pad/85/?format=json',
        'agency_id': 163,
        'name': '43/4 (43R)',
        'info_url': null,
        'wiki_url': '',
        'map_url': 'http://maps.google.com/maps?q=62.929+N,+40.457+E',
        'latitude': '62.92883',
        'longitude': '40.457098',
        'location': {
          'id': 6,
          'url': 'https://ll.thespacedevs.com/2.0.0/location/6/?format=json',
          'name': 'Plesetsk Cosmodrome, Russian Federation',
          'country_code': 'RUS',
          'map_image': 'https://spacelaunchnow-prod-east.nyc3.digitaloceanspaces.com/media/launch_images/location_6_20200803142434.jpg',
          'total_launch_count': 67,
          'total_landing_count': 0
        },
        'map_image': 'https://spacelaunchnow-prod-east.nyc3.digitaloceanspaces.com/media/launch_images/pad_85_20200803143554.jpg',
        'total_launch_count': 35
      },
      'webcast_live': false,
      'image': 'https://spacelaunchnow-prod-east.nyc3.digitaloceanspaces.com/media/launcher_images/soyuz25202.1a_image_20190224012318.jpeg',
      'infographic': null
    },
    {
      'id': '38ed8ff1-0335-4e68-9dd4-925c76ad01ed',
      'url': 'https://ll.thespacedevs.com/2.0.0/launch/38ed8ff1-0335-4e68-9dd4-925c76ad01ed/?format=json',
      'launch_library_id': 2038,
      'slug': 'astra-rocket-31-maiden-flight',
      'name': 'Astra Rocket 3.1 | Maiden Flight',
      'status': {
        'id': 2,
        'name': 'TBD'
      },
      'net': '2020-08-31T02:00:00Z',
      'window_end': '2020-08-31T04:30:00Z',
      'window_start': '2020-08-31T02:00:00Z',
      'inhold': false,
      'tbdtime': true,
      'tbddate': true,
      'probability': -1,
      'holdreason': '',
      'failreason': null,
      'hashtag': null,
      'launch_service_provider': {
        'id': 285,
        'url': 'https://ll.thespacedevs.com/2.0.0/agencies/285/?format=json',
        'name': 'Astra Space',
        'type': null
      },
      'rocket': {
        'id': 2661,
        'configuration': {
          'id': 213,
          'launch_library_id': 243,
          'url': 'https://ll.thespacedevs.com/2.0.0/config/launcher/213/?format=json',
          'name': 'Astra Rocket 3.0',
          'family': 'Astra Rocket',
          'full_name': 'Astra Rocket 3.0',
          'variant': '3.0'
        }
      },
      'mission': {
        'id': 1083,
        'launch_library_id': 1280,
        'name': 'Maiden Flight',
        'description': 'This is the first orbital attempt of Astra Space small satellite launch vehicle. Rocket carries no payload for this flight.',
        'launch_designator': null,
        'type': 'Test Flight',
        'orbit': null
      },
      'pad': {
        'id': 114,
        'url': 'https://ll.thespacedevs.com/2.0.0/pad/114/?format=json',
        'agency_id': null,
        'name': 'Launch Pad B',
        'info_url': null,
        'wiki_url': 'https://en.wikipedia.org/wiki/Pacific_Spaceport_Complex_%E2%80%93_Alaska',
        'map_url': 'https://www.google.ee/maps/search/57.4304299,-152.3586347',
        'latitude': '57.4304299',
        'longitude': '-152.3586347',
        'location': {
          'id': 25,
          'url': 'https://ll.thespacedevs.com/2.0.0/location/25/?format=json',
          'name': 'Kodiak Launch Complex, Alaska, USA',
          'country_code': 'USA',
          'map_image': 'https://spacelaunchnow-prod-east.nyc3.digitaloceanspaces.com/media/launch_images/location_25_20200803142500.jpg',
          'total_launch_count': 2,
          'total_landing_count': 0
        },
        'map_image': 'https://spacelaunchnow-prod-east.nyc3.digitaloceanspaces.com/media/launch_images/pad_114_20200803145248.jpg',
        'total_launch_count': 0
      },
      'webcast_live': false,
      'image': 'https://spacelaunchnow-prod-east.nyc3.digitaloceanspaces.com/media/launcher_images/astra2520rocket25203.0_image_20200216210731.jpg',
      'infographic': 'https://spacelaunchnow-prod-east.nyc3.digitaloceanspaces.com/media/launch_images/astra2520rocke_infographic_20200801170349.png'
    },
    {
      'id': 'a9526278-ef96-48ba-abbd-275f1370d6c0',
      'url': 'https://ll.thespacedevs.com/2.0.0/launch/a9526278-ef96-48ba-abbd-275f1370d6c0/?format=json',
      'launch_library_id': 2084,
      'slug': 'electron-return-to-flight-payload-unknown',
      'name': 'Electron | Return To Flight (Payload unknown)',
      'status': {
        'id': 2,
        'name': 'TBD'
      },
      'net': '2020-08-31T03:00:00Z',
      'window_end': '2020-08-31T07:00:00Z',
      'window_start': '2020-08-31T03:00:00Z',
      'inhold': false,
      'tbdtime': true,
      'tbddate': true,
      'probability': -1,
      'holdreason': null,
      'failreason': null,
      'hashtag': null,
      'launch_service_provider': {
        'id': 147,
        'url': 'https://ll.thespacedevs.com/2.0.0/agencies/147/?format=json',
        'name': 'Rocket Lab Ltd',
        'type': 'Commercial'
      },
      'rocket': {
        'id': 2740,
        'configuration': {
          'id': 26,
          'launch_library_id': 148,
          'url': 'https://ll.thespacedevs.com/2.0.0/config/launcher/26/?format=json',
          'name': 'Electron',
          'family': 'Electron',
          'full_name': 'Electron',
          'variant': ''
        }
      },
      'mission': null,
      'pad': {
        'id': 65,
        'url': 'https://ll.thespacedevs.com/2.0.0/pad/65/?format=json',
        'agency_id': 147,
        'name': 'Rocket Lab Launch Complex 1A',
        'info_url': null,
        'wiki_url': 'https://en.wikipedia.org/wiki/Rocket_Lab_Launch_Complex_1',
        'map_url': 'https://www.google.com/maps/place/-39.262833,177.864469',
        'latitude': '-39.262833',
        'longitude': '177.864469',
        'location': {
          'id': 10,
          'url': 'https://ll.thespacedevs.com/2.0.0/location/10/?format=json',
          'name': 'Onenui Station, Mahia Peninsula, New Zealand',
          'country_code': 'NZL',
          'map_image': 'https://spacelaunchnow-prod-east.nyc3.digitaloceanspaces.com/media/launch_images/location_10_20200803142509.jpg',
          'total_launch_count': 13,
          'total_landing_count': 0
        },
        'map_image': 'https://spacelaunchnow-prod-east.nyc3.digitaloceanspaces.com/media/launch_images/pad_65_20200803143549.jpg',
        'total_launch_count': 13
      },
      'webcast_live': false,
      'image': 'https://spacelaunchnow-prod-east.nyc3.digitaloceanspaces.com/media/launcher_images/electron_image_20190705175640.jpeg',
      'infographic': null
    }
  ]
}";
            */

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