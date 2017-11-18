using System.IO;
using System.Net;

namespace FindFotoUrl
{
	internal class Vkapi
	{
		private string url = "https://api.vk.com/method/";

		public string Get(string methodName, string param)
		{
			WebRequest webRequest = WebRequest.Create(string.Concat(url, methodName, "?", param,"&v=5.34","&access_token=", User.Default.Token));
			WebResponse response = webRequest.GetResponse();
			Stream responseStream = response.GetResponseStream();
		    if (responseStream != null)
		    {
		        StreamReader streamReader = new StreamReader(responseStream);
		        string result = streamReader.ReadToEnd();
		        streamReader.Close();
		        return result;
		    }
		    return null;
		}
	}
}
