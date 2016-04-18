using System;

namespace Particle
{
	public class AccessToken
	{
		public AccessToken ()
		{
		}

		public string Token { get; set; }
		public string Expires_At { get; set; }
		public string Client { get; set; }
	}
}

