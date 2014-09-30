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
		SearchView _searchView;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			// Prepare the search box
			_searchView = FindViewById<SearchView> (Resource.Id.searchView);
			_searchView.SetIconifiedByDefault (false);
			_searchView.QueryTextSubmit += (sender, e) => {
				PlayFirstResult(e.Query);
				_searchView.SetQuery("", false);
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
					AddFileToMediaLibrary(fileName);
					Play(fileName);
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

		void AddFileToMediaLibrary(string fileName) {
			Intent intent = new Intent(Intent.ActionMediaScannerScanFile);
			intent.SetData (Android.Net.Uri.FromFile (new Java.IO.File (fileName)));
			SendBroadcast (intent);
		}

		void Play(string fileName) {
			var player = new MediaPlayer();
			player.SetDataSource (fileName);
			player.Prepare ();
			player.Start ();
		}
	}
}


