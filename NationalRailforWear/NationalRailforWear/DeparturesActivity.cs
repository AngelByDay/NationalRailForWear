
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using TransportAPISharp;
using System.Collections.Generic;

namespace NationalRailforWear
{
    [Activity(Label = "service_list")]
    public class DeparturesActivity : ListActivity
    {
        //Create Clients
        TransportAPISharp.TransportApiClient _client = new TransportAPISharp.TransportApiClient("d467a8a3", "6e252569862290742e24efc45efe9976");

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            //Get Station Code from Intent
            string _station_code = Intent.GetStringExtra("station_code");

            LiveTrainsResponse liveTrains = await _client.TrainsLive(_station_code);
            if (liveTrains != null)
            {
                //Create and Apply Places Adapter
                Dictionary<string, List<TrainDeparture>> _departures = liveTrains.Departures;
                ListAdapter = new DeparturesAdapter(this, _departures["all"].ToArray());
            };

            //this.ListAdapter = new ServiceAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, _serviceList);
        }
    }

    public class DeparturesAdapter : BaseAdapter<string>
    {
        TrainDeparture[] departures;
        Activity context;
        public DeparturesAdapter(Activity context, TrainDeparture[] departures) : base()
        {
            this.context = context;
            this.departures = departures;
        }
        public override long GetItemId(int position)
        {
            return position;
        }
        public override string this[int position]
        {
            get { return departures[position].ToString(); }
        }
        public override int Count
        {
            get { return departures.Length; }
        }
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView; // re-use an existing view, if one is available
            if (view == null) // otherwise create a new one
                view = context.LayoutInflater.Inflate(Android.Resource.Layout.SimpleListItem1, null);
            view.FindViewById<TextView>(Android.Resource.Id.Text1).Text = departures[position].ToString();
            view.FindViewById<TextView>(Android.Resource.Id.Text1).ContentDescription = departures[position].service;
            return view;
        }
    }
}