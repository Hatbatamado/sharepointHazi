using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using hazi.WEB.Models;
using hazi.WEB.Logic;

namespace hazi.WEB.Account
{
    public partial class Register : Page
    {
        protected void Page_load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                if (User.Identity.IsAuthenticated)
                    Response.Redirect("/Account/Login");
                else
                {
                    VezetoDDL.DataSource = UsersBLL.GetUserNames();
                    VezetoDDL.DataBind();
                }
            }
        }
        protected void CreateUser_Click(object sender, EventArgs e)
        {
            RoleActions roleA = new RoleActions();
            string error = roleA.createUserAs(UserName.Text, Password.Text, RegisterUserAs.NormalUser);
            if (error != "")
                ErrorMessage.Text = error;

            UsersBLL.ProfilMentes(Server, PictureFileUpload, ErrorMessage, UserName.Text,
                SzuletesiTB.Text, VezetoDDL.SelectedValue, Response, "/");
        }
    }
}