using System;
using System.IO;
using System.Net;
using System.Text;
using System.Collections.Generic;
using Particle;
using RestSharp;

namespace Particle
{
	public class SSECore {

		public SSECore(string baseUrl,string accessToken){
			BaseUrl = baseUrl;
			AccessToken = accessToken;
		}

		private static List<string> Queue = new List<string>(1024);
		public string BaseUrl { get ; private set; }
		public string AccessToken { get; private set; }


		// https://api.particle.io/v1/events/temp?access_token=1234

		public void GetStreamOfEvents(string eventPrefix = ""){

			var request = new RestRequest ("v1/events/{eventPrefix}");

			if (!String.IsNullOrEmpty (eventPrefix)) {
				request.AddParameter("eventPrefix", eventPrefix);
			}

			request.AddQueryParameter ("access_token", AccessToken);


			var encoding = ASCIIEncoding.ASCII;

			var client = new RestClient (BaseUrl);
			request.ResponseWriter = (responseStream) => ReadStreamForever (responseStream, encoding);
			client.DownloadData (request);

		}
			
		public void GetStreamOfDeviceEvents(string deviceId,string eventPrefix = ""){
			var request = new RestRequest ("v1/devices/{deviceId}/events/{eventPrefix}");

			request.AddQueryParameter ("deviceId", deviceId);

			if (!String.IsNullOrEmpty (eventPrefix)) {
				request.AddParameter("eventPrefix", eventPrefix);
			}

			request.AddQueryParameter ("access_token", AccessToken);


			var encoding = ASCIIEncoding.ASCII;

			var client = new RestClient (BaseUrl);
			request.ResponseWriter = (responseStream) => ReadStreamForever (responseStream, encoding);
			client.DownloadData (request);
		}

		public void PublishAnEvent(){
		}

	 void ReadStreamForever(Stream stream,Encoding encoding) {
			using (var reader = new System.IO.StreamReader (stream, encoding)) {
				// handle a better cycle
				// this seems to lock SparkCore for a while...
				while (true) {
					string responseText = reader.ReadLine ();
					Push (responseText);
					Console.WriteLine ("Debug " + responseText);
				}
			}
		}

		 void Push(string text) {
			if (String.IsNullOrWhiteSpace (text)) {
				return;
			}

			var lines = text.Trim ().Split ('\n');
			SSECore.Queue.AddRange (lines);

			if (text.Contains ("data:")) {
				ProcessLines ();
			}
		}

		 void ProcessLines() {
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

		private void Publish(SSEvent eve)
		{
			if (SSEPublished != null)
				SSEPublished (eve);
		}

		//This delegate can be used to point to methods
		//which return void and take a string.
		public delegate void SSEPublishedHandler(SSEvent eve);

		//This event can cause any method which conforms
		//to SSEPublishedHandler to be called.
		public event SSEPublishedHandler SSEPublished;

		#endregion
	}
}
