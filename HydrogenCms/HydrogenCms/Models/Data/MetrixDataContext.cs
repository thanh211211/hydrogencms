using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using HydrogenCms.Utilities;

namespace HydrogenCms.Models.Data
{
	public class MetrixDataContext : Data.IDataContext
	{
		private static readonly string _dataContextType;
		private readonly IDataContext _dataContext;

		private struct Keys
		{
			public const string SECTION_METRIX_DATA_CONTEXT = "HydrogenCms.Models.Data.MetrixDataContext";
			public const string SETTING_DATA_CONTEXT_TYPE = "dataContextType";
		}

		static MetrixDataContext()
		{
			Hashtable settings = ConfigurationManager.GetSection(Keys.SECTION_METRIX_DATA_CONTEXT) as Hashtable;

			if (settings != null)
			{
				_dataContextType = settings[Keys.SETTING_DATA_CONTEXT_TYPE].ToString();
			}
		}

		public MetrixDataContext()
		{
			 this._dataContext = Utilities.ReflectionHelper.CreateObject<IDataContext>(_dataContextType, null);
		}

		#region IDataContext Members

		#region Page

		public void Page_Delete(Guid pageId)
		{
			using (new Utilities.CodeBenchmark())
			{
				this._dataContext.Page_Delete(pageId);
			}
		}

		public Page Page_GetByPageId(Guid pageId, bool includeNonPublished)
		{
			using (new Utilities.CodeBenchmark())
			{
				return this._dataContext.Page_GetByPageId(pageId, includeNonPublished);
			}
		}

		public void Page_Insert(Models.Page page)
		{
			using (new Utilities.CodeBenchmark())
			{
				this._dataContext.Page_Insert(page);
			}
		}

		public Page[] Page_ListAll(bool includeNonPublished)
		{
			using (new Utilities.CodeBenchmark())
			{
				return this._dataContext.Page_ListAll(includeNonPublished);
			}
		}

		public void Page_Update(Models.Page page)
		{
			using (new Utilities.CodeBenchmark())
			{
				this._dataContext.Page_Update(page);
			}
		}

		#endregion

		#region Meta

		public Meta[] Meta_ListForSite()
		{
			using (new Utilities.CodeBenchmark())
			{
				return this._dataContext.Meta_ListForSite();
			}
		}

		#endregion

		#region Setting

		public void Setting_Insert(Models.Setting setting)
		{
			using (new Utilities.CodeBenchmark())
			{
				this._dataContext.Setting_Insert(setting);
			}
		}

		public Models.Setting[] Setting_ListAll()
		{
			using (new Utilities.CodeBenchmark())
			{
				return this._dataContext.Setting_ListAll();
			}
		}

		public void Setting_Update(Models.Setting setting)
		{
			using (new Utilities.CodeBenchmark())
			{
				this._dataContext.Setting_Update(setting);
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
