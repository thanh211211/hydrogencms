using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;

namespace HydrogenCms.Views.Sitemap
{
	public partial class Index : ViewPage<HydrogenCms.Models.ViewContainers.ViewContainer>
	{
		public Index()
		{
			this.PreRender += new EventHandler(Sitemap_Index_PreRender);
		}

		void Sitemap_Index_PreRender(object sender, EventArgs e)
		{
			this.Response.ContentType = "text/xml";

			//
			// grrr...xml namespaces.  Seems like the more colon you put in something, the worse it gets.
			//
			
			XNamespace xn = "http://www.google.com/schemas/sitemap/0.84";

						UrlHelper u = new UrlHelper(this.ViewContext);

			XDocument xDocument = new XDocument(
				new XDeclaration("1.0", "utf-8", "yes"),
				new XElement(xn + "urlset",
					new XAttribute("xmlns", "http://www.google.com/schemas/sitemap/0.84"), 
					from p in this.ViewData.Model.AllPages
					select new XElement(xn + "url", 
						new XElement(xn + "loc", HttpContext.Current.Request.Url.ToString().Replace("Sitemap/Index", HttpUtility.UrlEncode(p.Slug)).Replace("Sitemap.aspx/Index", HttpUtility.UrlEncode(p.Slug + ".aspx"))),  // jes - there has to be a better way to do this.
						new XElement(xn + "lastmod", p.ModifiedOn.HasValue && p.ModifiedOn != DateTime.MinValue ? p.ModifiedOn.Value.ToString("yyyy-MM-dd") : p.CreatedOn.ToString("yyyy-MM-dd")), 
						new XElement(xn + "changefreq", "weekly"))));


			using (System.IO.TextWriter stringWriter = new System.IO.StringWriter())
			{
				xDocument.Save(stringWriter);
				Response.Write(stringWriter.ToString());
			}
		}
	}
}
