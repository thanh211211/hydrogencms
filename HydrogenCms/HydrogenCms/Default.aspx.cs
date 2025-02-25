using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

namespace HydrogenCms
{
	public partial class Default : System.Web.UI.Page
	{
		public void Page_Load(object sender, System.EventArgs e)
		{
			string redirectUrl = "~/Page" + (GlobalApplication.Iis6Enabled ? ".aspx" : string.Empty) + "/Show";

			try
			{
				Models.Page page = Models.Data.DataService.Page_GetFrontPage();

				if (page != null)
				{
					redirectUrl = "~/" + page.Slug + (GlobalApplication.Iis6Enabled ? ".aspx" : string.Empty);
				}
			}
			catch (System.Exception exception)
			{
				System.Diagnostics.Debug.WriteLine(exception.Message);
			}

			Response.Redirect(redirectUrl);
		}
	}
}
