using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Net;
using System.IO;
using System.Runtime.Serialization.Json;

namespace Tracks
{
	[Activity (Label = "Tracks", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity
	{
		int count = 1;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			// Get our button from the layout resource,
			// and attach an event to it
			Button button = FindViewById<Button> (Resource.Id.myButton);
			
			button.Click += delegate {
				var request = HttpWebRequest.Create("http://itunes.apple.com/search?term=beyonce%20diva");
				request.Method = "GET";

				using (HttpWebResponse response = request.GetResponse() as HttpWebResponse) {
					DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(Response));
					Response searchResults = (Response)jsonSerializer.ReadObject(response.GetResponseStream());
					button.Text = searchResults.Results[0].ArtistName;
				}
			};
		}
	}
}


