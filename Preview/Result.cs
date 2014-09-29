using System;
using System.Runtime.Serialization;

namespace Preview
{
	[DataContract]
	public class Result
	{
		[DataMember(Name = "previewUrl")]
		public string PreviewUrl { get; set; }
		[DataMember(Name = "artistName")]
		public string ArtistName { get; set; }
		[DataMember(Name = "trackName")]
		public string TrackName { get; set; }
	}

}

