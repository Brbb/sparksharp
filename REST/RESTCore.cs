using System;
using System.Collections.Specialized;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using RestSharp;
using System.Threading.Tasks;


namespace Spark
{
	public class RESTCore
	{
		public static SparkResponse SparkGet (string variableName)
		{
			try {
				var requestText = String.Format("https://api.spark.io/v1/devices/{0}/{2}?access_token={1}",Configuration.DeviceID,Configuration.AccessToken,variableName);
				HttpWebRequest request = WebRequest.Create (requestText) as HttpWebRequest;

				using (HttpWebResponse response = request.GetResponse() as HttpWebResponse) {
					if (response.StatusCode != HttpStatusCode.OK)
						throw new Exception (String.Format (
                	"Server error (HTTP {0}: {1}).",
                	response.StatusCode,
                	response.StatusDescription)
						);




					// Spark JSON response format
					/* {
  						"cmd": "VarReturn",
  						"name": "temperature",
  						"result": 28.08791208791208,
  						"coreInfo": {
    						"last_app": "",
    						"last_heard": "2014-08-24T17:18:17.726Z",
    						"connected": true,
    						"deviceID": "<deviceID>"
  							}
						} */


					var stream = response.GetResponseStream ();
					var rdr = new StreamReader (stream);
					var stringResponse = rdr.ReadToEnd ();

					return JsonConvert.DeserializeObject<SparkResponse>(stringResponse);
				}
			} catch (Exception e) {
				Console.WriteLine(e.Message);
			}

			return null;
		}

		public static async Task<SparkResponse> SparkGetAsync(string variable)
		{
			var request = String.Format("https://api.spark.io/v1/devices/{0}/{2}?access_token={1}",Configuration.DeviceID,Configuration.AccessToken,variable);
			var client = new RestClient ();

			var req = new RestRequest (request);
			var response = await client.ExecuteGetTaskAsync (req);

			if (response.StatusCode == HttpStatusCode.OK) {
				return JsonConvert.DeserializeObject<SparkResponse> (response.Content);
			}

			return null;

		}
	}
}
