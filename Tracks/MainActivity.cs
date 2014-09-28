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
using Android.Media;

namespace Tracks
{
	[Activity (Label = "Tracks", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity
	{
		MediaPlayer _player;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			_player = new MediaPlayer();

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			// Get our button from the layout resource,
			// and attach an event to it
			Button button = FindViewById<Button> (Resource.Id.myButton);
			
			button.Click += delegate {
				var request = HttpWebRequest.Create("http://itunes.apple.com/search?term=roxanne");
				request.Method = "GET";

				using (HttpWebResponse response = request.GetResponse() as HttpWebResponse) {
					DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(Response));
					Response searchResults = (Response)jsonSerializer.ReadObject(response.GetResponseStream());
					var url = searchResults.Results[0].PreviewUrl;
					_player.SetDataSource(url);
					_player.Prepare();
					_player.Start();
					button.Text = searchResults.Results[0].ArtistName;
				}
			};
		}
	}
}


