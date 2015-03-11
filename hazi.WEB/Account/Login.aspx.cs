using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using hazi.WEB.Models;
using System.Collections.Generic;
using hazi.WEB.Logic;

namespace hazi.WEB.Account
{
    public partial class Login : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!User.Identity.IsAuthenticated)
                {
                    LoginForm.Visible = true;
                    RegisterHyperLink.NavigateUrl = "Register";
                    OpenAuthLogin.ReturnUrl = Request.QueryString["ReturnUrl"];
                    var returnUrl = HttpUtility.UrlEncode(Request.QueryString["ReturnUrl"]);
                    if (!String.IsNullOrEmpty(returnUrl))
                    {
                        RegisterHyperLink.NavigateUrl += "?ReturnUrl=" + returnUrl;
                    }
                }
                else
                {
                    LoginForm.Visible = false;
                    SucLogin.Visible = true;
                    helloLabel.Text = "Hello " + User.Identity.GetUserName();
                }
            }
        }

        protected void LogIn(object sender, EventArgs e)
        {
            if (IsValid)
            {
                Models.ApplicationDbContext context = new ApplicationDbContext();
                var userMgr = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                var manager = new UserManager();

                ApplicationUser user = userMgr.Find(UserName.Text, Password.Text);
                if (user != null)
                {
                    //Felhasználóhoz való szerepkör megkeresése
                    IList<string> roles = userMgr.GetRoles(user.Id);
                    //Ha nincs, akkor adja hozzá a user-t, a NormalUser-ekhez
                    if (roles.Count == 0)
                        RoleActions.AddToRole(user, RegisterUserAs.NormalUser, userMgr, context);

                    IdentityHelper.SignIn(manager, user, RememberMe.Checked);
                    //token hiba elkerülése miatt, az oldal újratöltése
                    Response.Redirect("./Login.aspx");
                }
                else
                {
                    FailureText.Text = "Invalid username or password.";
                    ErrorMessage.Visible = true;
                }
            }
        }

        protected void buttonLogoff_Click(object sender, EventArgs e)
        {
            var authenticationManager = HttpContext.Current.GetOwinContext().Authentication;
            authenticationManager.SignOut();
            Response.Redirect("./Login.aspx");
        }
    }
}