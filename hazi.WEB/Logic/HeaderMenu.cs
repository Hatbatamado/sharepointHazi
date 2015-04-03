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
        /// <summary>
        /// Header linkek beállítása felhasználói szerepkör alapján
        /// </summary>
        /// <param name="LogOffButtonDiv"></param>
        /// <param name="BalMenuRepeater"></param>
        /// <param name="JobbMenuRepeater"></param>
        public static void HeaderMenuBeallitas(System.Web.UI.HtmlControls.HtmlGenericControl LogOffButtonDiv,
            Repeater BalMenuRepeater, Repeater JobbMenuRepeater)
        {
            List<MyMenuItem> balmenuitems = new List<MyMenuItem>();
            List<MyMenuItem> jobbmenuitems = new List<MyMenuItem>();

            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                balmenuitems.Add(new MyMenuItem() { Text = "Bejelentések", Link = "/" });

                if (!RoleActions.IsInRole(HttpContext.Current.User.Identity.Name, RegisterUserAs.NormalUser.ToString()))
                    balmenuitems.Add(new MyMenuItem() { Text = "Jóváhagyások", Link = Konstansok.RedirectOsszegzoForm });

                balmenuitems.Add(new MyMenuItem() { Text = "Éves áttekintő", Link = Konstansok.RedirectAttekinto });

                if (RoleActions.IsInRole(HttpContext.Current.User.Identity.Name, RegisterUserAs.Admin.ToString()))
                    jobbmenuitems.Add(new MyMenuItem() { Text = "Admin oldal", Link = Konstansok.RedirectAdminOldal });

                jobbmenuitems.Add(new MyMenuItem()
                {
                    Text = "Hello, " + HttpContext.Current.User.Identity.Name +
                        " !",
                    Link = Konstansok.RedirectAccoutManage
                });
                LogOffButtonDiv.Visible = true;
            }
            else
            {
                jobbmenuitems.Add(new MyMenuItem() { Text = "Login", Link = Konstansok.RedirectAccoutLogin });
                jobbmenuitems.Add(new MyMenuItem() { Text = "Register", Link = Konstansok.RedirectAccoutRegister });
                LogOffButtonDiv.Visible = false;
            }

            BalMenuRepeater.DataSource = balmenuitems;
            BalMenuRepeater.DataBind();

            JobbMenuRepeater.DataSource = jobbmenuitems;
            JobbMenuRepeater.DataBind();  
        }
    }
}