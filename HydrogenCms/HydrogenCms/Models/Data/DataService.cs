using System;
using System.Configuration;
using System.Linq;
using System.Collections;

namespace HydrogenCms.Models.Data
{
	public static class DataService
	{
		private static readonly string _dataContextType;

		private struct Keys
		{
			public const string SECTION_DATA_PROVIDER = "HydrogenCms.Models.Data.DataService";
			public const string SETTING_DATA_CONTEXT_TYPE = "dataContextType";
		}

		static DataService()
		{
			Hashtable settings = ConfigurationManager.GetSection(Keys.SECTION_DATA_PROVIDER) as Hashtable;

			if (settings != null)
			{
				_dataContextType = settings[Keys.SETTING_DATA_CONTEXT_TYPE].ToString();
			}
		}

		private static IDataContext GetDataContext()
		{
			 return Utilities.ReflectionHelper.CreateObject<IDataContext>(_dataContextType, null);
		}

		#region Page

		public static void Page_Delete(Guid pageId)
		{
			using (IDataContext dataContext = GetDataContext())
			{
				dataContext.Page_Delete(pageId);
			}
		}

		public static Models.Page Page_GetFrontPage()
		{
			Models.Page[] pages = GetDataContext().Page_ListAll(false);
			Models.Page page = pages.Where(p => p.DisplayOrder == 0).FirstOrDefault();

			if (page != null)
			{
				return page;
			}
			else
			{
				//
				// We didnt find an explicit front page, so find the first page.  If there are no pages, then just return null.
				//
				return pages.OrderBy(p => p.DisplayOrder).FirstOrDefault();
			}
		}

		public static Models.Page Page_GetByPageId(Guid pageId, bool includeNonPublished)
		{
			using (IDataContext dataContext = GetDataContext())
			{
				return dataContext.Page_GetByPageId(pageId, includeNonPublished);
			}
		}

		public static Models.Page Page_GetBySlug(string slug)
		{
			return GetDataContext().Page_ListAll(false).Where(p => p.Slug == slug).FirstOrDefault();
		}

		public static void Page_Insert(Models.Page page)
		{
			if (!page.IsValid)
			{
				throw new System.ApplicationException("Page invalid.  " + page.ValidationMessage);
			}

			using (IDataContext dataContext = GetDataContext())
			{
				dataContext.Page_Insert(page);
			}
		}

		public static Models.Page[] Page_ListAll(bool includeNonPublished)
		{
			using (IDataContext dataContext = GetDataContext())
			{
				return dataContext.Page_ListAll(includeNonPublished);
			}
		}

		public static void Page_Update(Models.Page page)
		{
			if (!page.IsValid)
			{
				throw new System.ApplicationException("Page invalid.  " + page.ValidationMessage);
			}

			using (IDataContext dataContext = GetDataContext())
			{
				dataContext.Page_Update(page);
			}
		}

		#endregion

		#region Meta

		public static Models.Meta[] Meta_ListForSite()
		{
			using (IDataContext dataContext = GetDataContext())
			{
				return dataContext.Meta_ListForSite();
			}
		}

		#endregion

		#region Setting

		public static void Setting_Insert(Models.Setting setting)
		{
			using (IDataContext dataContext = GetDataContext())
			{
				dataContext.Setting_Insert(setting);
			}
		}

		public static Models.Setting[] Setting_ListAll()
		{
			using (IDataContext dataContext = GetDataContext())
			{
				return dataContext.Setting_ListAll();
			}
		}

		public static void Setting_Update(Models.Setting setting)
		{
			using (IDataContext dataContext = GetDataContext())
			{
				dataContext.Setting_Update(setting);
			}
		}

		#endregion
	}
}
