using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using HydrogenCms.Utilities;

namespace HydrogenCms.Controllers
{
	public class AdminController : Controller
	{
		public ActionResult Index()
		{
			Models.ViewContainers.AdminView adminView = new Models.ViewContainers.AdminView();
			adminView.AllPages = Models.Data.DataService.Page_ListAll(false);
			adminView.ControllerName = "Admin";

			adminView.MediaPath = Application.Settings.MediaPath;
			adminView.SiteName = Application.Settings.SiteName;
			adminView.SiteTagLine = Application.Settings.SiteTagline;
			adminView.Skin = Application.Settings.Skin;
			adminView.Skins = Utilities.SkinHelper.ListInstalledSkins();
			adminView.UserId = Application.Settings.UserId;

			this.SetSkin();
			return this.View("Index", adminView);
		}

		public ActionResult Update()
		{
			Models.ViewContainers.AdminView adminView = new Models.ViewContainers.AdminView();

			adminView.MediaPath = Request.Form["MediaPath"];
			adminView.SiteName = Request.Form["SiteName"];
			adminView.SiteTagLine = Request.Form["SiteTagLine"];
			adminView.Skin = Request.Form["Skin"];
			adminView.UserId = Request.Form["UserId"];
			adminView.Password = Request.Form["Password"];

			//
			// jes - validate
			//

			try
			{
				//
				// jes - We need transactions.
				//
				Application.Settings.MediaPath = adminView.MediaPath;
				Application.Settings.SiteName = adminView.SiteName;
				Application.Settings.SiteTagline = adminView.SiteTagLine;
				Application.Settings.Skin = adminView.Skin;
				Application.Settings.UserId = adminView.UserId;
				if (!string.IsNullOrEmpty(adminView.Password))
				{
					Application.Settings.HashedPassword = Utilities.Security.HashPassword(adminView.Password);
				}

				this.TempData.Clear();
				this.TempData.Add("adminSuccess", "Settings saved successfully.");
			}
			catch (System.Exception exception)
			{
				adminView.Password = string.Empty;
				this.TempData.Add("adminView", adminView);
				this.TempData.Add("adminError", exception.Message); // jes - Dont send the exception back to the client!

				return RedirectToAction("Index");
			}

			return RedirectToAction("Index");
		}
	}
}
