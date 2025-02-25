using System;
using System.Collections;
using System.Configuration;
using System.Web;
using System.Web.Caching;

namespace HydrogenCms.Utilities
{
	/// <summary>
	/// Later, this class will use an ICache provider to enable httpContext, EntLib, or memcache caching.
	/// <remarks>
	/// Taken from http://blog.falafel.com/CategoryView,category,C%23.aspx
	/// 
	/// This class has a different scope than the one referenced in the url above.  Basically, its a bottomless pit cache, untill invalidated.
	/// Most likely, the concepts in this class are excellent, but this implementation is too narrow for reuse outside in a bigger project, as is.
	/// </remarks>
	/// </summary>
	public class CacheHelper
	{
		public delegate T Fetcher<T, D>(D data);
		public delegate T ParameterlessFetcher<T>();

		public static T GetFromCache<T, D>(string key, string[] dependencies, Fetcher<T, D> fetcher, D data)
		{
			T result = GetFromCache<T>(key);

			if (result == null)
			{
				result = fetcher(data);

				if (result != null)
				{
					AddToCache(key, dependencies, result);
				}
			}

			return result;
		}

		public static T GetFromCache<T>(string key, string[] dependencies, ParameterlessFetcher<T> fetcher)
		{
			T result = GetFromCache<T>(key);

			if (result == null)
			{
				result = fetcher();

				if (result != null)
				{
					AddToCache(key, dependencies, result);
				}
			}
			return result;
		}

		public static T GetFromCache<T>(string key)
		{
			if (HttpContext.Current != null)
			{
				return (T)HttpContext.Current.Cache[key];
			}
			else
			{
				return default(T);
			}
		}

		private static void EnsureDependency(string dependencyKey)
		{
			if (HttpContext.Current.Cache[dependencyKey] == null)
			{
				HttpContext.Current.Cache[dependencyKey] = DateTime.Now;
			}
		}

		public static void AddToCache(string key, string[] dependencies, object dataToCache)
		{
			if (HttpContext.Current != null)
			{
				if (dependencies != null && dependencies.Length > 0)
				{
					foreach (string dependency in dependencies)
					{
						EnsureDependency(dependency);
					}
				}

				HttpContext.Current.Cache.Insert(key, dataToCache, new CacheDependency(null, dependencies), Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration);
			}
		}

		public static void RemoveFromCache(string key)
		{
			if (HttpContext.Current != null)
			{
				HttpContext.Current.Cache.Remove(key);
			}
		}
	}
}
