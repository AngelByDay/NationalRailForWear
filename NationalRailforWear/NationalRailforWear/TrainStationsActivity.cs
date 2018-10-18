
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Gms.Common;
using Android.Gms.Location;
using Android.OS;
using Android.Support.V4.Content;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TransportAPISharp;

namespace NationalRailforWear
{
    [Activity(Label = "@string/app_name")]
    public class TrainStationsActivity : ListActivity
    {
        //Create Clients
        TransportAPISharp.TransportApiClient _client = new TransportAPISharp.TransportApiClient("d467a8a3", "6e252569862290742e24efc45efe9976");
        FusedLocationProviderClient _fusedLocationProviderClient;


        Android.Locations.Location _lastLocation;


        protected override async void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            //Get Intent
            string _station_type = Intent.GetStringExtra("station_type");


            //Create Rquired Providers
            _fusedLocationProviderClient = LocationServices.GetFusedLocationProviderClient(this);    //Location Provider

            //Add Header
            ViewGroup headerView = (ViewGroup)LayoutInflater.Inflate(Resource.Layout.listview_header, this.ListView, false);
            this.ListView.AddHeaderView(headerView);

            //Add Handlers
            ListView.ItemClick += (object sender, AdapterView.ItemClickEventArgs e) =>
            {
                //Set Intent with Station Code and pass to DeparturesActivity
                var intent = new Intent(this, typeof(DeparturesActivity));
                intent.PutExtra("station_code", e.View.ContentDescription);
                intent.PutExtra("station_name", ((TextView)e.View).Text);
                StartActivity(intent);
            };

            //Get Nearby Stations Process
            if (_station_type == "nearby")
            {

                //Check if Google Play Services are Installed
                if (!IsGooglePlayServicesInstalled())
                {
                    //txtResult.Text = "ERROR (GPSERVICE)";
                    return;

                }
                //Check Location Permissions
                if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.AccessFineLocation) != Permission.Granted)
                {
                    RequestPermissions(new String[] { Manifest.Permission.AccessCoarseLocation, Manifest.Permission.AccessFineLocation }, 3);

                    //Re-Check Permissions
                    if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.AccessFineLocation) != Permission.Granted)
                    {
                        //txtResult.Text = "ERROR (PERMISSIONS)";
                        return;
                    }
                }
                //Get Current Location
                _lastLocation = await _fusedLocationProviderClient.GetLastLocationAsync();
                //if (!PackageManager.HasSystemFeature(PackageManager.FeatureLocationGps)) { txtResult.Text = "ERROR (LOCATION1-3)"; return; }
                //if (!_fusedLocationProviderClient.LastLocation.IsComplete){ txtResult.Text = "ERROR (LOCATION1-1)"; return; }
                //if(!_fusedLocationProviderClient.LastLocation.IsSuccessful) { txtResult.Text = "ERROR (LOCATION1-2)"; return; }
                if (_lastLocation == null)
                {
                    //txtResult.Text = "ERROR (LOCATION2)";
                    _lastLocation = new Android.Locations.Location("manual")    //DEBUGGING ONLY
                    {
                        Latitude = 52.000,
                        Longitude = -0.000
                    };
                }
                //Get Nearest Station List
                //Use TransportAPI
                PlacesNearResponse placesNear = await _client.PlacesNear(_lastLocation.Latitude, _lastLocation.Longitude, "train_station");
                if (placesNear != null)
                {
                    //Create and Apply Places Adapter
                    ListView.Adapter = new PlacesAdapter(this, placesNear.member);
                };

                //Set Header
                ((TextView)headerView.GetChildAt(0)).Text = "Nearby Stations";

            }

            //Get All Stations
            if (_station_type == "all")
            {
                //Check if Station Codes file exists
                if (!File.Exists(Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "station_codes.csv")))
                {
                    //Copy from Asset

                    //Download from National Rail
                    System.Net.WebClient _client = new System.Net.WebClient();
                    _client.DownloadFile("http://www.nationalrail.co.uk/static/documents/content/station_codes.csv", Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "station_codes.csv"));

                }
                //Parse Stations List
                var _stations = File.ReadLines(Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "station_codes.csv"))
                    .Skip(1)
                    .Select(x => x.Split(','))
                    .Select(x => new Place
                    {
                        station_code = x[1],
                        name = x[0]
                    }).ToArray();
                ListAdapter = new PlacesAdapter(this, _stations);

                //Set Header
                ((TextView)headerView.GetChildAt(0)).Text = "All Stations";

            }
        }


        public bool IsGooglePlayServicesInstalled()
        {
            var queryResult = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(this);
            if (queryResult == ConnectionResult.Success)
            {
                //Log.Info("MainActivity", "Google Play Services is installed on this device.");
                return true;
            }

            if (GoogleApiAvailability.Instance.IsUserResolvableError(queryResult))
            {
                // Check if there is a way the user can resolve the issue
                var errorString = GoogleApiAvailability.Instance.GetErrorString(queryResult);
                //Log.Error("MainActivity", "There is a problem with Google Play Services on this device: {0} - {1}",
                //queryResult, errorString);

                // Alternately, display the error to the user.
            }

            return false;
        }

        private async Task GetLastLocationFromDevice()
        {
            // This method assumes that the necessary run-time permission checks have succeeded.
            //txtResult.SetText(Resource.String.getting_last_location);
            Android.Locations.Location location = await _fusedLocationProviderClient.GetLastLocationAsync();

            if (location == null)
            {
                // Seldom happens, but should code that handles this scenario
            }
            else
            {
                // Do something with the location 
                //Log.Debug("Sample", "The latitude is " + location.Latitude);
            }
        }

    }

    public class PlacesAdapter : BaseAdapter<string>
    {
        Place[] places;
        Activity context;
        public PlacesAdapter(Activity context, Place[] places) : base()
        {
            this.context = context;
            this.places = places;
        }
        public override long GetItemId(int position)
        {
            return position;
        }
        public override string this[int position]
        {
            get { return places[position].ToString(); }
        }
        public override int Count
        {
            get { return places.Length; }
        }
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView; // re-use an existing view, if one is available
            if (view == null) // otherwise create a new one
                view = context.LayoutInflater.Inflate(Android.Resource.Layout.SimpleListItem1, null);
            view.FindViewById<TextView>(Android.Resource.Id.Text1).Text = places[position].ToString();
            view.FindViewById<TextView>(Android.Resource.Id.Text1).ContentDescription = places[position].station_code;
            return view;
        }
    }

}


