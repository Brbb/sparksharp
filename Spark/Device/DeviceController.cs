using System;
using RestSharp;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Text;

namespace Particle
{
	public class DeviceController
	{
		RestClient _client;  // "https://api.particle.io/v1" 

		public DeviceController(string baseUrl)
		{
			_client = new RestClient (baseUrl);
		}

		public DeviceController(RestClient client)
		{
			_client = client;
		}

		public async Task<List<Device>> ListDevicesAsync(string accessToken)
		{
			var request = new RestRequest ("v1/devices?access_token={access_token}", Method.GET);


			request.AddParameter("access_token", accessToken,ParameterType.UrlSegment);

			var response = await _client.ExecuteGetTaskAsync (request);

			if (response.StatusCode == HttpStatusCode.OK) {
				
				var responses = JsonConvert.DeserializeObject<List<Device>> (response.Content);

				return responses;
			}

			return default(List<Device>);

		}

		public async Task<Device> GetDeviceInfoByIdAsync(string accessToken,string deviceId)
		{
			var request = new RestRequest ("v1/devices/{deviceId}?access_token={access_token}", Method.GET);

			request.AddParameter("deviceId", deviceId,ParameterType.UrlSegment);
			request.AddParameter("access_token", accessToken,ParameterType.UrlSegment);

			var response = await _client.ExecuteGetTaskAsync (request);

			if (response.StatusCode == HttpStatusCode.OK) {
				

				var responses = JsonConvert.DeserializeObject<Device> (response.Content);

				return responses;
			}

			return default(Device);

		}

	}
}



