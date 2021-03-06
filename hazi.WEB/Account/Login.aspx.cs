﻿using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using hazi.WEB.Models;
using System.Collections.Generic;
using System.Web.Security;
using System.Web.UI.WebControls;
using hazi.WEB.Logic;

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
                if (User.Identity.IsAuthenticated)
                {
                    SikerBelep();
                    HeaderMenu.HeaderMenuBeallitas(Master.LogOffGombDiv, Master.BalMRep, Master.JobbMRep);
                }
                else
                {
                    SikerKilep();
                    HeaderMenu.HeaderMenuBeallitas(Master.LogOffGombDiv, Master.BalMRep, Master.JobbMRep);
                }
            }
            if (Request["__EVENTARGUMENT"] == "SikeresKilepes")
            {
                SikerKilep();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "asd", "KilepesHeader();", true);
            }
            else if (Request["__EVENTARGUMENT"] == "SikeresBelepes")
            {
                SikerBelep();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "asd", "BelepesHeader();", true);
            }
            else if (Request["__EVENTARGUMENT"] == "SikeresKilepesHeader")
            {
                HeaderMenu.HeaderMenuBeallitas(Master.LogOffGombDiv, Master.BalMRep, Master.JobbMRep);
            }
            else if (Request["__EVENTARGUMENT"] == "SikeresBelepesHeader")
            {
                HeaderMenu.HeaderMenuBeallitas(Master.LogOffGombDiv, Master.BalMRep, Master.JobbMRep);
            }
        }

        private void SikerBelep()
        {
            LoginForm.Visible = false;
            SucLogin.Visible = true;
            helloLabel.Text = "Hello " + User.Identity.Name;
        }

        private void SikerKilep()
        {
            LoginForm.Visible = true;
            SucLogin.Visible = false;
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
                        RoleActions.AddToRole(user.UserName, RegisterUserAs.NormalUser);

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

        protected void SzerepB_Click(object sender, EventArgs e)
        {
            Response.Redirect(Konstansok.RedirectAdminOldal);
        }

        protected void OsszegB_Click(object sender, EventArgs e)
        {
            Response.Redirect(Konstansok.RedirectOsszegzoForm);
        }
    }
}