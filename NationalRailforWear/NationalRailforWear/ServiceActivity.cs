
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using TransportAPISharp;
using System.Collections.Generic;
using System;

namespace NationalRailforWear
{
    [Activity(Label = "service_list")]
    public class ServiceActivity : ListActivity
    {
        //Create Clients
        TransportAPISharp.TransportApiClient _client = new TransportAPISharp.TransportApiClient("d467a8a3", "6e252569862290742e24efc45efe9976");

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            //Get Station Code from Intent
            string _service_id = Intent.GetStringExtra("service_id");
            //string _station_name = Intent.GetStringExtra("station_code");


            //Add Header
            ViewGroup headerView = (ViewGroup)LayoutInflater.Inflate(Resource.Layout.listview_header, this.ListView, false);
            this.ListView.AddHeaderView(headerView);
            ((TextView)headerView.GetChildAt(0)).Text = $"{_service_id}";

            //Get Live Departures
            TrainServiceResponse _trainService = await _client.TrainService(_service_id, DateTime.Now);
            if (_trainService != null)
            {
                //Create and Apply Places Adapter
                List<TrainServiceStop> _stops = _trainService.ServiceStops;
                ListAdapter = new ServiceAdapter(this, _stops.ToArray());
            };
        }
    }

    public class ServiceAdapter : BaseAdapter<string>
    {
        TrainServiceStop[] _stops;
        Activity context;
        public ServiceAdapter(Activity context, TrainServiceStop[] stops) : base()
        {
            this.context = context;
            this._stops = stops;
        }
        public override long GetItemId(int position)
        {
            return position;
        }
        public override string this[int position]
        {
            get { return _stops[position].ToString(); }
        }
        public override int Count
        {
            get { return _stops.Length; }
        }
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView; // re-use an existing view, if one is available
            if (view == null) // otherwise create a new one
                view = context.LayoutInflater.Inflate(Android.Resource.Layout.SimpleListItem1, null);
            view.FindViewById<TextView>(Android.Resource.Id.Text1).Text = _stops[position].ToString();
            view.FindViewById<TextView>(Android.Resource.Id.Text1).ContentDescription = _stops[position].StationCode;
            return view;
        }
    }
}