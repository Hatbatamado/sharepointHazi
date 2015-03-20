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
using System.Web.Security;
using System.Web.UI.WebControls;
using System.Collections.Specialized;

namespace hazi.WEB.Account
{
    public partial class Login : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                RegisterHyperLink.NavigateUrl = "Register";
                //OpenAuthLogin.ReturnUrl = Request.QueryString["ReturnUrl"];
                var returnUrl = HttpUtility.UrlEncode(Request.QueryString["ReturnUrl"]);
                if (!String.IsNullOrEmpty(returnUrl))
                {
                    RegisterHyperLink.NavigateUrl += "?ReturnUrl=" + returnUrl;
                }
                if (User.Identity.IsAuthenticated && RoleActions.GetRole(User.Identity.Name) == RegisterUserAs.Admin.ToString())
                {
                    Felhasznalok.DataSource = UsersBLL.UserList();
                    Felhasznalok.DataBind();
                }
            }
            if (!User.Identity.IsAuthenticated || Request["__EVENTARGUMENT"] == "SikeresKilepes")
            {
                LoginForm.Visible = true;
                SucLogin.Visible = false;
            }
            else if (User.Identity.IsAuthenticated || Request["__EVENTARGUMENT"] == "SikeresBelepes")
            {
                LoginForm.Visible = false;
                SucLogin.Visible = true;
                helloLabel.Text = "Hello " + User.Identity.GetUserName();
                //todo gomb ide
                Felhasznalok.DataSource = UsersBLL.UserList();
                Felhasznalok.DataBind();
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
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "asd", "Belepes();", true);
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
            ScriptManager.RegisterStartupScript(this, this.GetType(), "asd", "Kilepes();", true);
        }

        protected void Mentes_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < Felhasznalok.Rows.Count; i++)
            {
                IOrderedDictionary rowValues = new OrderedDictionary();
                rowValues = Utility.GetValues(Felhasznalok.Rows[i]);
                string ddlValue = (Felhasznalok.Rows[i].FindControl("SzerepkorDDL") as DropDownList).SelectedValue;

                //string vissza = RoleActions.ChangeRole(rowValues["Name"].ToString(), rowValues["Role"].ToString(),
                //    ;
            }
        }
    }
}