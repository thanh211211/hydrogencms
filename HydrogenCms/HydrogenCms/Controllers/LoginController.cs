using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HydrogenCms.Utilities;
using System.Web.Security;

namespace HydrogenCms.Controllers
{
	public class LoginController : Controller
	{
		public ActionResult Index()
		{
			Models.ViewContainers.ViewContainer viewContainer = new Models.ViewContainers.ViewContainer();
			viewContainer.AllPages = Models.Data.DataService.Page_ListAll(false);
			viewContainer.ControllerName = "Login";

			this.SetSkin();
			return View("Index", viewContainer);
		}

		public ActionResult Login()
		{
			string userid = Request.Form["userid"];
			string password = Request.Form["password"];

			if (!string.IsNullOrEmpty(userid))
			{
				if (Utilities.Security.ValidateUser(userid, password))
				{
					if (string.IsNullOrEmpty(Request.Form["ReturnUrl"]))
					{
						FormsAuthentication.RedirectFromLoginPage(userid, false);
						return null;
					}
					else
					{
						FormsAuthentication.SetAuthCookie(userid, false);
						Response.Redirect(Request.Form["ReturnUrl"]);
						return null;
					}
				}
				else
				{
					this.TempData.Add("ReturnUrl", Request.Form["ReturnUrl"]);
					this.TempData.Add("loginError", "Invalid login");
					return this.RedirectToAction("Index", "Login");
				}
			}
			else
			{
				this.TempData.Add("ReturnUrl", Request.Form["ReturnUrl"]);
				this.TempData.Add("loginError", "Login required");
				return this.RedirectToAction("Index", "Login");
			}
		}

		public ActionResult Logout()
		{
			FormsAuthentication.SignOut();
			return this.RedirectToAction("Show", "Page");
		}
	}
}
