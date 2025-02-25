using System;
using System.Collections.Generic;

namespace HydrogenCms.Models.ViewContainers
{
	public class ViewContainer
	{
		public virtual Models.Page[] AllPages { get; set; }
		public string ControllerName { get; set; }
		public Models.Page Page { get; set; }
	}
}
