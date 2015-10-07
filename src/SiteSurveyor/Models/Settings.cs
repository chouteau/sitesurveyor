using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteSurveyor.Models
{
	public class Settings
	{
		public string Nic { get; set; }
		public string Password { get; set; }
		public string Language { get; set; }
		public string FromPhoneNumber { get; set; }
		public bool IsMultiSession { get; set; }
		public string Account { get; set; }
		public string PhonePrefix { get; set; }
	}
}
