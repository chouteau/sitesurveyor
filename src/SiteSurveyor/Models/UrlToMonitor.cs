using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteSurveyor.Models
{
	public class UrlToMonitor
	{
		public string Url { get; set; }
		public List<string> PhoneNumberList { get; set; }
	}
}
