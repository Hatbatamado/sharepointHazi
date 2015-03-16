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
            if (User.Identity.IsAuthenticated)
            {
                if (RoleActions.GetRole(User.Identity.Name) == RegisterUserAs.Admin.ToString())
                {
                    using (hazi2Entities db = new hazi2Entities())
                    {
                        return (from b in db.IdoBejelentes1
                                join p in db.Jogcims on b.JogcimID equals p.ID
                                select new UjBejelentes
                                {
                                    ID = b.ID,
                                    KezdetiDatum = b.KezdetiDatum,
                                    VegeDatum = b.VegeDatum,
                                    JogcimID = b.JogcimID,
                                    UserName = b.UserName,
                                    LastEdit = b.LastEdit,
                                    JogcimNev = p.Cim
                                }).ToList();
                    }
                }
                if (RoleActions.GetRole(User.Identity.Name) == RegisterUserAs.NormalUser.ToString())
                {
                    using (hazi2Entities db = new hazi2Entities())
                    {


                        return (from b in db.IdoBejelentes1
                                join p in db.Jogcims on b.JogcimID equals p.ID
                                where b.UserName == User.Identity.Name
                                select new UjBejelentes
                                {
                                    ID = b.ID,
                                    KezdetiDatum = b.KezdetiDatum,
                                    VegeDatum = b.VegeDatum,
                                    JogcimID = b.JogcimID,
                                    UserName = b.UserName,
                                    LastEdit = b.LastEdit,
                                    JogcimNev = p.Cim
                                }).ToList();                              
                    }                    
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