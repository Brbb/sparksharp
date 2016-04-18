using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RestSharp;
using System.Net;
using Newtonsoft.Json;

namespace Particle
{
	public class AccessTokenController
	{
		RestClient _client; 

		public AccessTokenController (string baseUrl)
		{
			_client = new RestClient (baseUrl);
		}

		public AccessTokenController(RestClient client)
		{
			_client = client;
		}

		public async Task<List<AccessToken>> ListTokensAsync(string username,string password)
		{
			var request = new RestRequest("v1/access_tokens",Method.GET);
			string base64credentials = Convert.ToBase64String (System.Text.ASCIIEncoding.Default.GetBytes (String.Format ("{0}:{1}", username, password)));
			request.AddHeader ("Authorization", String.Format ("Basic {0}", base64credentials));

			var response = await _client.ExecuteGetTaskAsync(request);

			if (response.StatusCode == HttpStatusCode.OK) {
				List<AccessToken> responses = (List<AccessToken>) JsonConvert.DeserializeObject(response.Content,typeof(List<AccessToken>));

				return responses;
			}

			return default(List<AccessToken>);
		}

		public async Task<DeleteResponse> DeleteTokenAsync(string username, string password, string access_token)
		{
			// Assumption: grant_type, client_id and client_secret parameters are hard coded because 
			// should be always the same.

			var request = new RestRequest ("/access_tokens/{token}", Method.DELETE);
			request.AddParameter ("token", access_token);

			string base64credentials = Convert.ToBase64String (System.Text.ASCIIEncoding.Default.GetBytes (String.Format ("{0}:{1}", username, password)));

			request.AddHeader ("Authorization", String.Format ("Basic {0}", base64credentials));

			var response = await _client.ExecuteTaskAsync (request);

			if (response.StatusCode == HttpStatusCode.OK) {
				var deleteResponse = (DeleteResponse)JsonConvert.DeserializeObject<DeleteResponse> (response.Content);
				return deleteResponse;
			}

			return default(DeleteResponse);
		}

		public async Task<AuthResponse> GenerateAccessTokenAsync(string username, string password,
			string grantType = "password", int expires_in = 0, DateTime expires_at = default(DateTime))
		{
			// 			This is the equivalent SparkSharp way to ask for an auth token for the specified user (email).

			// 			var auth = Spark.RESTCore.PostAuthAsync ("user", "password").Result;
			//			 if (auth != null)
			//				Configuration.AccessToken = auth.Access_Token;
			//

			// Assumption: grant_type, client_id and client_secret parameters are hard coded because 
			// should be always the same.

			var request = new RestRequest("oauth/token",Method.POST);

			var client_id = "particle";
			var client_secret = "particle";

			request.AddParameter ("grant_type", grantType);
			request.AddParameter ("client_id", client_id);
			request.AddParameter ("client_secret", client_secret);
			request.AddParameter ("username", username);
			request.AddParameter ("password", password);

			if(expires_in != 0)
				request.AddParameter("expires_in",expires_in);

			if(expires_at != default(DateTime))
				request.AddParameter("expires_at",expires_at.ToUniversalTime());


			var response = await _client.ExecutePostTaskAsync(request);

			if (response.StatusCode == HttpStatusCode.OK) {
				AuthResponse authResponse = (AuthResponse) JsonConvert.DeserializeObject<AuthResponse>(response.Content);
				return authResponse;
			}

			return default(AuthResponse);
		}

	}
}

