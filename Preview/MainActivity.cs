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
using Java.Lang;

namespace Preview
{
	[Activity (Label = "Preview", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity
	{
		MediaPlayer _player;
		SearchView _searchView;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			_player = new MediaPlayer();

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			// Prepare the search box
			_searchView = FindViewById<SearchView> (Resource.Id.searchView);
			_searchView.SetIconifiedByDefault (false);
			_searchView.QueryTextSubmit += (sender, e) => {
				PlayFirstResult(e.Query);
			};
		}

		void PlayFirstResult(string term) {
			// TODO: encode term properly
			var request = HttpWebRequest.Create("http://itunes.apple.com/search?term=" + term);
			request.Method = "GET";
			request.Timeout = 2000;

			try {
				using (HttpWebResponse response = request.GetResponse() as HttpWebResponse) {
					DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(Response));
					Response searchResults = (Response)jsonSerializer.ReadObject(response.GetResponseStream());
					var result = searchResults.Results[0];
					var fileName = DownloadPreview(result.PreviewUrl, result.ArtistName, result.TrackName);
					try {
						_player.SetDataSource(fileName);
					} catch (IllegalStateException e) {
						_player.Reset();
						_player.SetDataSource(fileName);
					}
					_player.Prepare();
					_player.Start();
				}

			} catch (System.Net.WebException e) {
				Toast.MakeText (this, e.Message, ToastLength.Long).Show();
			}
		}

		string DownloadPreview(string address, string artistName, string trackName) {
			var storagePath = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;
			var fileName = Path.Combine(storagePath, string.Format("{0} - {1}.m4a", artistName, trackName));
			var webClient = new WebClient ();
			webClient.DownloadFile(address, fileName);
			return fileName;
		}
	}
}


