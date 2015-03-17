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
                {
                    List<UjBejelentes> lista = GetIdoBejelentesek();
                    bejelentesekLista.DataSource = lista;
                    bejelentesekLista.DataBind();

                    Bejelentesek.Visible = true;
                    //ha újból futattjuk és cookie-val bent vagyunk,
                    //akkor a User.IsInRole szerint nem vagyunk bent az adott szerepkörben
                    //emiatt az egész alkalmazásban lecseréltem
                    if (RoleActions.GetRole(User.Identity.Name) == RegisterUserAs.Admin.ToString())
                    {
                        for (int i = 0; i < bejelentesekLista.Rows.Count; i++)
                        {
                            bejelentesekLista.Rows[i].FindControl("Remove").Visible = false;
                            bejelentesekLista.Rows[i].FindControl("StatusDDL").Visible = true;
                        }
                    }
                    else if (RoleActions.GetRole(User.Identity.Name) == RegisterUserAs.NormalUser.ToString())
                    {
                        for (int i = 0; i < bejelentesekLista.Rows.Count; i++)
                        {
                            bejelentesekLista.Rows[i].FindControl("Remove").Visible = true;
                            bejelentesekLista.Rows[i].FindControl("StatusDDL").Visible = false;
                        }
                    }                   
                }
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

        protected void Ujbejelentes_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Pages/Bejelento");
        }

        protected void BejelentesTorles_Click(object sender, EventArgs e)
        {

        }
    }
}