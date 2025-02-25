using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;

using System.Web;
using System.Web.Mvc;

using System.IO;
using System.Linq;

using HydrogenCms.Models;

namespace HydrogenCms.Utilities
{
	public static class SkinHelper
	{
		private struct Keys
		{
			public const string DefaultSkin = "Default";
		}

		/// <summary>
		/// Extension to controller.
		/// </summary>
		/// <param name="controller"></param>
		/// <param name="useConfiguredSkin"></param>
		public static void SetSkin(this Controller controller, bool useConfiguredSkin)
		{
			if (useConfiguredSkin)
			{
				SetSkin(controller, Application.Settings.Skin);
			}
			else
			{
				SetSkin(controller, Keys.DefaultSkin);
			}
		}

		/// <summary>
		/// Extension to controller.
		/// </summary>
		/// <param name="controller"></param>
		/// <param name="skinName"></param>
		public static void SetSkin(this Controller controller, string skinName)
		{
			if (controller != null)
			{
				if (string.IsNullOrEmpty(skinName))
				{
					skinName = Keys.DefaultSkin;
				}

				controller.ViewEngine = controller.ViewEngine;

				ViewLocator viewLocator = (controller.ViewEngine as WebFormViewEngine).ViewLocator as ViewLocator;
				//
				// Should we append these new formats, or replace the old ones?  Right now, we are replacing.
				//
				viewLocator.ViewLocationFormats = new string[] { "~/Views/" + skinName + "/{1}_{0}.aspx" };
				// viewLocator.MasterLocationFormats = new string[] { "~/Views/" + skinName + "/{0}.master" };
			}
		}

		/// <summary>
		/// Extension to controller.
		/// </summary>
		/// <param name="controller"></param>
		public static void SetSkin(this Controller controller)
		{
			SetSkin(controller, true);
		}

		public static List<string> ListInstalledSkins()
		{
			string viewPath = HttpContext.Current.Server.MapPath("~/Views/");

			List<string> skins = (from directory in Directory.GetDirectories(viewPath) select Path.GetFileName(directory)).Where(s => s != "Sitemap").ToList();

			return skins;
		}
	}
}
