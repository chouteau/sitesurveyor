using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteSurveyor.Models
{
	public class StatusResult
	{
		public StatusResult()
		{
			IsDown = false;
		}
		public bool IsDown { get; set; }
		public string ErrorMessage { get; set; }
	}
}
