using hazi.DAL;
using hazi.WEB.Logic;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace hazi.WEB.Pages
{
    public partial class OsszegzoForm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (User.Identity.IsAuthenticated &&
                    (RoleActions.GetRole(User.Identity.Name) == RegisterUserAs.Admin.ToString()) ||
                    RoleActions.GetRole(User.Identity.Name) == RegisterUserAs.Jovahagyok.ToString())
                {
                    Jovahagyas.Visible = true;
                    List<UjBejelentes> lista = JovahagyBLL.GetJovahagyAll();
                    Jovahagyas.DataSource = lista;
                    Jovahagyas.DataBind();

                    if (lista.Count == 0)
                        Mentes.Visible = false;
                }
            }
        }

        protected void Mentes_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < Jovahagyas.Rows.Count; i++)
            {
                IOrderedDictionary rowValues = new OrderedDictionary();
                rowValues = Utility.GetValues(Jovahagyas.Rows[i]);

                string ddlSelected = (Jovahagyas.Rows[i].FindControl("StatuszDDL") as DropDownList).SelectedValue;

                if (rowValues["JovaStatus"].ToString() != ddlSelected)
                    JovahagyBLL.JovahagyasMentes(Convert.ToInt32(rowValues["ID"]), ddlSelected);
            }

            Jovahagyas.DataSource = JovahagyBLL.GetJovahagyAll();
            Jovahagyas.DataBind();
        }
    }
}