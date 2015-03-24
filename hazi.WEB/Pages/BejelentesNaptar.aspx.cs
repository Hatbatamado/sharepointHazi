using hazi.BLL;
using hazi.DAL;
using hazi.WEB.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace hazi.WEB.Pages
{
    public partial class BejelentesNaptar : System.Web.UI.Page
    {
        public int? Id
        {
            get
            {
                return (int?)ViewState["Id"];
            }
            set
            {
                ViewState["Id"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["ID"] != null)
            {
                Bejelentes.UjBejelentes = false;
                Id = Int32.Parse(Request.QueryString["ID"]);
                List<UjBejelentes> ib = new List<UjBejelentes>();
                if (RoleActions.GetRole(User.Identity.Name) != RegisterUserAs.Admin.ToString())
                    ib = JogcimBLL.GetIdoBejelentesById(Id.Value, User.Identity.Name, false);
                else
                    ib = JogcimBLL.GetIdoBejelentesById(Id.Value, User.Identity.Name, true);
                BejelentesNaptarNezetGV.DataSource = ib;
                BejelentesNaptarNezetGV.DataBind();
            }
        }
    }
}