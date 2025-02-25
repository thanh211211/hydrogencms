using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using HydrogenCms.Utilities;

namespace HydrogenCms.Controllers
{
	public class PageController : Controller
	{
		public ActionResult Show(string slug)
		{
			Models.ViewContainers.PageView pageView = new HydrogenCms.Models.ViewContainers.PageView();
			pageView.ControllerName = "Page";

			if (string.IsNullOrEmpty(slug))
			{
				pageView.Page = Models.Data.DataService.Page_GetFrontPage();
			}
			else
			{
				pageView.Page = Models.Data.DataService.Page_GetBySlug(slug);
			}

			if (pageView.Page == null)
			{
				// 404
				throw new System.Web.HttpException(404, "Page not found.");
			}
			else
			{
				pageView.AllPages = Models.Data.DataService.Page_ListAll(false);

				this.SetSkin();

				Models.Page.OnServing(pageView.Page, null);

				return this.View("Show", pageView);
			}
		}
	}
}
