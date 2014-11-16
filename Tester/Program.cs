using System;
using Spark;

namespace Tester
{
	class MainClass
	{
		public static void Main (string[] args)
		{
//			Spark.SSECore.SSEPublished +=(eve) =>   {
//				Console.WriteLine(eve.Info.Data);
//			};
//			Spark.SSECore.OpenSSEStream(String.Format("https://api.spark.io/v1/devices/{0}/events/?access_token={1}",Spark.Configuration.DeviceID,Spark.Configuration.AccessToken));
//

			var auth = Spark.RESTCore.PostAuthAsync ("user", "password").Result;
			if (auth != null)
				Configuration.AccessToken = auth.Access_Token;

			Spark.VariableResponse response =  Spark.RESTCore.GetVariableAsync("temperature").Result;
			Console.WriteLine(response.Result);

			var tokens = Spark.RESTCore.ListTokensAsync ("user", "password").Result;

			var result = Spark.RESTCore.CallFunctionAsync("brew").Result;
		}
	}
}
