using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Spark
{
	public class SSEvent {
		public string Name { get; set; }
		public SSEInfo Info { get; private set;}

		public void SetData (string rawString)
		{
			Info = SSEInfo.Parse(rawString);
		}
	}

	public class SSEInfo
	{
		public string Data { get; set;}
		public string TTL { get; set; }
		public string Published_At { get; set; }
		public string CoreId { get; set; }

		// if not able to parse, set RAW manually
		public string RAW { get; set; }

		public static SSEInfo Parse(string rawData)
		{
			SSEInfo result = new SSEInfo ();
			try
			{
				result = JsonConvert.DeserializeObject<SSEInfo> (rawData);
			}
			catch(Exception e) {
				result.RAW = rawData;
				Console.WriteLine (e.Message);
			}

			return result;
		}
	}
}

