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

            Boolean fileOK = false;
            String path = Server.MapPath("~/Images/");
            if (PictureFileUpload.HasFile)
            {
                String fileExtension = System.IO.Path.GetExtension(PictureFileUpload.FileName).ToLower();
                String[] allowedExtensions = { ".gif", ".png", ".jpeg", ".jpg" };
                for (int i = 0; i < allowedExtensions.Length; i++)
                {
                    if (fileExtension == allowedExtensions[i])
                    {
                        fileOK = true;
                    }
                }
            }

            if (fileOK)
            {
                try
                {
                    // Save to Images folder.
                    PictureFileUpload.PostedFile.SaveAs(path + PictureFileUpload.FileName);
                }
                catch (Exception ex)
                {
                    ErrorMessage.Text = ex.Message;
                }
            }
            else
            {
                ErrorMessage.Text = "Unable to accept file type.";
            }

            if (fileOK)
                UsersBLL.FelhasznaloiAdatokMentese(UserName.Text, SzuletesiTB.Text, VezetoDDL.SelectedValue, "/Images/" + PictureFileUpload.FileName);
            else
                UsersBLL.FelhasznaloiAdatokMentese(UserName.Text, SzuletesiTB.Text, VezetoDDL.SelectedValue, "");

            Response.Redirect("/");
        }
    }
}