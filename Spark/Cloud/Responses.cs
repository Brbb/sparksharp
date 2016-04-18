using System;

namespace Particle
{
	
	public class VariableResponse
	{
		public string Cmd{ get; set; }
		public string Name { get; set; }
		public string Result { get; set; }

		public CoreInfo CoreInfo {get;set; }
	}

	public class AuthResponse
	{
		public string Access_Token { get; set;}
		public string Token_Type {get;set;}
		public long Expires_In { get; set;}
		public string Refresh_Token {get;set;}
	}

	public class DeleteResponse
	{
		public bool OK {get;set;}
	}

	public class FunctionResponse
	{
		public string ID { get; set;}
		public string Name { get; set;}
		public bool Connected { get; set;}
		public string Return_Value{ get; set;}
	}
}

