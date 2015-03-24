using hazi.BLL;
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
    public partial class AdminPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!User.Identity.IsAuthenticated ||
                RoleActions.GetRole(User.Identity.Name) != RegisterUserAs.Admin.ToString())
            {
                Error404.HibaDobas(Response);
            }
            else
            {
                if(!IsPostBack)
                {
                    Felhasznalok.DataSource = UsersBLL.UserList();
                    Felhasznalok.DataBind(); 
                }
            }
            if (Request["__EVENTARGUMENT"] == "Felhasznalok")
            {
                Felhasznalok.Visible = true;
                Felhasznalok.DataSource = UsersBLL.UserList();
                Felhasznalok.DataBind();
            }
            else if (Request["__EVENTARGUMENT"] == "Jogcim")
            {
                List<Jogcim> lista = JogcimBLL.GetJogcimek();
                CheckBoxCheck(lista);
                JogcimekGV.DataSource = lista;
                JogcimekGV.DataBind();
            }
        }

        protected void Mentes_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(HiddenField.Value) == 0)
            {
                //ha már volt üzenet a felhasználónak
                Master.Uzenet.Visible = false;

                string uzenet = string.Empty;

                for (int i = 0; i < Felhasznalok.Rows.Count; i++)
                {
                    IOrderedDictionary rowValues = new OrderedDictionary();
                    rowValues = Utility.GetValues(Felhasznalok.Rows[i]);
                    string ddlValue = (Felhasznalok.Rows[i].FindControl("SzerepkorDDL") as DropDownList).SelectedValue;

                    //magunkat nem tudjuk megváltoztatni
                    if (rowValues["Name"].ToString() != User.Identity.Name)
                    {
                        string role = rowValues["Role"].ToString();
                        if (rowValues["Role"].ToString() != ddlValue)
                            uzenet = RoleActions.ChangeRole(rowValues["Name"].ToString(), rowValues["Role"].ToString(), ddlValue);
                    }
                    else
                    {
                        if (rowValues["Role"].ToString() != ddlValue)
                        {
                            uzenet = "Saját szerepkört nem lehet változtatni!";
                            break;
                        }
                    }
                }

                if (uzenet != string.Empty)
                {
                    Master.Uzenet.Visible = true;
                    Master.Uzenet.Text = uzenet;
                }

                Felhasznalok.DataSource = UsersBLL.UserList();
                Felhasznalok.DataBind();
            }
            else
            {
                for (int i = 0; i < JogcimekGV.Rows.Count; i++)
                {
                    IOrderedDictionary rowValues = new OrderedDictionary();
                    rowValues = Utility.GetValues(JogcimekGV.Rows[i]);

                    int id = Convert.ToInt32(rowValues["ID"]);
                    string nev = (JogcimekGV.Rows[i].FindControl("ujJogcimNev") as TextBox).Text;
                    bool inaktiv = (JogcimekGV.Rows[i].FindControl("JogcimAktiv") as CheckBox).Checked;

                    JogcimBLL.JogcimMentes(id, nev, inaktiv);
                }
                LoadJogcimekGV();
            }
        }

        private void CheckBoxCheck(List<Jogcim> lista)
        {
            for (int i = 0; i < JogcimekGV.Rows.Count; i++)
            {
                CheckBox cb = JogcimekGV.Rows[i].FindControl("JogcimAktiv") as CheckBox;
                cb.Checked = (bool)lista[i].Inaktiv;
            }
        }

        private void LoadJogcimekGV()
        {
            Felhasznalok.Visible = false; //bind miatti visszatérés miatt
            string js = string.Format("$(\"#tabs\" ).tabs(\"option\", \"active\", 1 );");
            ClientScript.RegisterStartupScript(this.GetType(), "showTab", js, true);
        }     
    }
}