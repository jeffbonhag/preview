using System;
using System.Runtime.Serialization;

namespace Tracks
{
	[DataContract]
	public class Response
	{
		[DataMember(Name = "results")]
		public Result[] Results { get; set; }
	}
}

