using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HydrogenCms.Controllers
{
	public class SitemapController : Controller
	{
		public ActionResult Index()
		{
			Models.ViewContainers.ViewContainer viewContainer = new Models.ViewContainers.ViewContainer();
			viewContainer.AllPages = Models.Data.DataService.Page_ListAll(false);
			viewContainer.ControllerName = "Sitemap";

			return this.View("Index", viewContainer);
		}
	}
}
