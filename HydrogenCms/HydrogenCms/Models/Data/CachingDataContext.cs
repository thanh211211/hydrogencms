using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Web;

namespace HydrogenCms.Models.Data
{
	public class CachingDataContext : Data.IDataContext
	{
		private static readonly string _dataContextType;
		private readonly IDataContext _dataContext;
		private static readonly bool _enabled;

		private struct Keys
		{
			public const string SECTION_CACHING_DATA_CONTEXT = "HydrogenCms.Models.Data.CachingDataContext";
			public const string SETTING_DATA_CONTEXT_TYPE = "dataContextType";
			public const string SETTING_ENABLED = "enabled";

			#region Page

			public const string PAGE_GET_BY_PAGEID = "HydrogenCMS_Page_GetByPageId_";
			public const string PAGE_LIST_ALL = "HydrogenCMS_Page_ListAll_";
			public const string PAGE_DEPENDENCY = "HydrogenCMS_Page";

			#endregion

			#region Meta

			public const string META_FOR_SITE = "HydrogenCMS_Meta_ListForSite";

			#endregion

			#region Setting

			public const string SETTING_LIST_ALL = "HydrogenCMS_Setting_ListAll";
			public const string SETTING_DEPENDENCY = "HydrogenCMS_Setting";

			#endregion

		}

		static CachingDataContext()
		{
			Hashtable settings = ConfigurationManager.GetSection(Keys.SECTION_CACHING_DATA_CONTEXT) as Hashtable;

			if (settings != null)
			{
				_dataContextType = settings[Keys.SETTING_DATA_CONTEXT_TYPE].ToString();
				_enabled = bool.Parse(settings[Keys.SETTING_ENABLED].ToString());
			}
		}

		public CachingDataContext()
		{
			 this._dataContext = Utilities.ReflectionHelper.CreateObject<IDataContext>(_dataContextType, null);
		}

		private static bool Enabled
		{
			get
			{
				return (_enabled && HttpContext.Current != null);
			}
		}

		#region IDataContext Members

		#region Page

		public void Page_Delete(Guid pageId)
		{
			this._dataContext.Page_Delete(pageId);

			if (Enabled)
			{
				//
				// Invalidate the cache.
				//
				Utilities.CacheHelper.RemoveFromCache(Keys.PAGE_DEPENDENCY);
			}
		}

		public Page Page_GetByPageId(Guid pageId, bool includeNonPublished)
		{
			return Utilities.CacheHelper.GetFromCache<Models.Page>(Keys.PAGE_GET_BY_PAGEID + includeNonPublished + pageId.ToString(), new string[] { Keys.PAGE_DEPENDENCY }, delegate() { return this._dataContext.Page_GetByPageId(pageId, includeNonPublished); });
		}

		public void Page_Insert(Models.Page page)
		{
			this._dataContext.Page_Insert(page);

			if (Enabled)
			{
				//
				// Invalidate the cache.
				//
				Utilities.CacheHelper.RemoveFromCache(Keys.PAGE_DEPENDENCY);
			}
		}

		public Page[] Page_ListAll(bool includeNonPublished)
		{
			return Utilities.CacheHelper.GetFromCache<Models.Page[]>(Keys.PAGE_LIST_ALL + includeNonPublished.ToString(), new string[] { Keys.PAGE_DEPENDENCY }, delegate() { return this._dataContext.Page_ListAll(includeNonPublished); });
		}

		public void Page_Update(Models.Page page)
		{
			this._dataContext.Page_Update(page);

			if (Enabled)
			{
				//
				// Invalidate the cache.
				//
				Utilities.CacheHelper.RemoveFromCache(Keys.PAGE_DEPENDENCY);
			}
		}

		#endregion

		#region Meta

		public Meta[] Meta_ListForSite()
		{
			return Utilities.CacheHelper.GetFromCache<Models.Meta[]>(Keys.META_FOR_SITE, new string[] { Keys.PAGE_DEPENDENCY }, delegate() { return this._dataContext.Meta_ListForSite(); });
		}

		#endregion

		#region Setting

		public void Setting_Insert(Models.Setting setting)
		{
			this._dataContext.Setting_Insert(setting);

			if (Enabled)
			{
				//
				// Invalidate the cache.
				//
				Utilities.CacheHelper.RemoveFromCache(Keys.SETTING_DEPENDENCY);
			}
		}

		public Models.Setting[] Setting_ListAll()
		{
			return Utilities.CacheHelper.GetFromCache<Models.Setting[]>(Keys.SETTING_LIST_ALL, new string[] { Keys.SETTING_DEPENDENCY }, delegate() { return this._dataContext.Setting_ListAll(); });
		}

		public void Setting_Update(Models.Setting setting)
		{
			this._dataContext.Setting_Update(setting);

			if (Enabled)
			{
				//
				// Invalidate the cache.
				//
				Utilities.CacheHelper.RemoveFromCache(Keys.SETTING_DEPENDENCY);
			}
		}

		#endregion

		#endregion

		#region IDisposable Members

		public void Dispose()
		{
			if (this._dataContext != null)
			{
				this._dataContext.Dispose();
			}
		}

		#endregion
	}
}
