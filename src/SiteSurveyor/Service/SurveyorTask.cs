using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TasksOnTime;

namespace SiteSurveyor.Service
{
	public class SurveyorTask : TasksOnTime.ITask
	{
		public static List<Models.History> m_HistoryList = new List<Models.History>();

		public void Execute(ExecutionContext context)
		{
			var currentPath = System.IO.Path.GetDirectoryName(this.GetType().Assembly.Location);
			var fileName = System.IO.Path.Combine(currentPath, "urltomonitor.json");

			if (!System.IO.File.Exists(fileName))
			{
				return;
			}

			var content = System.IO.File.ReadAllText(fileName);
			var urlList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Models.UrlToMonitor>>(content);

			foreach (var item in urlList)
			{
				System.Diagnostics.Trace.WriteLine(string.Format("Check url : {0}", item.Url), "info");
				if (IsDown(item.Url))
				{
					System.Diagnostics.Trace.TraceError(string.Format("error url : {0}", item.Url), "error");

					var smsService = new SmsService();
					var message = new Models.Message();
					message.Content = string.Format("site [{0}] is down !", item.Url);
					foreach (var phone in item.PhoneNumberList)
					{
						message.MobileNumber = phone;
						smsService.SendSMS(message);

						m_HistoryList.Add(new Models.History()
						{
							LastSentErrorDate = DateTime.Now,
							PhoneNumber = phone,
							Url = item.Url,
						});
					}
				}
			}
		}

		private bool IsDown(string url)
		{
			var handler = new HttpClientHandler()
			{
				AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip,
				AllowAutoRedirect = false,
				UseCookies = true
			};

			var httpclient = new HttpClient(handler);
			httpclient.DefaultRequestHeaders.Add("UserAgent", "SiteSurveyor/1.0");

			var result = false;
			var response = httpclient.GetAsync(url).ContinueWith((task) =>
            {
				if (task.IsFaulted)
				{
					result = true;
				}
				if (task.Result.StatusCode != HttpStatusCode.OK)
				{
					result = true;
				}
			}).Wait(5 * 1000);

			return result;
		}
	}
}
