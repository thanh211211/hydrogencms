﻿using System;
using System.IO;
using System.Web;

namespace HydrogenCms.Handlers
{
	/// <summary>
	/// The FileHandler serves all files that is uploaded from
	/// the admin pages except for images. 
	/// </summary>
	/// <remarks>
	/// By using a HttpHandler to serve files, it is very easy
	/// to add the capability to stop bandwidth leeching or
	/// to create a statistics analysis feature upon it.
	///
	/// Taken from BlogEngine.net 1.3
	/// </remarks>
	public class FileHandler : IHttpHandler
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
		/// Enables processing of HTTP Web requests by a custom HttpHandler that 
		/// implements the <see cref="T:System.Web.IHttpHandler"></see> interface.
		/// </summary>
		/// <param name="context">An <see cref="T:System.Web.HttpContext"></see> 
		/// object that provides references to the intrinsic server objects 
		/// (for example, Request, Response, Session, and Server) used to service HTTP requests.
		/// </param>
		public void ProcessRequest(HttpContext context)
		{
			if (!string.IsNullOrEmpty(context.Request.QueryString["file"]))
			{
				string fileName = context.Request.QueryString["file"];
				OnServing(fileName);

				try
				{
					string basePath = Application.Settings.MediaPath;
					string file = context.Server.MapPath(Path.Combine(basePath, fileName));

					//
					// Verify that the request is for a file located in the basePath, disallowing ../../ requests.
					//
					basePath = context.Server.MapPath(basePath);

					if (file.Substring(0, basePath.Length).ToUpper() != basePath.ToUpper())
					{
						OnBadRequest(fileName);
						context.Response.Status = "404 Bad Request";
					}
					else
					{
						FileInfo fileInfo = new FileInfo(file);

						if (fileInfo.Exists)
						{
							context.Response.AppendHeader("Content-Disposition", "inline; filename=" + fileName);
							SetContentType(context, fileName);

							context.Response.TransmitFile(fileInfo.FullName);
							OnServed(fileName);
						}
						else
						{
							OnBadRequest(fileName);
							context.Response.Status = "404 Bad Request";
						}
					}
				}
				catch (Exception)
				{
					OnBadRequest(fileName);
					context.Response.Status = "404 Bad Request";
				}
			}
		}

		/// <summary>
		/// Sets the content type depending on the filename's extension.
		/// </summary>
		private static void SetContentType(HttpContext context, string fileName)
		{
			if (fileName.EndsWith(".pdf"))
			{
				context.Response.AddHeader("Content-Type", "application/pdf");
			}
			else
			{
				context.Response.AddHeader("Content-Type", "application/octet-stream");
			}
		}

		#endregion

		#region Events

		/// <summary>
		/// Occurs when the requested file does not exist;
		/// </summary>
		public static event EventHandler<EventArgs> Serving;

		private static void OnServing(string file)
		{
			if (Serving != null)
			{
				Serving(file, EventArgs.Empty);
			}
		}

		/// <summary>
		/// Occurs when a file is served;
		/// </summary>
		public static event EventHandler<EventArgs> Served;

		private static void OnServed(string file)
		{
			if (Served != null)
			{
				Served(file, EventArgs.Empty);
			}
		}

		/// <summary>
		/// Occurs when the requested file does not exist;
		/// </summary>
		public static event EventHandler<EventArgs> BadRequest;

		private static void OnBadRequest(string file)
		{
			if (BadRequest != null)
			{
				BadRequest(file, EventArgs.Empty);
			}
		}

		#endregion

	}

}