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



			Spark.VariableResponse response =  Spark.RESTCore.GetVariableAsync("temperature").Result;
			Console.WriteLine(response.Result);

		}
	}
}