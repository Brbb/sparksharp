using System;

namespace Spark
{
	public class Configuration
	{
		private static String deviceId = "<YOUR_DEVICE_ID>";
		private static String accessToken = "<YOUR_ACCESS_TOKEN>";

		// TODO: Read deviceId and accessToken from local storage file or login result.
		public Configuration()
		{

		}

		public static String DeviceID { get {return deviceId;}}
		public static String AccessToken { get {return accessToken;}}
	}
}

