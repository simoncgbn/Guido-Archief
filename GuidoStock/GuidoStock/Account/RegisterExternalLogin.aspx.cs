using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Web;
using GuidoStock.App_Code;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Owin;
using GuidoStock.Models;

namespace GuidoStock.Account
{
    public partial class RegisterExternalLogin : System.Web.UI.Page
    {
        protected string ProviderName
        {
            get { return (string)ViewState["ProviderName"] ?? String.Empty; }
            private set { ViewState["ProviderName"] = value; }
        }

        protected string ProviderAccountKey
        {
            get { return (string)ViewState["ProviderAccountKey"] ?? String.Empty; }
            private set { ViewState["ProviderAccountKey"] = value; }
        }

        private void RedirectOnFail()
        {
            Response.Redirect((User.Identity.IsAuthenticated) ? "~/Dashboard/Overzicht" : "~/Account/Login");
        }

        protected void Page_Init()
        {
            // Process the result from an auth provider in the request
            ProviderName = IdentityHelper.GetProviderNameFromRequest(Request);
            if (String.IsNullOrEmpty(ProviderName))
            {
                RedirectOnFail();
                return;
            }
            if (!IsPostBack)
            {
                var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var signInManager = Context.GetOwinContext().Get<ApplicationSignInManager>();
                var loginInfo = Context.GetOwinContext().Authentication.GetExternalLoginInfo();
                if (loginInfo == null)
                {
                    RedirectOnFail();
                    return;
                }
                var user = manager.Find(loginInfo.Login);
                if (user != null)
                {
                    signInManager.SignIn(user, isPersistent: false, rememberBrowser: false);
                    IdentityHelper.RedirectToReturnUrl(Request.QueryString["ReturnUrl"], Response);
                }
                else if (User.Identity.IsAuthenticated)
                {
                    // Apply Xsrf check when linking
                    var verifiedloginInfo = Context.GetOwinContext().Authentication.GetExternalLoginInfo(IdentityHelper.XsrfKey, User.Identity.GetUserId());
                    if (verifiedloginInfo == null)
                    {
                        RedirectOnFail();
                        return;
                    }

                    var result = manager.AddLogin(User.Identity.GetUserId(), verifiedloginInfo.Login);
                    if (result.Succeeded)
                    {
                        IdentityHelper.RedirectToReturnUrl(Request.QueryString["ReturnUrl"], Response);
                    }
                    else
                    {
                        AddErrors(result);
                        return;
                    }
                }
                else
                {
                    var currentUser = new ApplicationUser()
                    {
                        UserName = loginInfo.DefaultUserName, Email = loginInfo.DefaultUserName,
                        FirstName = loginInfo.ExternalIdentity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.GivenName).Value,
                        SurName = loginInfo.ExternalIdentity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Surname).Value,
                        Name = loginInfo.ExternalIdentity.Claims.FirstOrDefault(c => c.Type == "name").Value
                    };
                    IdentityResult result = manager.Create(currentUser);
                    if (result.Succeeded)
                    {
                        result = manager.AddLogin(currentUser.Id, loginInfo.Login);
                        manager.AddToRole(currentUser.Id, "Gebruiker");
                        DBClass db = new DBClass();
                        db.AddGebruiker(currentUser.FirstName, currentUser.SurName, currentUser.PhoneNumber, currentUser.Email);
                        if (result.Succeeded)
                        {
                            signInManager.SignIn(currentUser, false, false);
                            IdentityHelper.RedirectToReturnUrl(Request.QueryString["ReturnUrl"], Response);
                            return;
                        }
                    }
                }
            }
        }

        private void AddErrors(IdentityResult result) 
        {
            foreach (var error in result.Errors) 
            {
                ModelState.AddModelError("", error);
            }
        }
    }
}