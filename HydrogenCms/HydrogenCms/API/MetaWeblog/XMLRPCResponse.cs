using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using System.Xml;

namespace HydrogenCms.API.MetaWeblog
{
	/// <summary>
	/// Object is the outgoing XML-RPC response.  This objects properties are set
	/// and the Response method is called sending the response via the HttpContext Response.
	/// </summary>
	/// <remarks>
	/// Taken from BlogEngine.net 1.3.
	/// </remarks>
	internal class XMLRPCResponse
	{
		#region Contructors

		/// <summary>
		/// Constructor sets default value
		/// </summary>
		/// <param name="methodName">MethodName of called XML-RPC method</param>
		public XMLRPCResponse(string methodName)
		{
			_methodName = methodName;
			_blogs = new List<MWABlogInfo>();
			_pages = new List<MWAPage>();
		}
		#endregion

		#region Local Vars

		private string _methodName;

		private List<MWABlogInfo> _blogs;
		private bool _completed;
		private MWAFault _fault;
		private MWAMediaInfo _mediaInfo;
		private List<MWAPage> _pages;
		private MWAPage _page;
		private string _pageId;
		public object Categories
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		#endregion

		#region Public Properties

		/// <summary>
		/// List of blog structs.  Used by blogger.getUsersBlogs.
		/// </summary>
		public List<MWABlogInfo> Blogs
		{
			get { return _blogs; }
			set { _blogs = value; }
		}

		/// <summary>
		/// Marks whether function call was completed and successful.  
		/// Used by metaWeblog.editPost and blogger.deletePost.
		/// </summary>
		public bool Completed
		{
			get { return _completed; }
			set { _completed = value; }
		}

		/// <summary>
		/// Fault Struct. Used by API to return error information
		/// </summary>
		public MWAFault Fault
		{
			get { return _fault; }
			set { _fault = value; }
		}

		/// <summary>
		/// MediaInfo Struct
		/// </summary>
		public MWAMediaInfo MediaInfo
		{
			get { return _mediaInfo; }
			set { _mediaInfo = value; }
		}

		/// <summary>
		/// Id of page that was just added.
		/// </summary>
		public string PageID
		{
			get { return _pageId; }
			set { _pageId = value; }
		}

		/// <summary>
		/// List of Page Structs
		/// </summary>
		public List<MWAPage> Pages
		{
			get { return _pages; }
			set { _pages = value; }
		}

		/// <summary>
		/// MWAPage struct
		/// </summary>
		public MWAPage Page
		{
			get { return _page; }
			set { _page = value; }
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Response generates the XML-RPC response and returns it to the caller.
		/// </summary>
		/// <param name="context">httpContext.Response.OutputStream is used from the context</param>
		public void Response(HttpContext context)
		{
			context.Response.ContentType = "text/xml";
			using (XmlTextWriter data = new XmlTextWriter(context.Response.OutputStream, System.Text.Encoding.UTF8))
			{
				data.Formatting = Formatting.Indented;
				data.WriteStartDocument();
				data.WriteStartElement("methodResponse");
				if (_methodName == "fault")
					data.WriteStartElement("fault");
				else
					data.WriteStartElement("params");

				switch (_methodName)
				{
					case "metaWeblog.newPost":
						// WriteNewPost(data);
						break;
					case "metaWeblog.getPost":
						// WritePost(data);
						break;
					case "metaWeblog.newMediaObject":
						WriteMediaInfo(data);
						break;
					case "metaWeblog.getCategories":
						// WriteGetCategories(data);
						break;
					case "metaWeblog.getRecentPosts":
						// WritePosts(data);
						break;
					case "blogger.getUsersBlogs":
					case "metaWeblog.getUsersBlogs":
						WriteGetUsersBlogs(data);
						break;
					case "metaWeblog.editPost":
					case "blogger.deletePost":
					case "wp.editPage":
					case "wp.deletePage":
						WriteBool(data);
						break;
					case "wp.newPage":
						WriteNewPage(data);
						break;
					case "wp.getPage":
						WritePage(data);
						break;
					case "wp.getPageList":
						WriteShortPages(data);
						break;
					case "wp.getPages":
						WritePages(data);
						break;
					case "fault":
						WriteFault(data);
						break;

				}

				data.WriteEndElement();
				data.WriteEndElement();
				data.WriteEndDocument();

			}
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Writes Fault Parameters of Response.
		/// </summary>
		/// <param name="data">xml response</param>
		private void WriteFault(XmlTextWriter data)
		{
			data.WriteStartElement("value");
			data.WriteStartElement("struct");

			// faultCode
			data.WriteStartElement("member");
			data.WriteElementString("name", "faultCode");
			data.WriteElementString("value", _fault.faultCode);
			data.WriteEndElement();

			// faultString
			data.WriteStartElement("member");
			data.WriteElementString("name", "faultString");
			data.WriteElementString("value", _fault.faultString);
			data.WriteEndElement();

			data.WriteEndElement();
			data.WriteEndElement();

		}

		/// <summary>
		/// Writes Boolean parameter of Response
		/// </summary>
		/// <param name="data">xml response</param>
		private void WriteBool(XmlTextWriter data)
		{
			string postValue = "0";
			if (_completed)
				postValue = "1";
			data.WriteStartElement("param");
			data.WriteStartElement("value");
			data.WriteElementString("boolean", postValue);
			data.WriteEndElement();
			data.WriteEndElement();
		}

		/// <summary>
		/// Writes the MediaInfo Struct of Response
		/// </summary>
		/// <param name="data">xml response</param>
		private void WriteMediaInfo(XmlTextWriter data)
		{
			data.WriteStartElement("param");
			data.WriteStartElement("value");
			data.WriteStartElement("struct");

			// url
			data.WriteStartElement("member");
			data.WriteElementString("name", "url");
			data.WriteStartElement("value");
			data.WriteElementString("string", _mediaInfo.url);
			data.WriteEndElement();
			data.WriteEndElement();

			data.WriteEndElement();
			data.WriteEndElement();
			data.WriteEndElement();
		}

		/// <summary>
		/// Writes the PageID string of Response.
		/// </summary>
		/// <param name="data">xml response</param>
		private void WriteNewPage(XmlTextWriter data)
		{
			data.WriteStartElement("param");
			data.WriteStartElement("value");
			data.WriteElementString("string", _pageId);
			data.WriteEndElement();
			data.WriteEndElement();
		}

		/// <summary>
		/// Writes the Metaweblog Page Struct of Response.
		/// </summary>
		/// <param name="data">xml response</param>
		private void WritePage(XmlTextWriter data)
		{
			data.WriteStartElement("param");
			data.WriteStartElement("value");
			data.WriteStartElement("struct");

			// pageid
			data.WriteStartElement("member");
			data.WriteElementString("name", "page_id");
			data.WriteStartElement("value");
			data.WriteElementString("string", _page.pageId);
			data.WriteEndElement();
			data.WriteEndElement();

			// title
			data.WriteStartElement("member");
			data.WriteElementString("name", "title");
			data.WriteStartElement("value");
			data.WriteElementString("string", _page.title);
			data.WriteEndElement();
			data.WriteEndElement();

			// description
			data.WriteStartElement("member");
			data.WriteElementString("name", "description");
			data.WriteStartElement("value");
			data.WriteElementString("string", _page.description);
			data.WriteEndElement();
			data.WriteEndElement();

			// link
			data.WriteStartElement("member");
			data.WriteElementString("name", "link");
			data.WriteStartElement("value");
			data.WriteElementString("string", _page.link);
			data.WriteEndElement();
			data.WriteEndElement();

			// mt_convert_breaks
			data.WriteStartElement("member");
			data.WriteElementString("name", "mt_convert_breaks");
			data.WriteStartElement("value");
			data.WriteElementString("string", "__default__");
			data.WriteEndElement();
			data.WriteEndElement();

			// dateCreated
			data.WriteStartElement("member");
			data.WriteElementString("name", "dateCreated");
			data.WriteStartElement("value");
			data.WriteElementString("dateTime.iso8601", ConvertDatetoISO8601(Page.pageDate));
			data.WriteEndElement();
			data.WriteEndElement();

			data.WriteEndElement();
			data.WriteEndElement();
			data.WriteEndElement();
		}

		private void WritePages(XmlTextWriter data)
		{
			data.WriteStartElement("param");
			data.WriteStartElement("value");
			data.WriteStartElement("array");
			data.WriteStartElement("data");

			foreach (MWAPage page in _pages)
			{
				data.WriteStartElement("value");
				data.WriteStartElement("struct");

				// pageid
				data.WriteStartElement("member");
				data.WriteElementString("name", "page_id");
				data.WriteStartElement("value");
				data.WriteElementString("string", page.pageId);
				data.WriteEndElement();
				data.WriteEndElement();

				// title
				data.WriteStartElement("member");
				data.WriteElementString("name", "title");
				data.WriteStartElement("value");
				data.WriteElementString("string", page.title);
				data.WriteEndElement();
				data.WriteEndElement();

				// description
				data.WriteStartElement("member");
				data.WriteElementString("name", "description");
				data.WriteStartElement("value");
				data.WriteElementString("string", page.description);
				data.WriteEndElement();
				data.WriteEndElement();

				// link
				data.WriteStartElement("member");
				data.WriteElementString("name", "link");
				data.WriteStartElement("value");
				data.WriteElementString("string", page.link);
				data.WriteEndElement();
				data.WriteEndElement();

				// mt_convert_breaks
				data.WriteStartElement("member");
				data.WriteElementString("name", "mt_convert_breaks");
				data.WriteStartElement("value");
				data.WriteElementString("string", "__default__");
				data.WriteEndElement();
				data.WriteEndElement();

				// dateCreated
				data.WriteStartElement("member");
				data.WriteElementString("name", "dateCreated");
				data.WriteStartElement("value");
				data.WriteElementString("dateTime.iso8601", ConvertDatetoISO8601(page.pageDate));
				data.WriteEndElement();
				data.WriteEndElement();

				// page_parent_id
				if (page.pageParentID != null && page.pageParentID != "")
				{
					data.WriteStartElement("member");
					data.WriteElementString("name", "page_parent_id");
					data.WriteStartElement("value");
					data.WriteElementString("string", page.pageParentID);
					data.WriteEndElement();
					data.WriteEndElement();
				}

				data.WriteEndElement();
				data.WriteEndElement();

			}

			// Close tags
			data.WriteEndElement();
			data.WriteEndElement();
			data.WriteEndElement();
			data.WriteEndElement();
		}

		private void WriteShortPages(XmlTextWriter data)
		{
			data.WriteStartElement("param");
			data.WriteStartElement("value");
			data.WriteStartElement("array");
			data.WriteStartElement("data");

			foreach (MWAPage page in _pages)
			{
				data.WriteStartElement("value");
				data.WriteStartElement("struct");

				// pageid
				data.WriteStartElement("member");
				data.WriteElementString("name", "page_id");
				data.WriteStartElement("value");
				data.WriteElementString("string", page.pageId);
				data.WriteEndElement();
				data.WriteEndElement();

				// title
				data.WriteStartElement("member");
				data.WriteElementString("name", "page_title");
				data.WriteStartElement("value");
				data.WriteElementString("string", page.title);
				data.WriteEndElement();
				data.WriteEndElement();

				// page_parent_id
				data.WriteStartElement("member");
				data.WriteElementString("name", "page_parent_id");
				data.WriteStartElement("value");
				data.WriteElementString("string", page.pageParentID);
				data.WriteEndElement();
				data.WriteEndElement();

				// dateCreated
				data.WriteStartElement("member");
				data.WriteElementString("name", "dateCreated");
				data.WriteStartElement("value");
				data.WriteElementString("dateTime.iso8601", ConvertDatetoISO8601(page.pageDate));
				data.WriteEndElement();
				data.WriteEndElement();

				// dateCreated gmt
				data.WriteStartElement("member");
				data.WriteElementString("name", "date_created_gmt");
				data.WriteStartElement("value");
				data.WriteElementString("dateTime.iso8601", ConvertDatetoISO8601(page.pageDate));
				data.WriteEndElement();
				data.WriteEndElement();

				data.WriteEndElement();
				data.WriteEndElement();

			}

			// Close tags
			data.WriteEndElement();
			data.WriteEndElement();
			data.WriteEndElement();
			data.WriteEndElement();
		}

		/// <summary>
		/// Writes array of BlogInfo structs of Response
		/// </summary>
		/// <param name="data"></param>
		private void WriteGetUsersBlogs(XmlTextWriter data)
		{
			data.WriteStartElement("param");
			data.WriteStartElement("value");
			data.WriteStartElement("array");
			data.WriteStartElement("data");

			foreach (MWABlogInfo blog in _blogs)
			{
				data.WriteStartElement("value");
				data.WriteStartElement("struct");

				// url
				data.WriteStartElement("member");
				data.WriteElementString("name", "url");
				data.WriteElementString("value", blog.url);
				data.WriteEndElement();

				// blogid
				data.WriteStartElement("member");
				data.WriteElementString("name", "blogid");
				data.WriteElementString("value", blog.blogId);
				data.WriteEndElement();

				// blogName
				data.WriteStartElement("member");
				data.WriteElementString("name", "blogName");
				data.WriteElementString("value", blog.blogName);
				data.WriteEndElement();

				data.WriteEndElement();
				data.WriteEndElement();

			}

			// Close tags
			data.WriteEndElement();
			data.WriteEndElement();
			data.WriteEndElement();
			data.WriteEndElement();

		}

		/// <summary>
		/// Convert Date to format expected by MetaWeblog Response.
		/// </summary>
		/// <param name="date">DateTime to convert</param>
		/// <returns>ISO8601 date string</returns>
		private string ConvertDatetoISO8601(DateTime date)
		{
			string temp = date.Year.ToString() + date.Month.ToString().PadLeft(2, '0') + date.Day.ToString().PadLeft(2, '0') +
				"T" + date.Hour.ToString().PadLeft(2, '0') + ":" + date.Minute.ToString().PadLeft(2, '0') + ":" + date.Second.ToString().PadLeft(2, '0');
			return temp;
		}

		#endregion

	}
}
