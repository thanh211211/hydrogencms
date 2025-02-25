using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HydrogenCms.Controls
{
	public partial class HorizontalMenu : System.Web.Mvc.ViewUserControl<Models.ViewContainers.ViewContainer>
	{
		//
		// jes - Doesnt do child pages as child menus, yet.  Right now, it either lists all pages, or the top pages.  Nothing else.  Very incomplete.
		//

		public string ClassName { get; set; }
		public string SelectedClassName { get; set; }
		public bool TopMenuOnly { get; set; }

		public string WriteMenu()
		{
			if (this.ViewData.Model.AllPages == null)
			{
				return string.Empty;
			}
			else
			{
				string buffer = "\n<ul" + (string.IsNullOrEmpty(this.ClassName) ? ">\n" : " class=\"" + this.ClassName + "\">\n");

				IEnumerable<Models.Page> pages = this.ViewData.Model.AllPages.AsEnumerable();

				if (this.TopMenuOnly)
				{
					pages = pages.Where(p => p.ParentId == new Guid());
				}

				foreach (Models.Page page in pages)
				{
					if (this.ViewData.Model.Page == null)
					{
						buffer += "<li>";
					}
					else
					{
						buffer += "<li" + (page.Slug == this.ViewData.Model.Page.Slug && !string.IsNullOrEmpty(this.SelectedClassName) ? " class=\"" + this.SelectedClassName + "\">" : ">");
					}
					buffer += Html.ActionLink(HttpUtility.HtmlEncode(page.Title), "Show", "Page", new { slug = page.Slug }) + "</li>\n";
				}

				// buffer += "<li" + (HttpContext.Current.Request.Url.ToString().IndexOf("/admin/", StringComparison.InvariantCultureIgnoreCase) >= 0 ? " class=\"" + this.SelectedClassName + "\">" : ">") + Html.ActionLink(HttpUtility.HtmlEncode("Admin"), "Index", "Admin", new { slug = "" }) + "</li>\n";
				buffer += "<li" + (this.ViewData.Model.ControllerName == "Admin" ? " class=\"" + this.SelectedClassName + "\">" : ">") + Html.ActionLink(HttpUtility.HtmlEncode("Admin"), "Index", "Admin") + "</li>\n";

				buffer += "</ul>\n";

				return buffer;
			}
		}
	}
}
