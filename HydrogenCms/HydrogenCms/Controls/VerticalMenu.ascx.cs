using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HydrogenCms.Controls
{
	public partial class VerticalMenu : System.Web.Mvc.ViewUserControl<Models.ViewContainers.ViewContainer>
	{
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
					buffer += WriteMenu(page, pages.ToList());
				}

				buffer += "<li" + (this.ViewData.Model.ControllerName == "Admin" ? " class=\"" + this.SelectedClassName + "\">" : ">") + Html.ActionLink(HttpUtility.HtmlEncode("Admin"), "Index", "Admin") + "</li>\n";

				buffer += "</ul>\n";

				return buffer;
			}
		}

		private string WriteMenu(Models.Page parent, List<Models.Page> allPages)
		{
			string buffer = string.Empty;

			if (this.ViewData.Model.Page == null)
			{
				buffer = "<li>";
			}
			else
			{
				buffer = "<li" + (parent.Slug == this.ViewData.Model.Page.Slug && !string.IsNullOrEmpty(this.SelectedClassName) ? " class=\"" + this.SelectedClassName + "\">" : ">");
			}

			buffer += Html.ActionLink(HttpUtility.HtmlEncode(parent.Title), "Show", "Page", new { slug = parent.Slug }) + "\n";

			List<Models.Page> childPages = allPages.Where(p => p.ParentId == parent.PageId).ToList();

			if (childPages.Count > 0)
			{
				foreach (Models.Page page in childPages)
				{
					buffer += "<ul>" + WriteMenu(page, allPages) + "</ul>";
				}
			}

			buffer += "</li>\n";

			return buffer;
		}
	}
}
