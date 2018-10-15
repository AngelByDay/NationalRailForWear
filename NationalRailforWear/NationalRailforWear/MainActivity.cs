using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using System;

namespace NationalRailforWear
{
    [Activity(Label = "@string/app_name", MainLauncher =true)]
    public class MainActivity : ListActivity
    {
        string[] items;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            //Add Header
            ViewGroup headerView = (ViewGroup)LayoutInflater.Inflate(Resource.Layout.listview_header, this.ListView, false);
            this.ListView.AddHeaderView(headerView);
            ((TextView)headerView.GetChildAt(0)).Text = "National Rail";

            //Set Menu Items
            items = new string[] { "Nearby Stations", "All Stations" };
            ListAdapter = new ArrayAdapter<String>(this, Android.Resource.Layout.SimpleListItem1, items);

        }
        protected override void OnListItemClick(ListView l, View v, int position, long id)
        {
            var intent = new Intent(this, typeof(NearbyStationsActivity));
            StartActivity(intent);
        }
    }
}