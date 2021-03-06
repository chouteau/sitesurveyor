﻿using System;
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
				var result = IsDown(item.Url).Result;
				if (result.IsDown)
				{
					System.Diagnostics.Trace.TraceError(string.Format("error url : {0} {1}", item.Url, result.ErrorMessage), "error");

					Alert.Add(item.Url);

					if (Alert.IsAlreadyFailed(item.Url))
					{
						try
						{
							SendSMS(item, result.ErrorMessage);
						}
						catch { }
						try
						{
							SendEmail(item, result.ErrorMessage);
						}
						catch { }
					}
					else 
					{
						var history = m_HistoryList.SingleOrDefault(i => i.Url == item.Url);
						if (history != null
							&& DateTime.Now > history.LastSentErrorDate)
						{
							Alert.Remove(item.Url);
						}
					}
				}
			}
		}

		private void SendSMS(Models.UrlToMonitor item, string errorMessage)
		{
			if (item.PhoneNumberList == null
				|| item.PhoneNumberList.Count == 0)
			{
				return;
			}
			var smsService = new SmsService();
			var message = new Models.Message();
			message.Content = $"[{item.Url}] down detected from [{System.Environment.MachineName}] ! {errorMessage}";
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

		private void SendEmail(Models.UrlToMonitor item, string errorMessage)
		{
			if (item.EmailList == null
				|| item.EmailList.Count == 0)
			{
				return;
			}
			var smtpClient = new System.Net.Mail.SmtpClient();
			var message = new System.Net.Mail.MailMessage();

			foreach (var email in item.EmailList)
			{
				try
				{
					message.To.Add(email);
					message.Subject = $"[{item.Url}] down detected from [{System.Environment.MachineName}]";
					message.Body = errorMessage;
					smtpClient.Send(message);
				}
				catch
				{

				}
			}
		}

		private async Task<Models.StatusResult> IsDown(string url)
		{
			var handler = new HttpClientHandler()
			{
				AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip,
				AllowAutoRedirect = false,
				UseCookies = true
			};

			var httpclient = new HttpClient(handler);
			httpclient.Timeout = TimeSpan.FromSeconds(10);
			httpclient.DefaultRequestHeaders.Add("UserAgent", "SiteSurveyor/1.0");

			var result = new Models.StatusResult();
			await httpclient.GetAsync(url).ContinueWith(task =>
			{
				if (task.IsCanceled)
				{
					result.IsDown = true;
					result.ErrorMessage = "This site cant be reached";
				}
				else if (task.IsFaulted)
				{
					result.IsDown = true;
					result.ErrorMessage = GetErrorMessage(task.Exception);
				}
				else
				{
					try
					{
						var response = task.Result;
						response.EnsureSuccessStatusCode();
						result.IsDown = false;
					}
					catch (Exception ex)
					{
						result.ErrorMessage = GetErrorMessage(ex);
					}
				}
			});

			return result;
		}

		private string GetErrorMessage(Exception ex)
		{
			var x = ex;
			while (true)
			{
				if (x.InnerException == null)
				{
					break;
				}
				x = x.InnerException;
			}
			return x.Message;
		}
	}
}
