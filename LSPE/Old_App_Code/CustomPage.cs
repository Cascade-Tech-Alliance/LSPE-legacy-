using System;
using System.IO;
using System.Web.UI;

namespace LSPE
{
	/// <summary>
	/// Summary description for CustomPage.
	/// </summary>
	public class CustomPage : System.Web.UI.Page
	{
		public CustomPage()
		{
		}

		protected override void SavePageStateToPersistenceMedium(object viewState)
		{
			// serialize the view state into a base-64 encoded string
			LosFormatter los = new LosFormatter();
			StringWriter writer = new StringWriter();
			los.Serialize(writer, viewState);
			// save the string to disk
			StreamWriter sw = File.CreateText(ViewStateFilePath);
			sw.Write(writer.ToString());
			sw.Close();
		}
		protected override object LoadPageStateFromPersistenceMedium()
		{
			// determine the file to access
			if (!File.Exists(ViewStateFilePath))
				return null;
			else
			{
				// open the file
				StreamReader sr = File.OpenText(ViewStateFilePath);
				string viewStateString = sr.ReadToEnd();
				sr.Close();
				// deserialize the string
				LosFormatter los = new LosFormatter();
				return los.Deserialize(viewStateString);
			}
		}
		public string ViewStateFilePath
		{
			get
			{
				string folderName = 
					Path.Combine(Request.PhysicalApplicationPath, 
					"PersistedViewState");
				string fileName = Session.SessionID + "-" + 
					Path.GetFileNameWithoutExtension(Request.Path).Replace("/", 
					"-") + ".vs";
				return Path.Combine(folderName, fileName);
			}
		}

		override protected void OnInit(EventArgs e)
		{
			base.OnInit(e);

			// Check if the given page supports session or not (this tested as reliable indicator 
			// if EnableSessionState is true), do not care about a page that does not need session state.
			if (Context.Session != null)
			{
				// If a new session is detected, but a cookie already exists, then the page has timed out.
				if (Session.IsNewSession)
				{
					string szCookieHeader = Request.Headers["Cookie"];
					if ((null != szCookieHeader) && (szCookieHeader.IndexOf("ASP.NET_SessionId") >= 0))
					{
						Response.Redirect("timeout.aspx");
					}  
				} 
			}
		}
	}
}
