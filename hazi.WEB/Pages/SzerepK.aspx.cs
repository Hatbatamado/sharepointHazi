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
    public partial class SzerepK : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                if (User.Identity.IsAuthenticated &&
                    RoleActions.GetRole(User.Identity.Name) == RegisterUserAs.Admin.ToString())
                {
                    Felhasznalok.Visible = true;
                    Felhasznalok.DataSource = UsersBLL.UserList();
                    Felhasznalok.DataBind();
                }
                else
                {
                    Response.Clear();
                    Response.StatusCode = 404;
                    Response.End();
                }
            }
        }

        protected void Mentes_Click(object sender, EventArgs e)
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
                    if (rowValues["Role"].ToString() != ddlValue)
                        uzenet = RoleActions.ChangeRole(rowValues["Name"].ToString(), rowValues["Role"].ToString(), ddlValue);
                }
                else
                {
                    uzenet = "Saját szerepkört nem lehet változtatni!";
                    break;
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
    }
}