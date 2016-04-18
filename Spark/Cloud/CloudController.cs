using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using RestSharp;
using System.Threading.Tasks;
using RestSharp.Authenticators;


namespace Particle
{
	/// <summary>
	/// Cloud controller keeps centralized information (Access Token, Device) and operations (get variable, call function).
	/// This happens because the other controllers have their own responsibility about Access Token CRUD and Devices, but we need
	/// a common point to controll the Particle in the cloud without retrieve each time credentials and IDs.
	/// </summary>
	public class CloudController
	{

		#region Auth

		public RestClient Client { get; private set; }

		// "https://api.particle.io/v1"
		public CloudController(string baseUrl, AccessToken accessToken, Device device)
		{
			Client = new RestClient (baseUrl);
			AccessToken = accessToken;
			Device = device;
		}

		public AccessToken AccessToken {
			get;
			private set;
		}

		public Device Device {
			get;
			private set;
		}

		public async Task<VariableResponse> GetVariableAsync(string variable)
		{
			var request = new RestRequest ("v1/devices/{deviceId}/{variable}?access_token={token}");

			request.AddParameter ("deviceId", Device.Id, ParameterType.UrlSegment);
			request.AddParameter ("variable", variable, ParameterType.UrlSegment);
			request.AddParameter ("token", AccessToken.Token, ParameterType.UrlSegment);

			var response = await Client.ExecuteGetTaskAsync (request);

			if (response.StatusCode == HttpStatusCode.OK) {
				return JsonConvert.DeserializeObject<VariableResponse> (response.Content);
			}

			return null;
		}
			

		public async Task<FunctionResponse> CallFunctionAsync(string functionName,string arg = ""){

			if (String.IsNullOrEmpty (functionName))
				throw new ArgumentNullException();
			if (!String.IsNullOrEmpty (arg) && arg.Length > 63)
				throw new ArgumentException ("Argument cannot be longer than 63 characters");

			var request = new RestRequest("v1/devices/{deviceId}/{functionName}?access_token={token}");

			request.AddParameter ("deviceId", Device.Id, ParameterType.UrlSegment);
			request.AddParameter ("functionName", functionName, ParameterType.UrlSegment);
			request.AddParameter ("token", AccessToken.Token, ParameterType.UrlSegment);

			if (!String.IsNullOrEmpty (arg))
				request.AddParameter ("arg", arg);

			var response = await Client.ExecutePostTaskAsync (request);

			if (response.StatusCode == HttpStatusCode.OK) {
				return JsonConvert.DeserializeObject<FunctionResponse>(response.Content);
			}

			return default(FunctionResponse);
		}

		#endregion
	}
}
