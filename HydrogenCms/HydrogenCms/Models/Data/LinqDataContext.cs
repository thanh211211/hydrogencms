using System;
using System.Data.Linq;
using System.Linq;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Data.Linq.Mapping;
using System.Web;
using System.Web.Hosting;
using System.Collections;

namespace HydrogenCms.Models.Data
{
	public class LinqDataContext : DataContext, IDataContext
	{
		#region MappingSource

		private static readonly MappingSource _mappingSource;
		private static readonly string _owner;
		private static readonly string _prefix;

		/// <summary>
		/// The mapping source for this data context.  The reason this is a public property is to enable you to use the other constructor for the DataContext.
		/// </summary>
		public static MappingSource MappingSource
		{
			get
			{
				return _mappingSource;
			}
		}

		#endregion

		private struct Keys
		{
			public const string SECTION_MAPPING_SOURCE = "HydrogenCms.Models.Data.LinqDataContext";
			public const string SETTING_MAPPING_FILE = "mappingFile";
			public const string SETTING_OWNER = "owner";
			public const string SETTING_PREFIX = "prefix";
		}

		static LinqDataContext()
		{
			Hashtable settings = ConfigurationManager.GetSection(Keys.SECTION_MAPPING_SOURCE) as Hashtable;

			if (settings != null)
			{
				if (settings[Keys.SETTING_OWNER] != null)
				{
					_owner = settings[Keys.SETTING_OWNER].ToString();
					_prefix = settings[Keys.SETTING_PREFIX].ToString();
					_mappingSource = XmlMappingSource.FromXml(string.Format(new StreamReader(HostingEnvironment.MapPath(settings[Keys.SETTING_MAPPING_FILE].ToString())).ReadToEnd(), _owner, _prefix));
				}
				else
				{
					_mappingSource = XmlMappingSource.FromStream(new FileStream(HostingEnvironment.MapPath(settings[Keys.SETTING_MAPPING_FILE].ToString()), FileMode.Open));
				}
			}
		}

		public LinqDataContext() : base(ConfigurationManager.ConnectionStrings[Application.Constants.Connection.ConnectionString].ConnectionString, _mappingSource)
		{
		}

		private void SetDefaultDataLoadOptions()
		{
			if (this.LoadOptions == null)
			{
				DataLoadOptions dataLoadOptions = new DataLoadOptions();
				dataLoadOptions.LoadWith<Models.Page>(p => p.Metas);
				this.LoadOptions = dataLoadOptions;

				// this.ObjectTrackingEnabled = false;
				this.DeferredLoadingEnabled = false;
			}
		}

		private IQueryable<Models.Page> GetDefaultPageQuery(bool includeNonPublished)
		{
			if (includeNonPublished)
			{
				return this.Pages;
			}
			else
			{
				return this.Pages.Where(p => p.Published == true && p.PublishDate <= System.DateTime.Now);
			}
		}

		#region Page

		public void Page_Delete(Guid pageId)
		{
			Models.Page page = this.Pages.Where(p => p.PageId == pageId).FirstOrDefault();

			if (page != null)
			{
				this.Pages.DeleteOnSubmit(page);
				this.SubmitChanges();
			}
		}

		public Page Page_GetByPageId(Guid pageId, bool includeNonPublished)
		{
			return this.GetDefaultPageQuery(includeNonPublished).Where(p => p.PageId == pageId).FirstOrDefault();
		}

		public void Page_Insert(Models.Page page)
		{
			this.Pages.InsertOnSubmit(page);
			if (page.Metas != null && page.Metas.Count > 0)
			{
				this.Metas.InsertAllOnSubmit(page.Metas);
			}
			this.SubmitChanges();
		}

		public Page[] Page_ListAll(bool includeNonPublished)
		{
			this.SetDefaultDataLoadOptions();

			return this.GetDefaultPageQuery(includeNonPublished).OrderBy(p => p.DisplayOrder).ToArray();
		}

		public void Page_Update(Models.Page page)
		{
			this.SetDefaultDataLoadOptions();

			Models.Page originalPage = this.Page_GetByPageId(page.PageId, true);

			if (originalPage == null)
			{
				throw new System.ApplicationException(string.Format("Original page not found for update.  PageId:  {0}", page.PageId.ToString()));
			}
			else
			{
				//
				// Update page.
				//
				originalPage.Content = page.Content;
				originalPage.DisplayOrder = page.DisplayOrder;
				originalPage.ModifiedBy = Application.Settings.UserId;
				originalPage.ModifiedOn = DateTime.Now;
				originalPage.ParentId = page.ParentId;
				originalPage.PublishDate = page.PublishDate;
				originalPage.Published = page.Published;
				originalPage.Slug = page.Slug;
				originalPage.Title = page.Title;
			}

			//
			// Update existing meta tags that were pased in, and delete old non-passed in ones.
			//
			for (int counter = originalPage.Metas.Count - 1; counter >= 0; counter--)
			{
				Models.Meta originalMeta = originalPage.Metas[counter];
				Models.Meta newMeta = page.Metas.Where(m => m.Name == originalMeta.Name).FirstOrDefault();

				if (newMeta == null)
				{
					originalPage.Metas.RemoveAt(counter);
				}
				else
				{
					originalPage.Metas[counter].Content = newMeta.Content;
				}
			}

			//
			// Add new meta tags that did not exist before.
			//
			foreach (Models.Meta meta in page.Metas)
			{
				Models.Meta originalMeta = originalPage.Metas.Where(m => m.Name == meta.Name).FirstOrDefault();

				if (originalMeta == null)
				{
					originalPage.Metas.Add(new Models.Meta() { Content = meta.Content, Name = meta.Name, PageId = page.PageId });
				}
			}

			this.SubmitChanges();
		}

		#endregion

		#region Meta

		public Meta[] Meta_ListForSite()
		{
			this.SetDefaultDataLoadOptions();

			return this.Metas.Where(m => m.PageId == null).ToArray();
		}

		#endregion

		#region Setting

		public void Setting_Insert(Models.Setting setting)
		{
			this.Settings.InsertOnSubmit(setting);
			this.SubmitChanges();
		}

		public Models.Setting[] Setting_ListAll()
		{
			this.SetDefaultDataLoadOptions();

			return this.Settings.ToArray();
		}

		public void Setting_Update(Models.Setting setting)
		{
			Models.Setting originalSetting = this.Setting_ListAll().Where(s => s.SettingId == setting.SettingId).FirstOrDefault();

			if (originalSetting == null)
			{
				throw new System.ApplicationException(string.Format("Original setting not found for update.  SettingId:  {0}", setting.SettingId.ToString()));
			}
			else
			{
				originalSetting.Name = setting.Name;
				originalSetting.Value = setting.Value;

				this.SubmitChanges();
			}

			this.SubmitChanges();
		}

		#endregion
	}
}
