using System;
using System.Collections.Generic;

namespace HydrogenCms.Models.Data
{
	public interface IDataContext : IDisposable
	{
		#region Page

		void Page_Delete(Guid pageId);
		Models.Page Page_GetByPageId(Guid pageId, bool includeNonPublished);
		void Page_Insert(Models.Page page);
		Models.Page[] Page_ListAll(bool includeNonPublished);
		void Page_Update(Models.Page page);

		#endregion

		#region Meta

		Models.Meta[] Meta_ListForSite();

		#endregion

		#region Setting

		void Setting_Insert(Models.Setting setting);
		Models.Setting[] Setting_ListAll();
		void Setting_Update(Models.Setting setting);

		#endregion
	}
}
