using System;
using Particle;
using System.Linq;
using System.Threading.Tasks;

namespace Tester
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			string username = "myUser";
			string password = "myPassword";

			var baseUrl = "https://api.particle.io";
			var accessTokenController = new AccessTokenController (baseUrl);

			// Login, if a token exists and it's not expired use it.
			var accessTokens = accessTokenController.ListTokensAsync (username, password).Result;
			var userToken = accessTokens.FirstOrDefault (t => t.Client == "user");

			if (userToken == null) {
				var authResponse = accessTokenController.GenerateAccessTokenAsync (username, password).Result;
				userToken = new AccessToken {
					Token = authResponse.Access_Token,
					Expires_At = DateTime.UtcNow.AddSeconds (authResponse.Expires_In).ToString ("o"),
					Client = "user"
				};
			}
//			var sseManager = new SSECore (baseUrl,userToken.Token);
//			sseManager.SSEPublished += (eve) => {
//				Console.WriteLine (eve.Info.Data);
//			};
//
//			Task.Run (() => {
//				sseManager.GetStreamOfEvents ("Dummy");
//			});

			var deviceController = new DeviceController (baseUrl);
			var devices = deviceController.ListDevicesAsync (userToken.Token).Result;

			var device = devices.FirstOrDefault ();

			if (device != null && device.Connected) {
				var cloudController = new CloudController (baseUrl, userToken, device);

				Task.Run (async () => {

					var deviceInfo = await deviceController.GetDeviceInfoByIdAsync (userToken.Token, device.Id);

//					var testVariable = deviceInfo.VariableList.FirstOrDefault ();
//
//					var vResp = await cloudController.GetVariableAsync (testVariable.Name);
//
//					if(vResp != null)
//						Console.WriteLine (vResp.Result);

					var testFunction = deviceInfo.Functions.FirstOrDefault ();
					var fResp = await cloudController.CallFunctionAsync (testFunction, "test");

					if(fResp != null)
						Console.WriteLine (fResp.Return_Value);	
					
				}).Wait();
			}
		}
	}
}