using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteSurveyor.Models
{
	public class History
	{
		public string Url { get; set; }
		public string PhoneNumber { get; set; }
		public DateTime? LastSentErrorDate { get; set; }
	}
}
