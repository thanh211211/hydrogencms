using System;
using System.Text;
using System.Web;

namespace HydrogenCms.Models
{
	public partial class Page : Validator
	{
		#region Validation

		public override void Validate()
		{
			this.AddRule("Title", "Title must be set.", string.IsNullOrEmpty(this.Title));
			this.AddRule("Slug", "Slug must be set.", string.IsNullOrEmpty(this.Slug));
			this.AddRule("Slug", "Slug cannot be 'Admin'.", "admin".Equals(this.Slug, StringComparison.CurrentCultureIgnoreCase));
		}

		#endregion

		#region Base overrides

		/// <summary>
		/// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
		/// </returns>
		public override string ToString()
		{
			return this.Title == null ? "Null" : this.Title;
		}

		#endregion

		#region Events

		/// <summary>
		/// Occurs when the page is being served to the output stream.
		/// </summary>
		public static event EventHandler<EventArgs> Serving;

		/// <summary>
		/// Raises the event in a safe way
		/// </summary>
		public static void OnServing(Page page, EventArgs arg)
		{
			if (Serving != null)
			{
				Serving(page, arg);
			}
		}

		#endregion

	}
}
