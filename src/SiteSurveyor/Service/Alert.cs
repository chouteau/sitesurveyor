using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteSurveyor.Service
{
	public class Alert
	{
		private class AlertItem
		{
			public string Url { get; set; }
			public int AlertCount { get; set; }
		}

		private static Lazy<Alert> m_Alert = new Lazy<Alert>(() =>
		{
			var result = new Alert();
			return result;
		}, true);

		private List<AlertItem> AlertPendingList;

		private Alert()
		{
			AlertPendingList = new List<AlertItem>();
		}

		public static void Add(string url)
		{
			var alert = m_Alert.Value.AlertPendingList.SingleOrDefault(i => i.Url == url);
			if (alert == null)
			{
				alert = new AlertItem()
				{
					Url = url
				};
				m_Alert.Value.AlertPendingList.Add(alert);
			}
			alert.AlertCount++;
		}

		public static void Remove(string url)
		{
			var alert = m_Alert.Value.AlertPendingList.SingleOrDefault(i => i.Url == url);
			if (alert != null)
			{
				m_Alert.Value.AlertPendingList.Remove(alert);
			}
		}

		public static bool IsAlreadyFailed(string url)
		{
			var alert = m_Alert.Value.AlertPendingList.SingleOrDefault(i => i.Url == url);
			if (alert != null
				&& alert.AlertCount > 1)
			{
				return true;
			}
			return false;
		}

	}
}
