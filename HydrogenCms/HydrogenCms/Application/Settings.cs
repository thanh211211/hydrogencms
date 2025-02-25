using System;
using System.Collections.Generic;
using System.Linq;

namespace HydrogenCms.Application
{
	/// <summary>
	/// This class turns the list of settings from the business/data providers into a strongly typed list of known settings.
	/// </summary>
	public class Settings
	{
		/// <summary>
		/// Gets a setting from the data store.
		/// </summary>
		/// <param name="settingName"></param>
		/// <returns></returns>
		/// <remarks>This is public to enable the storage of arbitrary settings in the ui/plugins.</remarks>
		public static string GetSetting(string settingName)
		{
			Models.Setting setting = Models.Data.DataService.Setting_ListAll().Where(s => s.Name == settingName).FirstOrDefault();

			if (setting == null)
			{
				return null;
			}
			else
			{
				return setting.Value;
			}
		}

		/// <summary>
		/// Saves a setting to the data store.
		/// </summary>
		/// <param name="settingName"></param>
		/// <param name="settingValue"></param>
		/// <remarks>This is public to enable the storage of arbitrary settings in the ui/plugins.</remarks>
		public static void UpdateSetting(string settingName, string settingValue)
		{
		    Models.Setting setting = Models.Data.DataService.Setting_ListAll().Where(s => s.Name == settingName).FirstOrDefault();

		    if (setting == null)
		    {
		        Models.Data.DataService.Setting_Insert(new Models.Setting() { Name = settingName, Value = settingValue });
		    }
		    else
		    {
		        setting.Value = settingValue;

		        Models.Data.DataService.Setting_Update(setting);
		    }
		}

		public static string HashedPassword
		{
			get
			{
				return GetSetting(Constants.Settings.Password);
			}
			set
			{
			    UpdateSetting(Constants.Settings.Password, value);
			}
		}

		public static string MediaPath
		{
			get
			{
				return GetSetting(Constants.Settings.MediaPath);
			}
			set
			{
			    UpdateSetting(Constants.Settings.MediaPath, value);
			}
		}

		public static string SiteName
		{
			get
			{
				return GetSetting(Constants.Settings.SiteName);
			}
			set
			{
				UpdateSetting(Constants.Settings.SiteName, value);
			}
		}

		public static string Skin
		{
			get
			{
				return GetSetting(Constants.Settings.Skin);
			}
			set
			{
			    UpdateSetting(Constants.Settings.Skin, value);
			}
		}

		public static string SiteTagline
		{
			get
			{
				return GetSetting(Constants.Settings.SiteTagline);
			}
			set
			{
			    UpdateSetting(Constants.Settings.SiteTagline, value);
			}
		}

		public static string UserId
		{
			get
			{
				return GetSetting(Constants.Settings.UserId);
			}
			set
			{
			    UpdateSetting(Constants.Settings.UserId, value);
			}
		}
	}
}
