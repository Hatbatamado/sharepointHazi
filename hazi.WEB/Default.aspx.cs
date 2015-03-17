using hazi.DAL;
using hazi.WEB.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.ModelBinding;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace hazi.WEB
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (User.Identity.IsAuthenticated)
                    Bejelentesek.Visible = true;
                else
                {
                    Bejelentesek.Visible = false;
                    Response.Redirect("/Account/Login.aspx");
                }
            }
        }

        public List<UjBejelentes> GetIdoBejelentesek()
        {
            string admin = RegisterUserAs.Admin.ToString();
            string normal = RegisterUserAs.NormalUser.ToString();

            if (User.Identity.IsAuthenticated)
            {
                if (RoleActions.GetRole(User.Identity.Name) == admin)
                {
                    return Bejelentes.GetIdoBejelentesek(admin,"",
                        DateTime.MinValue, DateTime.MinValue);
                }
                if (RoleActions.GetRole(User.Identity.Name) == normal)
                {
                    return Bejelentes.GetIdoBejelentesek(normal,
                        User.Identity.Name, DateTime.MinValue, DateTime.MinValue);
                }
            }
            return null;
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Pages/Bejelento");
        }
    }
}