using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Configuration;
using System.Collections;

namespace HydrogenCms
{
	public class GlobalApplication : System.Web.HttpApplication
	{
		private struct Keys
		{
			public const string SECTION_GLOBAL_APPLICATION = "HydrogenCms.GlobalApplication";
			public const string SETTING_IIS6_ENABLED = "iis6Enabled";
		}

		#region Iis6Enabled

		private static readonly bool _iis6Enabled;

		/// <summary>
		/// Iis 6 mode enabled.
		/// </summary>
		public static bool Iis6Enabled
		{
			get
			{
				return _iis6Enabled;
			}
		}

		#endregion

		static GlobalApplication()
		{
			Hashtable settings = ConfigurationManager.GetSection(Keys.SECTION_GLOBAL_APPLICATION) as Hashtable;

			if (settings != null)
			{
				_iis6Enabled = bool.Parse(settings[Keys.SETTING_IIS6_ENABLED].ToString());
			}
		}

		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			/*
			routes.MapRoute(
				"Default",                                              // Route name
				"{controller}/{action}/{id}",                           // URL with parameters
				new { controller = "Page", action = "Show", slug = string.Empty }  // Parameter defaults
			);

			return;
			*/




			// Note: Change the URL to "{controller}.mvc/{action}/{id}" to enable
			//       automatic support on IIS6 and IIS7 classic mode

			//
			// Default route.
			//
			// routes.Add(new Route("Default.aspx", new MvcRouteHandler())	{ Defaults = new { controller = "Page", action = "Show", slug = string.Empty }) });
			routes.MapRoute("Default", "Default.aspx", new { controller = "Page", action = "Show", slug = string.Empty });

			if (_iis6Enabled)
			{
				routes.MapRoute("page", "{slug}.aspx", new { controller = "Page", action = "Show", slug = string.Empty });
				routes.MapRoute("page-action", "{controller}.aspx/{action}", new { controller = "Page", action = "Show", slug = string.Empty });
				routes.MapRoute("page-action-slug", "{controller}.aspx/{action}/{slug}", new { controller = "Page", action = "Show", slug = string.Empty });
			}
			else
			{
				routes.MapRoute("page", "{slug}", new { controller = "Page", action = "Show", slug = string.Empty });
				routes.MapRoute("page-action", "{controller}/{action}", new { controller = "Page", action = "Show", slug = string.Empty });
				routes.MapRoute("page-action-slug", "{controller}/{action}/{slug}", new { controller = "Page", action = "Show", slug = string.Empty });
			}
		}

		protected void Application_Start(object sender, EventArgs e)
		{
			RegisterRoutes(RouteTable.Routes);
		}
	}
}