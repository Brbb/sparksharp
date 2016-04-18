using System;
using System.Xml;
using System.Xml.Linq;
using System.Linq;


namespace Particle
{
	public class Configuration
	{
		// TODO: Read deviceId and accessToken from local storage file or login result.
		public Configuration()
		{

		}


		public static void CheckForExistingCredentials ()
		{
			string credentials = @"	<Credentials>
										<DeviceID>1234556677</DeviceID>
										<AccessToken>alongalphadecimalstring123</AccessToken>
									</Credentials>";

			var xdoc = XDocument.Parse (credentials);
			var deviceID = xdoc.Descendants ("DeviceID").FirstOrDefault ();
			var accessToken = xdoc.Descendants ("AccessToken").FirstOrDefault ();

			if (deviceID != null)
				Configuration.DeviceID = deviceID.Value;

			if (accessToken != null)
				Configuration.AccessToken = accessToken.Value;
		}
	}
}

