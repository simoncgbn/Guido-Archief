using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using GuidoStock.Models;
using Microsoft.AspNet.Identity;
using System.Diagnostics;
using System.Web.Providers.Entities;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using GuidoStock.Code;
namespace GuidoStock
{
    public partial class SiteMaster : MasterPage
    {
        private const string AntiXsrfTokenKey = "__AntiXsrfToken";
        private const string AntiXsrfUserNameKey = "__AntiXsrfUserName";
        private string _antiXsrfTokenValue;

        private string Test
        {
            get { return (string) ViewState["test"]; }
            set { ViewState["test"] = value; }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            Session["value"] = 0;
            // The code below helps to protect against XSRF attacks
            var requestCookie = Request.Cookies[AntiXsrfTokenKey];
            Guid requestCookieGuidValue;
            if (requestCookie != null && Guid.TryParse(requestCookie.Value, out requestCookieGuidValue))
            {
                // Use the Anti-XSRF token from the cookie
                _antiXsrfTokenValue = requestCookie.Value;
                Page.ViewStateUserKey = _antiXsrfTokenValue;
            }
            else
            {
                // Generate a new Anti-XSRF token and save to the cookie
                _antiXsrfTokenValue = Guid.NewGuid().ToString("N");
                Page.ViewStateUserKey = _antiXsrfTokenValue;

                var responseCookie = new HttpCookie(AntiXsrfTokenKey)
                {
                    HttpOnly = true,
                    Value = _antiXsrfTokenValue
                };
                if (FormsAuthentication.RequireSSL && Request.IsSecureConnection)
                {
                    responseCookie.Secure = true;
                }
                Response.Cookies.Set(responseCookie);
            }

            Page.PreLoad += master_Page_PreLoad; 
        }

        protected void master_Page_PreLoad(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Set Anti-XSRF token
                ViewState[AntiXsrfTokenKey] = Page.ViewStateUserKey;
                ViewState[AntiXsrfUserNameKey] = Context.User.Identity.Name ?? String.Empty;
            }
            else
            {
                // Validate the Anti-XSRF token
                if ((string)ViewState[AntiXsrfTokenKey] != _antiXsrfTokenValue
                    || (string)ViewState[AntiXsrfUserNameKey] != (Context.User.Identity.Name ?? String.Empty))
                {
                    throw new InvalidOperationException("Validation of Anti-XSRF token failed.");
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
            if (!Request.IsAuthenticated || !System.Web.HttpContext.Current.User.Identity.IsAuthenticated || manager == null)
            {
                if (!Request.RawUrl.Contains("Login"))
                    RedirectToLogin();
            }
            else
            {
                var currentUser = manager.FindByName(HttpContext.Current.User.Identity.Name);
                var urlParts = Request.RawUrl.Split('/');
                if (!RechtenManager.Instance.VerifyAccess(urlParts[1], urlParts[2], currentUser, manager))
                {
                    var curPage = this.Page.Request.FilePath;
                    if (curPage == "/Dashboard/Overzicht") return;
                    Response.Redirect("~/Dashboard/Overzicht.aspx");
                }  
            }
        }

        protected void Unnamed_LoggingOut(object sender, LoginCancelEventArgs e)
        {
            Context.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            Response.Redirect("http://login.microsoftonline.com/logout.srf");
        }

        private void RedirectToLogin()
        {
            string page = Page.Request.FilePath;
            if (page != "/Account/Login")
                Response.Redirect("~/Account/Login");
        }

        protected void OnClick(object sender, EventArgs e)
        {
            Context.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            Response.Redirect("http://login.microsoftonline.com/logout.srf");
        }
    }

}