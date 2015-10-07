using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Flurl;

namespace SiteSurveyor.Service
{
	public class SmsService
	{
		public SmsService()
		{
			Settings = new Models.Settings();
			Settings.BindFromConfiguration(System.Configuration.ConfigurationManager.AppSettings);
		}

		protected Models.Settings Settings { get; set; }

		public void SendSMS(Models.Message message)
		{
			var from = Settings.FromPhoneNumber;
			var phonePrefix = Settings.PhonePrefix;

			var handler = new HttpClientHandler()
			{
				AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip,
				AllowAutoRedirect = false,
				UseCookies = true
			};
			var httpclient = new HttpClient(handler);
			httpclient.BaseAddress = new Uri("https://www.ovh.com");

			var request = "cgi-bin/sms/http2sms.cgi"
							.SetQueryParam("account", Settings.Account)
							.SetQueryParam("login", Settings.Nic)
							.SetQueryParam("password", Settings.Password)
							.SetQueryParam("from", from)
							.SetQueryParam("to", message.MobileNumber)
							.SetQueryParam("message", message.Content)
							.SetQueryParam("noStop", "1");

            var response = httpclient.GetAsync(request);
			if (response.Result.StatusCode == HttpStatusCode.OK)
			{
				var content = response.Result.Content.ReadAsStringAsync().Result;
			}

		}

		private string CleanPhoneNumber(string input)
		{
			if (input == null)
			{
				return string.Empty;
			}

			// Suppression de tous les caractères qui ne sont pas des chiffres
			input = System.Text.RegularExpressions.Regex.Replace(input, @"\D", "");

			return input;
		}

	}
}
