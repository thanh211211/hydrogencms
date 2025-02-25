using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;

namespace HydrogenCms.Controls
{
	public partial class Header : System.Web.Mvc.ViewUserControl<Models.ViewContainers.ViewContainer>
	{
		public string RenderHeader()
		{
			Models.Page page = this.ViewData.Model.Page;

			if (page == null)
			{
				return string.Empty;
			}
			else
			{
				Models.Meta[] siteMeta = Models.Data.DataService.Meta_ListForSite();

				StringBuilder stringBuilder = new StringBuilder();
				//
				// When we moved to master pages, we couldn't use this anymore.  I liked it.  It was better....
				//
				// stringBuilder.Append("<title>" + HttpUtility.HtmlEncode(page.Title) + "</title>" + Environment.NewLine);

				List<Models.Meta> combinedMeta = page.Metas.ToList();

				foreach (Models.Meta meta in siteMeta)
				{
					if (combinedMeta.Where(m => m.Name.Equals(meta.Name, StringComparison.InvariantCultureIgnoreCase)).Count() == 0)
					{
						combinedMeta.Add(meta);
					}
				}

				foreach (Models.Meta meta in combinedMeta)
				{
					stringBuilder.Append("<meta name=\"" + HttpUtility.HtmlEncode(meta.Name) + "\" content=\"" + HttpUtility.HtmlEncode(meta.Content) + "\" />" + Environment.NewLine);
				}

				return stringBuilder.ToString();
			}
		}
	}
}
