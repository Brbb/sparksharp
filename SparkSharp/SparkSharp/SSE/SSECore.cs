using System;
using System.IO;
using System.Net;
using System.Text;
using System.Collections.Generic;
using Spark;

namespace Spark
{

	public class SSECore {
		private static List<string> Queue = new List<string>(1024);

		public static void OpenSSEStream(string url) {
			/*
            Optionally ignore certificate errors
            ServicePointManager.ServerCertificateValidationCallback =
             new System.Net.Security.RemoteCertificateValidationCallback(AcceptAllCertifications);
        */

			// https://api.spark.io/v1/devices/DEVICE_ID/events/?access_token=ACCESS_TOKEN

			var request = WebRequest.Create( new Uri(url));
			((HttpWebRequest)request).AllowReadStreamBuffering = false;
			var response = request.GetResponse();


			var encoding = ASCIIEncoding.ASCII;
			var stream = response.GetResponseStream();

			SSECore.ReadStreamForever(stream,encoding);
		}

		static void ReadStreamForever(Stream stream,Encoding encoding) {
			using (var reader = new System.IO.StreamReader(stream, encoding))
			{
				// handle a better cycle
				// this seems to lock SparkCore for a while...
				while (true) {
					string responseText = reader.ReadLine ();
					Push (responseText);
				}
			}
		}

		static void Push(string text) {
			if (String.IsNullOrWhiteSpace(text)) {
				return;
			}

			var lines = text.Trim().Split('\n');
			SSECore.Queue.AddRange(lines);

			if (text.Contains("data:")) {
				SSECore.ProcessLines();
			}
		}

		static void ProcessLines() {
			var lines = SSECore.Queue;

			SSEvent lastEvent = null;
			int lastEventIdx = -1;

			for(int i=0;i<lines.Count;i++) {
				var line = lines[i];
				if (String.IsNullOrWhiteSpace(line)) {
					continue;
				}
				line = line.Trim();

				if (line.StartsWith("event:")) {
					lastEvent = new SSEvent() {
						Name = line.Replace("event:", String.Empty)
					};
				}
				else if (line.StartsWith("data:")) {
					if (lastEvent == null) {
						continue;
					}


					lastEvent.SetData(line.Replace("data:", String.Empty));

					lastEventIdx = i;
				}
			}

			Publish (lastEvent);

			//trim previously processed events
			if (lastEventIdx >= 0) {
				lines.RemoveRange(0, lastEventIdx); 
			}
		}

		#region Event Publishing

		private static void Publish(SSEvent eve)
		{
			if (SSEPublished != null)
				SSEPublished (eve);
		}

		//This delegate can be used to point to methods
		//which return void and take a string.
		public delegate void SSEPublishedHandler(SSEvent eve);

		//This event can cause any method which conforms
		//to SSEPublishedHandler to be called.
		public static event SSEPublishedHandler SSEPublished;

		#endregion
	}
}
