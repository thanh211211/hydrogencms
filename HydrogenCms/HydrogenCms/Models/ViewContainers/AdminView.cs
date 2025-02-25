using System;
using System.Collections.Generic;

namespace HydrogenCms.Models.ViewContainers
{
	public class AdminView : ViewContainer
	{
		public string MediaPath { get; set; }
		public string Password { get; set; } // Used for passing data back and forth in the admin controller, not for general use.
		public string SiteName { get; set; }
		public string SiteTagLine { get; set; }
		public string Skin { get; set; }
		public List<string> Skins { get; set; }
		public string UserId { get; set; }
	}
}
