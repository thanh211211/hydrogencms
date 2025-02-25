using System;
using System.Collections.Generic;
using System.Text;

namespace HydrogenCms.Utilities
{
	public class ReflectionHelper
	{
		public static T CreateObject<T>(string fullTypeName, params object[] constructorParameters)
		{
			return (T)CreateObject(fullTypeName, constructorParameters);
		}

		public static object CreateObject(string fullTypeName, params object[] constructorParameters)
		{
			string cacheKey = fullTypeName;
			Type type = null;

			if (System.Web.HttpContext.Current != null)
			{
				type = System.Web.HttpContext.Current.Cache[cacheKey] as System.Type;
			}

			if (type == null)
			{
				try
				{
					type = Type.GetType(fullTypeName, true);
					if (System.Web.HttpContext.Current != null)
					{
						System.Web.HttpContext.Current.Cache[cacheKey] = type;
					}
				}
				catch (Exception exception)
				{
					System.Diagnostics.Debug.WriteLine(exception.ToString());
					throw;
				}
			}

			return Activator.CreateInstance(type, constructorParameters);
		}
	}
}

