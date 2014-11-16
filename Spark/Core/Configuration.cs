using System;

namespace Spark
{
	public class Configuration
	{
		// TODO: Read deviceId and accessToken from local storage file or login result.
		public Configuration()
		{

		}

		public static String DeviceID {
			get;
			set;
		}
		public static String AccessToken { 
			get;
			set;
		}
	}
}

