using hazi.WEB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace hazi.WEB.Logic
{
    public class HeaderMenu
    {
        public static void HeaderMenuBeallitas(System.Web.UI.HtmlControls.HtmlGenericControl LogOffButtonDiv,
            Repeater BalMenuRepeater, Repeater JobbMenuRepeater)
        {
            List<MyMenuItem> balmenuitems = new List<MyMenuItem>();
            List<MyMenuItem> jobbmenuitems = new List<MyMenuItem>();
            //Bal Menü
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                balmenuitems.Add(new MyMenuItem() { Text = "Bejelentések", Link = "/" });

                if (!RoleActions.IsInRole(HttpContext.Current.User.Identity.Name, RegisterUserAs.NormalUser.ToString()))
                    balmenuitems.Add(new MyMenuItem() { Text = "Jóváhagyások", Link = "/Pages/OsszegzoForm" });

                if (RoleActions.IsInRole(HttpContext.Current.User.Identity.Name, RegisterUserAs.Admin.ToString()))
                    jobbmenuitems.Add(new MyMenuItem() { Text = "Admin oldal", Link = "/Pages/AdminPage" });

                jobbmenuitems.Add(new MyMenuItem()
                {
                    Text = "Hello, " + HttpContext.Current.User.Identity.Name +
                        " !",
                    Link = "/Account/Manage"
                });
                LogOffButtonDiv.Visible = true;
            }
            else
            {
                jobbmenuitems.Add(new MyMenuItem() { Text = "Login", Link = "/Account/Login" });
                jobbmenuitems.Add(new MyMenuItem() { Text = "Register", Link = "/Account/Register" });
                LogOffButtonDiv.Visible = false;
            }

            BalMenuRepeater.DataSource = balmenuitems;
            BalMenuRepeater.DataBind();

            JobbMenuRepeater.DataSource = jobbmenuitems;
            JobbMenuRepeater.DataBind();  
        }
    }
}