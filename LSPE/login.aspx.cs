using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.Security;

namespace LSPE
{
    /// <summary>
    /// Summary description for login.
    /// </summary>
    public partial class login : System.Web.UI.Page
    {
        int m_intUserId;
        string m_strUserType;
        string m_strFirstName;
        string m_strLastName;
        string m_strEmail;
        
        protected void Page_Load(object sender, System.EventArgs e)
        {
            lblLoginError.Visible = false;
        }
        
        protected void btnLogin_Click(object sender, System.EventArgs e)
        {
            if (txtUserName.Value.Length < 1 || txtPassword.Value.Length < 1)
            {
                lblLoginError.Visible = true;
            }
            else
            {
                bool blnIsAuthenticated = IsAuthenticated( txtUserName.Value, txtPassword.Value );
                
                if (blnIsAuthenticated == true )
                {
                    string strUserData = m_strUserType + '|' + m_intUserId + '|' + txtUserName.Value 
                        + '|' + m_strFirstName + '|' + m_strLastName + '|'+ m_strEmail;
            
                    // Create the authentication ticket
                    FormsAuthenticationTicket authTicket = 
                        new FormsAuthenticationTicket(1, // Version
                        txtUserName.Value,				  // User Name
                        DateTime.Now,					  // Creation
                        DateTime.Now.AddMinutes(60),     // Expiration
                        false,							  // Persistent
                        strUserData );					  // User Data
                    
                    // Encrypt the ticket and add it to the cookie as data.
                    string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
                    HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                    Response.Cookies.Add(authCookie);

                    // Load list of available departments
                    string strConnection = (string)Application["strConnection"];
                    SqlConnection sqlConnection = new SqlConnection(strConnection);
    
                    SqlCommand sqlCommand = new SqlCommand();
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.CommandText = "prSelectDepartmentsByUser";
                    sqlCommand.CommandType = CommandType.StoredProcedure;

                    sqlCommand.Parameters.Add(new SqlParameter("@intUS_ID", SqlDbType.Int));
                    sqlCommand.Parameters["@intUS_ID"].Value = m_intUserId;

                    SqlDataReader sqlReader;
                    ArrayList arrlstDepartments = new ArrayList();

                    sqlConnection.Open();
                    sqlReader = sqlCommand.ExecuteReader();
                    while (sqlReader.Read())
                    {
                        arrlstDepartments.Add(sqlReader["DP_Name"] + "|" + sqlReader["DP_ID"]);	
                    }
                    sqlConnection.Close();
            
                    string[] strarrDepartments = new string[arrlstDepartments.Count];
                    arrlstDepartments.CopyTo(strarrDepartments, 0);
                    Session["strarrDepartments"] = strarrDepartments;
                    
                    // Redirect user to requested page.
                    Response.Redirect( FormsAuthentication.GetRedirectUrl(txtUserName.Value, false ));
                }
                
                lblLoginError.Visible = !blnIsAuthenticated;
            }
        }

        public bool IsAuthenticated (string strUserName, string strPasswordIn)
        {
            try
            {
                // validate user credentials passed by a login client
                bool blnIsAuthenticated = false;
                
                string strConnection = (string)Application["strConnection"];
                SqlConnection sqlConnection = new SqlConnection(strConnection);
      
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
                m_intUserId = (int)sqlCommand.Parameters["@intUS_ID"].Value;
                m_strUserType = (string)sqlCommand.Parameters["@chvUT_Name"].Value;
                m_strFirstName = (string)sqlCommand.Parameters["@chvUS_NameFirst"].Value;
                m_strLastName = (string)sqlCommand.Parameters["@chvUS_NameLast"].Value;
                m_strEmail = (string)sqlCommand.Parameters["@chvUS_Email"].Value;
                
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

    }
}
