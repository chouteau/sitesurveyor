﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteSurveyor
{
	public static class Utils
	{
		public static void BindFromConfiguration(this object model, System.Collections.Specialized.NameValueCollection nvc)
		{
			if (model == null
				|| nvc == null)
			{
				return;
			}

			var propertyInfoList = model.GetType().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.GetProperty);
			foreach (var propertyInfo in propertyInfoList)
			{
				if (!propertyInfo.CanWrite)
				{
					continue;
				}

				var key = nvc.AllKeys.SingleOrDefault(i => i.Equals(propertyInfo.Name, StringComparison.InvariantCultureIgnoreCase));
				if (key == null)
				{
					continue;
				}

				var value = nvc[key];
				if (value == null)
				{
					continue;
				}

				var typedValue = System.Convert.ChangeType(value, propertyInfo.PropertyType);
				propertyInfo.SetValue(model, typedValue, null);
			}
		}

	}
}
