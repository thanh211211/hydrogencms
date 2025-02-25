// http://codex.wordpress.org/XML-RPC_wp#wp.newPage
// http://msdn2.microsoft.com/en-us/library/bb463260.aspx

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Xml;

using HydrogenCms.Models;
using System.Linq;
using System.Globalization;

namespace HydrogenCms.API.MetaWeblog
{
	/// <summary>
	/// HTTP Handler for MetaWeblog API
	/// </summary>
	/// <remarks>
	/// Taken from BlogEngine.net 1.3.
	/// </remarks>
	internal class MetaWeblogHandler : IHttpHandler
	{
		#region IHttpHandler Members

		/// <summary>
		/// Gets a value indicating whether another request can use the <see cref="T:System.Web.IHttpHandler"></see> instance.
		/// </summary>
		/// <value></value>
		/// <returns>true if the <see cref="T:System.Web.IHttpHandler"></see> instance is reusable; otherwise, false.</returns>
		public bool IsReusable
		{
			get { return false; }
		}

		/// <summary>
		/// Process the HTTP Request.  Create XMLRPC request, find method call, process it and create response object and sent it back.
		/// This is the heart of the MetaWeblog API
		/// </summary>
		/// <param name="context"></param>
		public void ProcessRequest(HttpContext context)
		{
			try
			{
				string rootUrl = context.Request.Url.ToString().Substring(0, context.Request.Url.ToString().IndexOf("metaweblog.axd"));
				XMLRPCRequest input = new XMLRPCRequest(context);
				XMLRPCResponse output = new XMLRPCResponse(input.MethodName);

				switch (input.MethodName)
				{
					case "metaWeblog.newPost":
						throw new MetaWeblogException("10", "The method newPost is not implemented.");
					case "metaWeblog.editPost":
						throw new MetaWeblogException("10", "The method editPost is not implemented.");
					case "metaWeblog.getPost":
						throw new MetaWeblogException("10", "The method getPost is not implemented.");
					case "metaWeblog.getCategories":
						throw new MetaWeblogException("10", "The method getCategories is not implemented.");
					case "metaWeblog.getRecentPosts":
						throw new MetaWeblogException("10", "The method getRecentPosts is not implemented.");
					case "blogger.deletePost":
						throw new MetaWeblogException("10", "The method deletePost is not implemented.");
					case "blogger.getUserInfo":
						//Not implemented.  Not planned.
						throw new MetaWeblogException("10", "The method GetUserInfo is not implemented.");

					case "metaWeblog.newMediaObject":
						output.MediaInfo = NewMediaObject(input.BlogID, input.UserName, input.Password, input.MediaObject, context);
						break;

					case "blogger.getUsersBlogs":
					case "metaWeblog.getUsersBlogs":
						output.Blogs = GetUserBlogs(input.AppKey, input.UserName, input.Password, rootUrl);
						break;
					case "wp.newPage":
						output.PageID = NewPage(input.BlogID, input.UserName, input.Password, input.Page, input.Publish);
						break;
					case "wp.getPageList":
					case "wp.getPages":
						output.Pages = GetPages(input.BlogID, input.UserName, input.Password);
						break;
					case "wp.getPage":
						output.Page = GetPage(input.BlogID, input.PageID, input.UserName, input.Password);
						break;
					case "wp.editPage":
						output.Completed = EditPage(input.BlogID, input.PageID, input.UserName, input.Password, input.Page, input.Publish);
						break;
					case "wp.deletePage":
						output.Completed = DeletePage(input.BlogID, input.PageID, input.UserName, input.Password);
						break;
				}

				output.Response(context);
			}
			catch (MetaWeblogException mwException)
			{
				XMLRPCResponse output = new XMLRPCResponse("fault");
				MWAFault fault = new MWAFault();
				fault.faultCode = mwException.Code;
				fault.faultString = mwException.Message;
				output.Fault = fault;
				output.Response(context);
			}
			catch (Exception exception)
			{
				XMLRPCResponse output = new XMLRPCResponse("fault");
				MWAFault fault = new MWAFault();
				fault.faultCode = "0";
				fault.faultString = exception.Message;
				output.Fault = fault;
				output.Response(context);
			}
		}

		#endregion

		#region API Methods

		/// <summary>
		/// metaWeblog.newMediaObject
		/// </summary>
		/// <param name="blogId">always 1000 in HydrogenCMS since it is a single cms instance</param>
		/// <param name="username">login username</param>
		/// <param name="password">login password</param>
		/// <param name="mediaObject">struct with media details</param>
		/// <returns>struct with url to media</returns>
		internal MWAMediaInfo NewMediaObject(string blogId, string username, string password, MWAMediaObject mediaObject, HttpContext request)
		{
			ValidateRequest(username, password);

			MWAMediaInfo mediaInfo = new MWAMediaInfo();

			string saveFolder = request.Server.MapPath(Application.Settings.MediaPath);
			string fileName = mediaObject.name;

			// Check/Create Folders & Fix fileName
			if (mediaObject.name.LastIndexOf('/') > -1)
			{
				saveFolder += mediaObject.name.Substring(0, mediaObject.name.LastIndexOf('/'));
				saveFolder = saveFolder.Replace('/', Path.DirectorySeparatorChar);
				fileName = mediaObject.name.Substring(mediaObject.name.LastIndexOf('/') + 1);
			}
			else
			{
				if (saveFolder.EndsWith(Path.DirectorySeparatorChar.ToString()))
				{
					saveFolder = saveFolder.Substring(0, saveFolder.Length - 1);
				}
			}

			if (!Directory.Exists(saveFolder))
			{
				Directory.CreateDirectory(saveFolder);
			}

			saveFolder += Path.DirectorySeparatorChar;

			// Save File
			using (FileStream fileStream = new FileStream(saveFolder + fileName, FileMode.Create))
			{
				BinaryWriter binaryWriter = new BinaryWriter(fileStream);
				binaryWriter.Write(mediaObject.bits);
				binaryWriter.Close();
			}

			// Set Url
			string rootUrl = request.Request.Url.ToString().Substring(0, request.Request.Url.ToString().IndexOf("metaweblog.axd"));

			string mediaType = mediaObject.type;
			if (mediaType.IndexOf('/') > -1)
			{
				mediaType = mediaType.Substring(0, mediaType.IndexOf('/'));
			}

			switch (mediaType)
			{
				case "image":
				case "notsent": // If there wasn't a type, let's pretend it is an image.  (Thanks Zoundry.  This is for you.)
					rootUrl += "image.axd?picture=";
					break;
				default:
					rootUrl += "file.axd?file=";
					break;
			}

			mediaInfo.url = rootUrl + mediaObject.name;
			
			return mediaInfo;
		}

		#region Conversion Helpers

		private static Models.Page MWAPageToPage(bool isNew, MWAPage mwaPage, string username, bool publish)
		{
			Models.Page page = new Models.Page();

			page.Content = mwaPage.description;

			if (isNew)
			{
				page.CreatedBy = username;
				page.CreatedOn = System.DateTime.Now;
			}
			else
			{
				page.ModifiedBy = username;
				page.ModifiedOn = System.DateTime.Now;
			}

			page.DisplayOrder = mwaPage.pageOrder;

			page.PageId = mwaPage.pageId == null ? Guid.NewGuid() : new Guid(mwaPage.pageId);

			page.Metas = new System.Data.Linq.EntitySet<Meta>();
			page.Metas.Add(new Meta() { Content = mwaPage.keywords, Name = Application.Constants.Meta.Keywords, Page = page, PageId = page.PageId });

			if (mwaPage.pageParentID != "0")
			{
				page.ParentId = new Guid(mwaPage.pageParentID);
			}

			if (mwaPage.pageDate != new DateTime())
			{
				page.PublishDate = mwaPage.pageDate; // jes - //mwaPage.pageDate.AddHours(BlogSettings.Instance.Timezone * -1);
			}
			else
			{
				page.PublishDate = System.DateTime.Now;
			}

			page.Published = publish;

			if (string.IsNullOrEmpty(mwaPage.slug))
			{
				page.Slug = ConvertTitleToSlug(mwaPage.title);
			}
			else
			{
				page.Slug = ConvertTitleToSlug(mwaPage.slug);
			}
			page.Title = mwaPage.title;

			return page;
		}

		private static MWAPage PageToMWAPage(Models.Page page)
		{
			MWAPage mwaPage = new MWAPage();

			Models.Meta meta = page.Metas.Where(m => m.Name == Application.Constants.Meta.Keywords).FirstOrDefault();
			if (meta != null && !string.IsNullOrEmpty(meta.Content))
			{
				mwaPage.keywords = meta.Content;
			}

			mwaPage.description = page.Content;
			mwaPage.link = "link"; // jes - link;
			mwaPage.mt_convert_breaks = "__default__";
			mwaPage.pageDate = page.PublishDate;
			mwaPage.pageId = page.PageId.ToString();
			mwaPage.pageParentID = page.ParentId.ToString();
			mwaPage.slug = page.Slug;
			mwaPage.title = page.Title;

			return mwaPage;
		}

		#endregion

		/// <summary>
		/// wp.newPage
		/// </summary>
		/// <param name="blogId">blogId in string format</param>
		/// <param name="username">login username</param>
		/// <param name="password">login password</param>
		/// <param name="mwaPage"></param>
		/// <param name="publish"></param>
		/// <returns></returns>
		internal string NewPage(string blogId, string username, string password, MWAPage mwaPage, bool publish)
		{
			ValidateRequest(username, password);

			Models.Page page = MWAPageToPage(true, mwaPage, username, publish);

			if (!page.IsValid)
			{
				throw new System.ApplicationException(string.Format("Unable to create page.  Page was invalid.  Validation Error:  {0}", page.ValidationMessage));
			}
			else
			{
				Models.Data.DataService.Page_Insert(page);
				return page.PageId.ToString();
			}
		}

		/// <summary>
		/// wp.getPages
		/// </summary>
		/// <param name="blogId">blogId in string format</param>
		/// <param name="username">login username</param>
		/// <param name="password">login password</param>
		/// <returns></returns>
		internal List<MWAPage> GetPages(string blogId, string username, string password)
		{
			ValidateRequest(username, password);

			List<MWAPage> mwaPages = new List<MWAPage>();
			Models.Page[] pages = Models.Data.DataService.Page_ListAll(true);

			foreach (Models.Page page in pages)
			{
				mwaPages.Add(PageToMWAPage(page));
			}

			return mwaPages;
		}

		/// <summary>
		/// wp.getPage
		/// </summary>
		/// <param name="blogId">blogId in string format</param>
		/// <param name="pageId">page guid in string format</param>
		/// <param name="username">login username</param>
		/// <param name="password">login password</param>
		/// <returns>struct with page details</returns>
		internal MWAPage GetPage(string blogId, string pageId, string username, string password)
		{
			ValidateRequest(username, password);

			Models.Page page = Models.Data.DataService.Page_GetByPageId(new Guid(pageId), true);

			if (page != null)
			{
				return PageToMWAPage(page);
			}
			else
			{
				throw new System.ApplicationException(string.Format("Page not found.  PageId:  {0}", pageId));
			}
		}

		internal bool EditPage(string blogId, string pageId, string username, string password, MWAPage mwaPage, bool publish)
		{
			ValidateRequest(username, password);

			mwaPage.pageId = pageId.ToString();
			Models.Page page = MWAPageToPage(false, mwaPage, username, publish);

			Models.Data.DataService.Page_Update(page);
	
			return true;
		}

		internal bool DeletePage(string blogId, string pageId, string username, string password)
		{
			ValidateRequest(username, password);

			try
			{
				Models.Data.DataService.Page_Delete(new Guid(pageId));
			}
			catch (Exception ex)
			{
				throw new MetaWeblogException("15", "DeletePage failed.  Error: " + ex.Message);
			}

			return true;
		}

		/// <summary>
		/// blogger.getUsersBlogs
		/// </summary>
		/// <param name="appKey">Key from application.  Outdated methodology that has no use here.</param>
		/// <param name="username">login username</param>
		/// <param name="password">login password</param>
		/// <returns>array of blog structs</returns>
		internal List<MWABlogInfo> GetUserBlogs(string appKey, string username, string password, string rootUrl)
		{
			List<MWABlogInfo> blogs = new List<MWABlogInfo>();

			ValidateRequest(username, password);

			MWABlogInfo blogInfo = new MWABlogInfo();
			blogInfo.url = rootUrl;
			blogInfo.blogId = "1000";
			blogInfo.blogName = Application.Settings.SiteName;

			blogs.Add(blogInfo);

			return blogs;
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Checks username and password.  Throws error if validation fails.
		/// </summary>
		/// <param name="username"></param>
		/// <param name="password"></param>
		private void ValidateRequest(string username, string password)
		{
			if (!Utilities.Security.ValidateUser(username, password))
			{
				throw new MetaWeblogException("11", "User authentication failed");
			}
		}

		/// <summary>
		/// Strips all illegal characters from the specified title.
		/// </summary>
		private static string ConvertTitleToSlug(string title)
		{
			if (string.IsNullOrEmpty(title))
				return title;

			title = title.Replace(":", string.Empty);
			title = title.Replace("/", string.Empty);
			title = title.Replace("?", string.Empty);
			title = title.Replace("#", string.Empty);
			title = title.Replace("[", string.Empty);
			title = title.Replace("]", string.Empty);
			title = title.Replace("@", string.Empty);
			title = title.Replace(".", string.Empty);
			title = title.Replace("\"", string.Empty);
			title = title.Replace("&", string.Empty);
			title = title.Replace("'", string.Empty);
			title = title.Replace(" ", "-");
			title = RemoveDiacritics(title);

			return HttpUtility.UrlEncode(title).Replace("%", string.Empty);
		}

		private static string RemoveDiacritics(string text)
		{
			string normalized = text.Normalize(NormalizationForm.FormD);
			StringBuilder stringBuilder = new StringBuilder();

			for (int counter = 0; counter < normalized.Length; counter++)
			{
				char character = normalized[counter];
				if (CharUnicodeInfo.GetUnicodeCategory(character) != UnicodeCategory.NonSpacingMark)
				{
					stringBuilder.Append(character);
				}
			}

			return stringBuilder.ToString();
		}

		#endregion
	}

	/// <summary>
	/// Exception specifically for MetaWeblog API.  Error (or fault) responses 
	/// request a code value.  This is our chance to add one to the exceptions
	/// which can be used to produce a proper fault.
	/// </summary>
	[Serializable()]
	public class MetaWeblogException : Exception
	{
		/// <summary>
		/// Constructor to load properties
		/// </summary>
		/// <param name="code">Fault code to be returned in Fault Response</param>
		/// <param name="message">Message to be returned in Fault Response</param>
		public MetaWeblogException(string code, string message) : base(message)
		{
			_code = code;
		}

		private string _code;
		/// <summary>
		/// Code is actually for Fault Code.  It will be passed back in the 
		/// response along with the error message.
		/// </summary>
		public string Code
		{
			get { return _code; }
		}
	}

	/// <summary>
	/// MetaWeblog BlogInfo struct
	/// returned as an array from getUserBlogs
	/// </summary>
	internal struct MWABlogInfo
	{
		/// <summary>
		/// Blog Url
		/// </summary>
		public string url;
		/// <summary>
		/// Blog ID (Since HydrogenCMS is single instance this number is always 1000.
		/// </summary>
		public string blogId;
		/// <summary>
		/// Blog Title
		/// </summary>
		public string blogName;
	}

	/// <summary>
	/// MetaWeblog Fault struct
	/// returned when error occurs
	/// </summary>
	internal struct MWAFault
	{
		/// <summary>
		/// Error code of Fault Response
		/// </summary>
		public string faultCode;
		/// <summary>
		/// Message of Fault Response
		/// </summary>
		public string faultString;
	}

	/// <summary>
	/// MetaWeblog MediaObject struct
	/// passed in the newMediaObject call
	/// </summary>
	internal struct MWAMediaObject
	{
		/// <summary>
		/// Name of media object (filename)
		/// </summary>
		public string name;
		/// <summary>
		/// Type of file
		/// </summary>
		public string type;
		/// <summary>
		/// Media
		/// </summary>
		public byte[] bits;
	}

	/// <summary>
	/// MetaWeblog MediaInfo struct
	/// returned from NewMediaObject call
	/// </summary>
	internal struct MWAMediaInfo
	{
		/// <summary>
		/// Url that points to Saved MediaObejct
		/// </summary>
		public string url;
	}

	/// <summary>
	/// MetaWeblog UserInfo struct
	/// returned from GetUserInfo call
	/// </summary>
	/// <remarks>
	/// Not used currently, but here for completeness.
	/// </remarks>
	internal struct MWAUserInfo
	{
		/// <summary>
		/// User Name Proper
		/// </summary>
		public string nickname;
		/// <summary>
		/// Login ID
		/// </summary>
		public string userID;
		/// <summary>
		/// Url to User Blog?
		/// </summary>
		public string url;
		/// <summary>
		/// Email address of User
		/// </summary>
		public string email;
		/// <summary>
		/// User LastName
		/// </summary>
		public string lastName;
		/// <summary>
		/// User First Name
		/// </summary>
		public string firstName;
	}

	/// <summary>
	/// wp Page Struct
	/// </summary>
	internal struct MWAPage
	{
		/// <summary>
		/// PageID Guid in string format
		/// </summary>
		public string pageId;

		/// <summary>
		/// Title of the page.
		/// </summary>
		public string title;

		/// <summary>
		/// Description of the page.
		/// </summary>
		public string description;

		/// <summary>
		/// Keywords for the page.
		/// </summary>
		public string keywords;

		/// <summary>
		/// Unique identifier for the page.
		/// </summary>
		public string slug;
		
		/// <summary>
		/// Unique identifier for the page.
		/// </summary>
		public string link;

		/// <summary>
		/// Display date of page (DateCreated)
		/// </summary>
		public DateTime pageDate;

		/// <summary>
		/// Convert Breaks
		/// </summary>
		public string mt_convert_breaks;

		/// <summary>
		/// Page Parent ID
		/// </summary>
		public string pageParentID;

		/// <summary>
		/// The display order of the page.
		/// </summary>
		public int pageOrder;
	}

}
