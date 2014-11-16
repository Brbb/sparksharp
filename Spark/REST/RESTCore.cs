using System;
using System.Collections.Specialized;
using System.Collections.Generic;
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



		#region Auth

		public static async Task<VariableResponse> GetVariableAsync(string variable)
		{
			var request = String.Format("https://api.spark.io/v1/devices/{0}/{2}?access_token={1}",Configuration.DeviceID,Configuration.AccessToken,variable);
			var client = new RestClient ();

			var req = new RestRequest (request);
			var response = await client.ExecuteGetTaskAsync (req);

			if (response.StatusCode == HttpStatusCode.OK) {
				return JsonConvert.DeserializeObject<VariableResponse> (response.Content);
			}

			return null;

		}

		public static async Task<FunctionResponse> CallFunctionAsync(string functionName){

			//			curl https://api.spark.io/v1/devices/0123456789abcdef01234567/brew \
//			-d access_token=1234123412341234123412341234123412341234 \
//				-d "args=202,230"


//			// EXAMPLE RESPONSE
//			{
//				"id": "0123456789abcdef01234567",
//				"name": "prototype99",
//				"connected": true,
//				"return_value": 42
//			}


			var request = String.Format("https://api.spark.io/v1/devices/{0}/{2}?access_token={1}",Configuration.DeviceID,Configuration.AccessToken,functionName);
			var client = new RestClient ();

			var req = new RestRequest (request);
			var response = await client.ExecutePostTaskAsync (req);

			if (response.StatusCode == HttpStatusCode.OK) {
				return JsonConvert.DeserializeObject<FunctionResponse>(response.Content);
			}

			return null;
		}

		public static async Task<List<TokenResponse>> ListTokensAsync(string email, string secret)
		{
			var request = String.Format("https://api.spark.io/v1/access_tokens");
			var client = new RestClient ();

			string base64credentials = Convert.ToBase64String(System.Text.ASCIIEncoding.Default.GetBytes(String.Format("{0}:{1}", email, secret)));

			var req = new RestRequest (request);
			req.AddHeader ("Authorization", String.Format ("Basic {0}", base64credentials));
			var response = await client.ExecuteGetTaskAsync (req);

			if (response.StatusCode == HttpStatusCode.OK) {
				List<TokenResponse> responses = (List<TokenResponse>) JsonConvert.DeserializeObject(response.Content,typeof(List<TokenResponse>));
				return responses;
			}

			return null;
		}

		public static async Task<AuthResponse> PostAuthAsync(string username, string password)
		{

			//curl https://api.spark.io/oauth/token -u spark:spark \
//			-d grant_type=password -d username=joe@example.com -d password=SuperSecret
//
//				# A typical JSON response will look like this
//			{
//				"access_token": "<token_id>",
//				"token_type": "bearer",
//				"expires_in": 7776000
//			}


			// Assumption: grant_type, client_id and client_secret parameters are hard coded because 
			// should be always the same.

			var request = String.Format("https://api.spark.io/oauth/token");
			var client = new RestClient ();

			var grant_type = "password";
			var client_id = "spark";
			var client_secret = "spark";


			var req = new RestRequest (request);
			req.AddParameter ("grant_type", grant_type);
			req.AddParameter ("client_id", client_id);
			req.AddParameter ("client_secret", client_secret);
			req.AddParameter ("username", username);
			req.AddParameter ("password", password);


			var response = await client.ExecutePostTaskAsync (req);

			if (response.StatusCode == HttpStatusCode.OK) {
				AuthResponse responses = (AuthResponse) JsonConvert.DeserializeObject<AuthResponse>(response.Content);
				return responses;
			}

			return null;
		}

		public static async Task<DeleteResponse> DeleteTokenAsync(string username, string password, string access_token)
		{

//			curl https://api.spark.io/v1/access_tokens/b5b901e8760164e134199bc2c3dd1d228acf2d98 \
//			-u joe@example.com:SuperSecret -X DELETE
//
//			# Example JSON response
//			{
//				"ok": true
//			}


			// Assumption: grant_type, client_id and client_secret parameters are hard coded because 
			// should be always the same.

			var request = String.Format("https://api.spark.io/v1/access_tokens/{0}",access_token);
			var client = new RestClient ();


			string base64credentials = Convert.ToBase64String(System.Text.ASCIIEncoding.Default.GetBytes(String.Format("{0}:{1}", username, password)));

			var req = new RestRequest (request,Method.DELETE);
			req.AddHeader ("Authorization", String.Format ("Basic {0}", base64credentials));


			var response = await client.ExecuteTaskAsync(req);

			if (response.StatusCode == HttpStatusCode.OK) {
				var responses = (DeleteResponse) JsonConvert.DeserializeObject<DeleteResponse>(response.Content);
				return responses;
			}

			return null;
		}



		#endregion
	}
}
