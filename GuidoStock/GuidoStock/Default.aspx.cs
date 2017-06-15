using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GuidoStock.Code;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace GuidoStock
{
    public partial class _Default : Page
    {

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
                Response.Redirect("Dashboard/Overzicht.aspx");
            }
            
        }
        private void RedirectToLogin()
        {
            string page = Page.Request.FilePath;
            if (page != "/Account/Login")
                Response.Redirect("~/Account/Login");
        }
    }
}