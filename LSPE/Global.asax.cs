using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.Security;

namespace LSPE
{
    public class Global1 : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            Application["strConnection"] = "server=198.237.190.87;uid=rxpress;pwd=rxpress;database=ResolutionExpress";

            SqlConnection conn = new SqlConnection(Application["strConnection"].ToString());
            SqlCommand cmd = new SqlCommand("prSelectCurrentYear", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection.Open();
            Application["CR_YR_ID"] = cmd.ExecuteScalar();
            cmd.Connection.Close();
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            // Extract the forms authentication cookie
            string cookieName = FormsAuthentication.FormsCookieName;
            HttpCookie authCookie = Context.Request.Cookies[cookieName];

            if (null == authCookie)
            {
                // There is no authentication cookie.
                return;
            }

            FormsAuthenticationTicket authTicket = null;
            try
            {
                authTicket = FormsAuthentication.Decrypt(authCookie.Value);
            }
            catch (Exception ex)
            {
                // Need to log exception details
                return;
            }

            if (null == authTicket)
            {
                // Cookie failed to decrypt.
                return;
            }

            // When the ticket was created, the UserData property was assigned a
            // pipe delimited string of user information.
            string[] strarrUserData = authTicket.UserData.Split('|');

            // Create an Identity object
            FormsIdentity id = new FormsIdentity(authTicket);

            // This principal will flow throughout the request.
            CustomPrincipal cprPrincipal = new CustomPrincipal(id, strarrUserData[0]);
            cprPrincipal.UserId = Convert.ToInt32(strarrUserData[1]);
            cprPrincipal.UserName = strarrUserData[2];
            cprPrincipal.FirstName = strarrUserData[3];
            cprPrincipal.LastName = strarrUserData[4];
            cprPrincipal.Email = strarrUserData[5];
            // Attach the new principal object to the current HttpContext object
            Context.User = cprPrincipal;
        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}