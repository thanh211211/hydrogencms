using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HydrogenCms.Views.Default
{
	public partial class Site : ViewMasterPage
	{
		public Site()
		{
			this.PreRender += new EventHandler(Site_PreRender);
		}

		void Site_PreRender(object sender, EventArgs e)
		{
			Models.ViewContainers.ViewContainer viewContainer = this.ViewContext.ViewData.Model as Models.ViewContainers.ViewContainer;

			if (viewContainer != null)
			{
				if (viewContainer.Page != null)
				{
					this.Page.Title = Html.Encode(viewContainer.Page.Title);
				}
			}
		}
	}
}
