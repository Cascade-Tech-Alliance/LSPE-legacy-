using System;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Web;
using System.Web.Security;

namespace LSPE
{
	/// <summary>
	/// Summary description for change_password.
	/// </summary>
	public partial class change_password : CustomPage
	{
		SqlConnection sqlConnection;
		CustomPrincipal cprPrincipal = HttpContext.Current.User as CustomPrincipal;		
		int m_intUS_ID = 0;
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			//
			if (!Page.IsPostBack)
			{
				lblMessage.Visible = false;
			}
		}

		public bool IsAuthenticated (string strUserName, string strPasswordIn)
		{
			try
			{
				// validate user credentials passed by a login client
				bool blnIsAuthenticated = false;
				
				string strConnection = (string)Application["strConnection"];
				sqlConnection = new SqlConnection(strConnection);
				
				SqlCommand sqlCommand = new SqlCommand();
				sqlCommand.Connection = sqlConnection;
				sqlCommand.CommandText = "prSelectUserInfo";
				sqlCommand.CommandType = CommandType.StoredProcedure;
			
				SqlParameter sqlParameter;
			
				sqlParameter = sqlCommand.Parameters.Add("@chvUS_Username", SqlDbType.VarChar, 30);
				sqlParameter.Direction = ParameterDirection.Input;
				sqlParameter.Value = strUserName;

				sqlParameter = sqlCommand.Parameters.Add("@chvUS_PasswordHash", SqlDbType.VarChar, 40);
				sqlParameter.Direction = ParameterDirection.Output;

				sqlParameter = sqlCommand.Parameters.Add("@chvUS_Salt", SqlDbType.VarChar, 10);
				sqlParameter.Direction = ParameterDirection.Output;

				sqlParameter = sqlCommand.Parameters.Add("@intUS_ID", SqlDbType.Int);
				sqlParameter.Direction = ParameterDirection.Output;

				sqlParameter = sqlCommand.Parameters.Add("@chvUT_Name", SqlDbType.VarChar, 30);
				sqlParameter.Direction = ParameterDirection.Output;

				sqlParameter = sqlCommand.Parameters.Add("@chvUS_NameFirst", SqlDbType.VarChar, 15);
				sqlParameter.Direction = ParameterDirection.Output;

				sqlParameter = sqlCommand.Parameters.Add("@chvUS_NameLast", SqlDbType.VarChar, 15);
				sqlParameter.Direction = ParameterDirection.Output;

				sqlParameter = sqlCommand.Parameters.Add("@chvUS_Email", SqlDbType.VarChar, 50);
				sqlParameter.Direction = ParameterDirection.Output;
				
				sqlConnection.Open();
				sqlCommand.ExecuteNonQuery();
							
				string strPasswordHash = (string)sqlCommand.Parameters["@chvUS_PasswordHash"].Value;
				string strSalt = (string)sqlCommand.Parameters["@chvUS_Salt"].Value;
				m_intUS_ID = (int)sqlCommand.Parameters["@intUS_ID"].Value;
								
				sqlConnection.Close();
				
				string strPasswordAndSalt = String.Concat(strPasswordIn, strSalt);
				string strHashedPasswordAndSalt = FormsAuthentication.HashPasswordForStoringInConfigFile(strPasswordAndSalt, "SHA1");
				blnIsAuthenticated = (strHashedPasswordAndSalt == strPasswordHash);
				
				return blnIsAuthenticated;
			}
			catch (Exception lExcep)
			{
				//To do:  Handle exception.
				return false;
			}
		}

		private void UpdatePassword (string strNewPassword)
		{
			try
			{
				string strConnection = (string)Application["strConnection"];
				sqlConnection = new SqlConnection(strConnection);
				
				SqlCommand sqlCommand = new SqlCommand();
				sqlCommand.Connection = sqlConnection;
				sqlCommand.CommandText = "prUpdatePassword";
				sqlCommand.CommandType = CommandType.StoredProcedure;
			
				sqlCommand.Parameters.Add(new SqlParameter("@intUS_ID", SqlDbType.Int));
				sqlCommand.Parameters.Add(new SqlParameter("@chvUS_PasswordHash", SqlDbType.VarChar, 40));
				sqlCommand.Parameters.Add(new SqlParameter("@chvUS_Salt", SqlDbType.VarChar, 10));
				sqlCommand.Parameters.Add(new SqlParameter("@intUS_ModifiedBy_US_ID", SqlDbType.Int));

				sqlCommand.Parameters["@intUS_ID"].Value = m_intUS_ID;
				string strSalt = CreateSalt(5);
				string strPasswordHash = CreatePasswordHash(strNewPassword, strSalt);
				sqlCommand.Parameters["@chvUS_PasswordHash"].Value = strPasswordHash;
				sqlCommand.Parameters["@chvUS_Salt"].Value = strSalt;
				sqlCommand.Parameters["@intUS_ModifiedBy_US_ID"].Value = m_intUS_ID;

				sqlConnection.Open();
				sqlCommand.ExecuteNonQuery();
								
				sqlConnection.Close();
			}
			catch (Exception lExcep)
			{
				//To do:  Handle exception.
			}
		}

		private static string CreateSalt(int intSize)
		{
			// Generate a cryptographic random number using the cryptographic service provider
			RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
			byte[] buff = new byte[intSize];
			rng.GetBytes(buff);
			// Return a Base64 string representation of the random number
			return Convert.ToBase64String(buff);
		}

		private static string CreatePasswordHash(string strPassword, string strSalt)
		{
			string strSaltAndPwd = String.Concat(strPassword, strSalt);
			string strHashedPwd = FormsAuthentication.HashPasswordForStoringInConfigFile(strSaltAndPwd, "SHA1");
			return strHashedPwd;
		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    

		}
		#endregion

		protected void lnkChangePassword_Click(object sender, System.EventArgs e)
		{
			if (Page.IsValid)
			{
				bool blnIsAuthenticated = IsAuthenticated(cprPrincipal.UserName, txtOldPassword.Text);
				lblIncorrectOldPassword.Visible = !blnIsAuthenticated;
				if (blnIsAuthenticated)
				{
					UpdatePassword(txtNewPassword.Text);
					lblMessage.Visible = true;
				}
			}
		}
	}
}
