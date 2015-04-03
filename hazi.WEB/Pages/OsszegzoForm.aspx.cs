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
        string admin = RegisterUserAs.Admin.ToString();
        string jova = RegisterUserAs.Jovahagyok.ToString();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (User.Identity.IsAuthenticated &&
                    (RoleActions.IsInRole(User.Identity.Name, admin) ||
                    RoleActions.IsInRole(User.Identity.Name, jova)))
                {
                    Jovahagyas.Visible = true;
                    DDLFeltoltes();

                    GVFeltoltese(); 
                }
                else
                {
                    Error404.HibaDobas(Response);
                }
            }
        }
        /// <summary>
        /// Gridview feltöltése adatokkal
        /// </summary>
        private void GVFeltoltese()
        {
            List<UjBejelentes> lista = new List<UjBejelentes>();
            lista = JovahagyBLL.GetJovahagyAll(JovahagySzures.SelectedValue);
            Jovahagyas.DataSource = lista;
            Jovahagyas.DataBind();

            if (lista.Count == 0)
                Mentes.Visible = false;
            else
                Mentes.Visible = true;
        }

        /// <summary>
        /// Jóváhagyás szűrő DDL feltöltése
        /// </summary>
        private void DDLFeltoltes()
        {
            JovahagySzures.Items.Add(new ListItem() { Value = JovaHagyasStatus.Rogzitve.ToString(), Text = JovaHagyasStatus.Rogzitve.ToDisplayString() });
            JovahagySzures.Items.Add(new ListItem() { Value = JovaHagyasStatus.Jovahagyva.ToString(), Text = JovaHagyasStatus.Jovahagyva.ToDisplayString() });
            JovahagySzures.Items.Add(new ListItem() { Value = JovaHagyasStatus.Elutasitva.ToString(), Text = JovaHagyasStatus.Elutasitva.ToDisplayString() });
            JovahagySzures.Items.Add(new ListItem() { Value = "Mind", Text = "Mind" });
        }

        /// <summary>
        /// Új jóváhagyási státusz elmentése
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Mentes_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < Jovahagyas.Rows.Count; i++)
            {
                IOrderedDictionary rowValues = new OrderedDictionary();
                rowValues = Utility.GetValues(Jovahagyas.Rows[i]);

                string ddlSelected = (Jovahagyas.Rows[i].FindControl("StatuszDDL") as DropDownList).SelectedValue;
                int id = Convert.ToInt32(rowValues["ID"]);

                string ekezetesstatus = rowValues["JovaStatusMegjelenes"].ToString();
                string jovastatus = ((JovaHagyasStatus)Enum.Parse(
                    typeof(JovaHagyasStatus), Utility.EkezetEltavolitas(ekezetesstatus))).ToString();

                if (jovastatus != ddlSelected)
                    JovahagyBLL.JovahagyasMentes(id, ddlSelected);                
            }

            List<UjBejelentes> lista = JovahagyBLL.GetJovahagyAll(JovahagySzures.SelectedValue);
            Jovahagyas.DataSource = lista;
            Jovahagyas.DataBind();
        }

        protected void JovahagySzures_SelectedIndexChanged(object sender, EventArgs e)
        {
            GVFeltoltese();
        }
    }
}