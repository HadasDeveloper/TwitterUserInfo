using System;
using System.Net;
using System.IO;
using Logger;


namespace oAuthTwitterWrapper
{
	public class Utility
	{
		public string RequstJson(string apiUrl, string tokenType, string accessToken)
		{
			var json = string.Empty;			
			HttpWebRequest apiRequest = (HttpWebRequest)WebRequest.Create(apiUrl);
			const string timelineHeaderFormat = "{0} {1}";
			apiRequest.Headers.Add("Authorization",
										string.Format(timelineHeaderFormat, tokenType,
													  accessToken));
			apiRequest.Method = "Get";
		    try
		    {
                WebResponse timeLineResponse = apiRequest.GetResponse();

			    using (timeLineResponse)
			    {
				    using (var reader = new StreamReader(timeLineResponse.GetResponseStream()))
				    {
					    json = reader.ReadToEnd();
				    }
			    }

		    }
            catch (WebException e)
            {
                Console.WriteLine("Utility.RequstJson : WebException = " + e.Status); 
                EventLogWriter logWriter = new EventLogWriter("oAuthTwitterWrapper");

                logWriter.WriteErrorToEventLog("Utility.RequstJson : WebException = " + e.Status);    

                //FileLogWriter fileLog = new FileLogWriter();
                //fileLog.WriteToLog(DateTime.Now, string.Format("Utility.RequstJson : WebException =  {0}, {1}", e.Message, e.Status), "Log");

                json = string.Format("Error: {0}",e.Message);
            }

			return json;
		}
	}
}
