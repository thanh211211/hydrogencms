using System;
using System.Data.Linq;
using System.Linq;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Web;

using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Text;

namespace HydrogenCms.Models.Data
{
	public class LinqXmlDataContext : IDataContext
	{
		private struct Constants
		{
			public const string PageFile = "~/App_Data/xml/Page.xml";
			public const string MetaFile = "~/App_Data/xml/Meta.xml";
			public const string SettingFile = "~/App_Data/xml/Setting.xml";
		}

		List<Models.Page> _pages;
		List<Models.Meta> _metas;
		List<Models.Setting> _settings;

		#region Load

		private void LoadPages()
		{
			if (_pages == null)
			{
				LoadMetas();

				_pages = (from pages in XDocument.Load(HttpContext.Current.Server.MapPath(Constants.PageFile)).Descendants("Page")
						  select new Models.Page()
						  {
							  //
							  // PageId must be set before child objects (Metas) because it will reset the entity to have a null guid if not.  Sneaky bug.
							  //
							  PageId = new Guid(pages.Element("PageId").Value),
							  Content = pages.Element("Content").Value, 
							  CreatedBy = pages.Element("CreatedBy").Value, 
							  CreatedOn = DateTime.Parse(pages.Element("CreatedOn").Value), 
							  DisplayOrder = int.Parse(pages.Element("DisplayOrder").Value),
							  Metas = ToEntitySet<Models.Meta>(_metas.Where(m => m.PageId == new Guid(pages.Element("PageId").Value))),
							  ModifiedBy = pages.Element("ModifiedBy") == null ? null : pages.Element("ModifiedBy").Value,
							  ModifiedOn = pages.Element("ModifiedOn") == null ? DateTime.MinValue : DateTime.Parse(pages.Element("ModifiedOn").Value),
							  ParentId = new Guid(pages.Element("ParentId").Value), 
							  PublishDate = DateTime.Parse(pages.Element("PublishDate").Value),
							  Published = bool.Parse(pages.Element("Published").Value),
							  Slug = pages.Element("Slug").Value,
							  Title = pages.Element("Title").Value
						  }).ToList();
			}
		}

		private void LoadMetas()
		{
			if (_metas == null)
			{
				_metas = (from metas in XDocument.Load(HttpContext.Current.Server.MapPath(Constants.MetaFile)).Descendants("Meta")
						  select new Models.Meta()
							  {
								  Content = metas.Element("Content").Value,
								  MetaId = int.Parse(metas.Element("MetaId").Value),
								  Name = metas.Element("Name").Value,
								  PageId = new Guid(metas.Element("PageId").Value)
								  // Dont load the page associated with the metas.
								  // Page = Page_GetByPageId(new Guid(metas.Element("PageId").Value), true)
							  }).ToList();
			}
		}

		private void LoadSettings()
		{
			if (_settings == null)
			{
				_settings = (from settings in XDocument.Load(HttpContext.Current.Server.MapPath(Constants.SettingFile)).Descendants("Setting")
						  select new Models.Setting()
						  {
							  Name = settings.Element("Name").Value, 
							  SettingId = int.Parse(settings.Element("SettingId").Value), 
							  Value = settings.Element("Value").Value
						  }).ToList();
			}
		}

		#endregion

		#region Save

		private void SavePages()
		{
			if (_pages != null)
			{
				XElement xml = new XElement("Pages",
					from p in _pages
					select new XElement("Page",
						new XElement("Content", p.Content),
						new XElement("CreatedBy", p.CreatedBy),
						new XElement("CreatedOn", p.CreatedOn.ToString()),
						new XElement("DisplayOrder", p.DisplayOrder.ToString()),
						new XElement("ModifiedBy", p.ModifiedBy),
						new XElement("ModifiedOn", p.ModifiedOn == null ? DateTime.MinValue.ToString() : p.ModifiedOn.ToString()),
						new XElement("PageId", p.PageId.ToString()),
						new XElement("ParentId", p.ParentId == null ? new Guid().ToString() : p.ParentId.ToString()),
						new XElement("PublishDate", p.PublishDate.ToString()),
						new XElement("Published", p.Published.ToString()),
						new XElement("Slug", p.Slug),
						new XElement("Title", p.Title)));

				xml.Save(HttpContext.Current.Server.MapPath(Constants.PageFile));

				SaveMetas();

				_pages = null;
			}
		}

		private void SaveMetas()
		{
			if (_metas != null)
			{
				XElement xml = new XElement("Metas",
					from m in _metas
					select new XElement("Meta",
						new XElement("Content", m.Content),
						new XElement("MetaId", m.MetaId.ToString()),
						new XElement("Name", m.Name),
						new XElement("PageId", m.PageId.ToString())));

				xml.Save(HttpContext.Current.Server.MapPath(Constants.MetaFile));

				_metas = null;
			}
		}

		private void SaveSettings()
		{
			if (_settings != null)
			{
				XElement xml = new XElement("Settings",
					from s in _settings
					select new XElement("Setting",
						new XElement("Name", s.Name),
						new XElement("SettingId", s.SettingId.ToString()),
						new XElement("Value", s.Value)));

				xml.Save(HttpContext.Current.Server.MapPath(Constants.SettingFile));

				_settings = null;
			}
		}

		#endregion

		#region Page

		public void Page_Delete(Guid pageId)
		{
			this.LoadPages();
			this.LoadMetas();

			Models.Page page = _pages.Where(p => p.PageId == pageId).FirstOrDefault();

			if (page != null)
			{
				_pages.Remove(page);
				
				foreach (Models.Meta meta in _metas.Where(m => m.PageId == pageId))
				{
					_metas.Remove(meta);
				}

				SavePages();
				SaveMetas();
			}
		}

		public Page Page_GetByPageId(Guid pageId, bool includeNonPublished)
		{
			LoadPages();

			IEnumerable<Models.Page> pages = _pages.Where(p => p.PageId == pageId);

			if (includeNonPublished)
			{
				return pages.FirstOrDefault();
			}
			else
			{
				return pages.Where(p => p.Published && p.PublishDate <= DateTime.Now).FirstOrDefault();
			}
		}

		public void Page_Insert(Page page)
		{
			LoadPages();
			LoadMetas();

			_pages.Add(page);
			
			foreach (Models.Meta meta in page.Metas)
			{
				meta.MetaId = GetMaxMetaId();
				_metas.Add(meta);
			}

			SavePages();
			SaveMetas();
		}

		public Page[] Page_ListAll(bool includeNonPublished)
		{
			LoadPages();

			if (includeNonPublished)
			{
				return _pages.ToArray();
			}
			else
			{
				return _pages.Where(p => p.Published && p.PublishDate <= DateTime.Now).OrderBy(p => p.DisplayOrder).ToArray();
			}
		}

		public void Page_Update(Page page)
		{
			LoadPages();
			LoadMetas();

			Models.Page existingPage = _pages.Where(p => p.PageId == page.PageId).FirstOrDefault();

			if (existingPage == null)
			{
				throw new System.ApplicationException(string.Format("Original page not found for update.  PageId:  {0}", page.PageId.ToString()));
			}
			else
			{
				_pages.Remove(existingPage);
				_pages.Add(page);

				_metas.RemoveAll(m => m.PageId == page.PageId);
				foreach (Models.Meta meta in page.Metas)
				{
					if (meta.MetaId == 0)
					{
						meta.MetaId = GetMaxMetaId();
						_metas.Add(meta);
					}
				}

				SavePages();
				SaveMetas();
			}
		}

		private Page PageFromXElement(XElement xElement)
		{
			return new Page()
			{
				Content = xElement.Element("Content").Value,
				CreatedBy = xElement.Element("CreatedBy").Value,
				CreatedOn = DateTime.Parse(xElement.Element("CreatedOn").Value),
				DisplayOrder = int.Parse(xElement.Element("DisplayOrder").Value),
				ModifiedBy = xElement.Element("ModifiedBy").Value,
				ModifiedOn = DateTime.Parse(xElement.Element("ModifiedOn").Value),
				PageId = new Guid(xElement.Element("PageId").Value),
				ParentId = new Guid(xElement.Element("ParentId").Value),
				PublishDate = DateTime.Parse(xElement.Element("PublishDate").Value),
				Published = bool.Parse(xElement.Element("Published").Value),
				Slug = xElement.Element("Slug").Value,
				Title = xElement.Element("Title").Value
			};
		}

		#endregion

		#region Meta

		public Meta[] Meta_ListForSite()
		{
			LoadMetas();

			return _metas.Where(m => m.PageId == null).ToArray();
		}

		#endregion

		#region Setting

		public void Setting_Insert(Setting setting)
		{
			LoadSettings();

			setting.SettingId = GetMaxSettingId();
			_settings.Add(setting);

			SaveSettings();
		}

		public Setting[] Setting_ListAll()
		{
			LoadSettings();

			return _settings.ToArray();
		}

		public void Setting_Update(Setting setting)
		{
			LoadSettings();

			Models.Setting existingSetting = _settings.Where(s => s.SettingId == setting.SettingId).FirstOrDefault();

			if (existingSetting == null)
			{
				throw new System.ApplicationException(string.Format("Original setting not found for update.  SettingId:  {0}", setting.SettingId.ToString()));
			}
			else
			{
				_settings.Remove(existingSetting);
				_settings.Add(setting);

				SaveSettings();
			}
		}

		#endregion

		#region IDisposable Members

		public void Dispose()
		{
			/*
			_pages = null;
			_metas = null;
			_settings = null;
			 * */
		}

		#endregion

		#region Helpers

		private EntitySet<T> ToEntitySet<T>(IEnumerable<T> source) where T : class
		{
			EntitySet<T> entitySet = new EntitySet<T>();
			entitySet.AddRange(source);
			return entitySet;
		}

		private int GetMaxMetaId()
		{
			LoadMetas();

			Models.Meta maxMeta = _metas.OrderByDescending(m => m.MetaId).FirstOrDefault();
			return maxMeta == null ? 1 : maxMeta.MetaId + 1;
		}

		private int GetMaxSettingId()
		{
			LoadSettings();

			Models.Setting maxSetting = _settings.OrderByDescending(s => s.SettingId).FirstOrDefault();
			return maxSetting == null ? 1 : maxSetting.SettingId + 1;
		}

		#endregion
	}
}
