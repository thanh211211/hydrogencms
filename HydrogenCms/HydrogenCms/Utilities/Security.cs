using System;
using System.Web.Security;

namespace HydrogenCms.Utilities
{
	public static class Security
	{
		public static string HashPassword(string password)
		{
			string salt = "HydrogenCMS"; // jes - salt?
			return FormsAuthentication.HashPasswordForStoringInConfigFile(salt + password, "SHA1");
		}

		public static bool ValidateUser(string userId, string password)
		{
			if (userId == Application.Settings.UserId)
			{
				if (HashPassword(password) == Application.Settings.HashedPassword)
				{
					return true;
				}
				else
				{
					return false;
				}
			}
			else
			{
				return false;
			}
		}
	}
}
