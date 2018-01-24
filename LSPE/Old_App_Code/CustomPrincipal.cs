using System;
using System.Security.Principal;

namespace LSPE
{
	/// <summary>
	/// This class implements and extends the IPrincipal interface.
	/// </summary>
	public class CustomPrincipal : IPrincipal
	{
		private IIdentity _ideIdentity;
		private int _intUserId;
		private string _strUserType;
		private string _strUserName;
		private string _strFirstName;
		private string _strLastName;
		private string _strEmail;

		public CustomPrincipal(IIdentity ideIdentity, string strUserType)
		{
			_ideIdentity = ideIdentity;
			_strUserType = strUserType;
		}
		#region IPrincipal Members

		public IIdentity Identity
		{
			get { return _ideIdentity; }
		}

		public bool IsInRole(string role)
		{
			return role == _strUserType ? true : false;
		}

		#endregion
		
		public int UserId
		{
			get { return _intUserId; }
			set { _intUserId = value; }
		}

		public string UserName
		{
			get    { return _strUserName; }
			set { _strUserName = value; }
		}

		public string FirstName
		{
			get    { return _strFirstName; }
			set { _strFirstName = value; }
		}

		public string LastName
		{
			get    { return _strLastName; }
			set { _strLastName = value; }
		}

		public string Email
		{
			get    { return _strEmail; }
			set { _strEmail = value; }
		}
	}
}
