using System;

namespace Spark
{
	
	public class SparkResponse
	{
		public string Cmd{ get; set; }
		public string Name { get; set; }
		public string Result { get; set; }

		public CoreInfo CoreInfo {get;set; }
	}
}

